using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArchUnitNET.PlantUml
{
    internal class PlantUmlPatterns
    {
        private static readonly string ComponentNameGroupName = "componentName";
        private static readonly string ComponentNameFormat;
        private static readonly Regex StereotypePattern;
        private static readonly string StereotypeFormat;
        private static readonly string AliasGroupName = "alias";
        private static readonly string AliasFormat;
        private static readonly Regex PlantUmlComponentPattern;

        static PlantUmlPatterns()
        {
            StereotypeFormat = "(?:<<" + Capture(AnythingBut("<>")) + ">>\\s*)";
            ComponentNameFormat = "\\[" + Capture(AnythingBut("\\[\\]"), ComponentNameGroupName) + "]";
            StereotypePattern = new Regex(StereotypeFormat);
            AliasFormat = "\\s*(?:as \"?" + Capture("[^\"]+", AliasGroupName) + "\"?)?";
            PlantUmlComponentPattern = new Regex("^\\s*" + ComponentNameFormat + "\\s*" + StereotypeFormat + "*" + AliasFormat + "\\s*");
        }

        private static string Capture(string pattern)
        {
            return "(" + pattern + ")";
        }

        private static string Capture(string pattern, string name)
        {
            return "(?<" + name + ">" + pattern + ")";
        }

        private static string AnythingBut(string charsJoined)
        {
            return "[^" + charsJoined + "]+";
        }

        internal IEnumerable<string> FilterComponents(IEnumerable<string> plantUmlDiagramLines)
        {
            return plantUmlDiagramLines.Where(l => PlantUmlComponentPattern.IsMatch(l));
        }

        internal IEnumerable<PlantUmlDependencyMatcher> MatchDependencies(IEnumerable<string> diagramLines)
        {
            var result = new List<PlantUmlDependencyMatcher>();
            foreach (string line in diagramLines)
            {
                result.AddRange(PlantUmlDependencyMatcher.TryParseFromLeftToRight(line));
                result.AddRange(PlantUmlDependencyMatcher.TryParseFromRightToLeft(line));
            }
            return result;
        }

        internal PlantUmlComponentMatcher MatchComponent(string input)
        {
            return new PlantUmlComponentMatcher(input);
        }

        internal class PlantUmlDependencyMatcher
        {
            private static readonly string ColorRegex = "\\[[^]]+]"; // for arrows like '--[#green]->'
            private static readonly string DeppendencyArrowCenterRegex = "(left|right|up|down|" + ColorRegex + ")?";
            private static readonly Regex DependencyRightArrowPattern = new Regex("\\s-+" + DeppendencyArrowCenterRegex + "-*>\\s");
            private static readonly Regex DependencyLeftArrowPattern = new Regex("\\s<-*" + DeppendencyArrowCenterRegex + "-+\\s");
            public PlantUmlDependencyMatcher(string origin, string target)
            {
                if (origin is null)
                {
                    throw new ArgumentNullException(nameof(origin));
                }

                if (target is null)
                {
                    throw new ArgumentNullException(nameof(target));
                }

                MatchOrigin = origin;
                MatchTarget = target;
            }

            public string MatchTarget { get; private set; }
            public string MatchOrigin { get; private set; }

            internal static IEnumerable<PlantUmlDependencyMatcher> TryParseFromLeftToRight(string line)
            {
                return IsDependencyFromLeftToRight(line)
                    ? ImmutableList.CreateRange(new List<PlantUmlDependencyMatcher> { ParseDependencyFromLeftToRight(line)})
                    : Enumerable.Empty<PlantUmlDependencyMatcher>();
            }

            internal static IEnumerable<PlantUmlDependencyMatcher> TryParseFromRightToLeft(string line)
            {
                return IsDependencyFromRightToLeft(line)
                    ? ImmutableList.CreateRange(new List<PlantUmlDependencyMatcher> { ParseDependencyFromRightToLeft(line) })
                    : Enumerable.Empty<PlantUmlDependencyMatcher>();
            }

            private static PlantUmlDependencyMatcher ParseDependencyFromRightToLeft(string line)
            {
                IList<string> parts = ParseParts(line, DependencyLeftArrowPattern);
                return new PlantUmlDependencyMatcher(parts[1], parts[0]);
            }

            private static bool IsDependencyFromRightToLeft(string line)
            {
                return DependencyLeftArrowPattern.IsMatch(line);
            }

            private static bool IsDependencyFromLeftToRight(string line)
            {
                return DependencyRightArrowPattern.IsMatch(line);
            }

            private static PlantUmlDependencyMatcher ParseDependencyFromLeftToRight(string line)
            {
                IList<string> parts = ParseParts(line, DependencyRightArrowPattern);
                return new PlantUmlDependencyMatcher(parts[0], parts[1]);
            }

            private static IList<string> ParseParts(string line, Regex dependencyRightArrowPattern)
            {
                line = RemoveOptionalDescription(line);
                return dependencyRightArrowPattern.Split(line).Select(l => l.Trim()).Take(2).ToList();
            }

            private static string RemoveOptionalDescription(string line)
            {
                return line.Replace(":.*", "");
            }
        }

        internal class PlantUmlComponentMatcher
        {
            private Match _componentMatch;
            private Match _stereotypeMatch;
            public PlantUmlComponentMatcher(string input)
            {
                _componentMatch = PlantUmlComponentPattern.Match(input);
                if (!_componentMatch.Success)
                {
                    throw new InvalidOperationException(string.Format("input {0} does not match pattern {1}", input, PlantUmlComponentPattern));
                }
                _stereotypeMatch = StereotypePattern.Match(input);
            }

            internal string MatchAlias()
            {
                //TODO what if not matched?
                return _componentMatch.Groups[AliasGroupName].Value;
            }

            internal string MatchComponentName()
            {
                return _componentMatch.Groups[ComponentNameGroupName].Value;
            }

            internal ISet<string> MatchStereoTypes()
            {
                ISet<string> result = new HashSet<string>();
                while (_stereotypeMatch.Success)
                {
                    result.Add(_stereotypeMatch.Groups[1].Value);
                    _stereotypeMatch = _stereotypeMatch.NextMatch();
                }
                return result;
            }
        }
    }
}