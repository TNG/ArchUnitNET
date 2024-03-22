using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain.PlantUml.Exceptions;
using static ArchUnitNET.Domain.PlantUml.Import.PlantUmlPatterns;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class PlantUmlParser
    {
        private PlantUmlPatterns _plantUmlPatterns = new PlantUmlPatterns();

        public PlantUmlParsedDiagram Parse(string filename)
        {
            if (filename is null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            return CreateDiagram(ReadLines(filename));
        }

        public PlantUmlParsedDiagram Parse(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException("stream");
            }
            return CreateDiagram(ReadLines(stream));
        }

        private IEnumerable<string> ReadLines(string filename)
        {
            try
            {
                return File.ReadAllLines(filename, Encoding.UTF8);
            }
            catch (Exception ex)
                when (ex is IOException
                    || ex is UnauthorizedAccessException
                    || ex is SecurityException
                )
            {
                throw new PlantUmlParseException("Could not parse diagram from " + filename, ex);
            }
        }

        private IEnumerable<string> ReadLines(Stream stream)
        {
            try
            {
                var lines = new List<string>();
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                return lines;
            }
            catch (Exception ex)
                when (ex is IOException
                    || ex is UnauthorizedAccessException
                    || ex is SecurityException
                )
            {
                throw new PlantUmlParseException("Could not parse diagram from stream", ex);
            }
        }

        private PlantUmlParsedDiagram CreateDiagram(IEnumerable<string> rawDiagramLines)
        {
            IEnumerable<string> diagramLines = FilterOutComments(rawDiagramLines);
            ISet<PlantUmlComponent> components = ParseComponents(diagramLines);

            var plantUmlComponents = new PlantUmlComponents(components);

            IEnumerable<ParsedDependency> dependencies = ParseDependencies(
                plantUmlComponents,
                diagramLines
            );

            return new PlantUmlDiagramBuilder(plantUmlComponents)
                .WithDependencies(dependencies)
                .Build();
        }

        private IEnumerable<ParsedDependency> ParseDependencies(
            PlantUmlComponents plantUmlComponents,
            IEnumerable<string> plantUmlDiagramLines
        )
        {
            var result = new List<ParsedDependency>();
            foreach (
                PlantUmlDependencyMatcher matcher in _plantUmlPatterns.MatchDependencies(
                    plantUmlDiagramLines
                )
            )
            {
                PlantUmlComponent origin = FindComponentMatching(
                    plantUmlComponents,
                    matcher.MatchOrigin
                );
                PlantUmlComponent target = FindComponentMatching(
                    plantUmlComponents,
                    matcher.MatchTarget
                );
                result.Add(new ParsedDependency(origin.Identifier, target.Identifier));
            }
            return result;
        }

        private PlantUmlComponent FindComponentMatching(
            PlantUmlComponents plantUmlComponents,
            string originOrTargetString
        )
        {
            originOrTargetString = Regex.Replace(originOrTargetString.Trim(), @"^\[", "");
            originOrTargetString = Regex.Replace(originOrTargetString, "]$", "");

            return plantUmlComponents.FindComponentWith(originOrTargetString);
        }

        private ISet<PlantUmlComponent> ParseComponents(IEnumerable<string> plantUmlDiagramLines)
        {
            return new HashSet<PlantUmlComponent>(
                _plantUmlPatterns
                    .FilterComponents(plantUmlDiagramLines)
                    .Select(p => CreateNewComponent(p))
                    .Distinct()
            );
        }

        private PlantUmlComponent CreateNewComponent(string input)
        {
            PlantUmlComponentMatcher matcher = _plantUmlPatterns.MatchComponent(input);

            var componentName = new ComponentName(matcher.MatchComponentName());
            ImmutableHashSet<Stereotype> immutableStereotypes = IdentifyStereotypes(
                matcher,
                componentName
            );
            string alias = matcher.MatchAlias();
            return new PlantUmlComponent(
                componentName,
                immutableStereotypes,
                alias != null ? new Alias(alias) : null
            );
        }

        private ImmutableHashSet<Stereotype> IdentifyStereotypes(
            PlantUmlComponentMatcher matcher,
            ComponentName componentName
        )
        {
            var stereotypes = ImmutableHashSet.CreateBuilder<Stereotype>();
            foreach (string stereotype in matcher.MatchStereoTypes())
            {
                stereotypes.Add(new Stereotype(stereotype));
            }

            ImmutableHashSet<Stereotype> result = stereotypes.ToImmutable();
            if (result.IsEmpty)
            {
                throw new IllegalDiagramException(
                    string.Format(
                        "Components must include at least one stereotype"
                            + " specifying the namespace identifier (<<.*>>), but component '{0}' does not",
                        componentName.AsString()
                    )
                );
            }
            return result;
        }

        private IEnumerable<string> FilterOutComments(IEnumerable<string> lines)
        {
            return lines.Where(l => !Regex.IsMatch(l, "^\\s*'")).ToList();
        }
    }
}
