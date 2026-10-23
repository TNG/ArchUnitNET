using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain.PlantUml.Exceptions;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class PlantUmlParser
    {
        public PlantUmlParsedDiagram Parse(string filename)
        {
            if (filename is null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            return CreateDiagram(
                ReadLines(() => new StreamReader(filename, Encoding.UTF8), filename)
            );
        }

        public PlantUmlParsedDiagram Parse(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            return CreateDiagram(
                ReadLines(() => new StreamReader(stream, Encoding.UTF8), "stream")
            );
        }

        private static IEnumerable<string> ReadLines(Func<TextReader> open, string source)
        {
            try
            {
                var lines = new List<string>();
                using (TextReader reader = open())
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
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
                throw new PlantUmlParseException("Could not parse diagram from " + source, ex);
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
                PlantUmlDependencyMatcher matcher in plantUmlDiagramLines.SelectMany(
                    PlantUmlDependencyMatcher.TryParse
                )
            )
            {
                PlantUmlComponent origin = FindComponentMatching(
                    plantUmlComponents,
                    matcher.Origin
                );
                PlantUmlComponent target = FindComponentMatching(
                    plantUmlComponents,
                    matcher.Target
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
                plantUmlDiagramLines
                    .Select(PlantUmlComponentMatcher.TryParse)
                    .Where(matcher => matcher != null)
                    .Select(CreateNewComponent)
            );
        }

        private PlantUmlComponent CreateNewComponent(PlantUmlComponentMatcher matcher)
        {
            var componentName = new ComponentName(matcher.Name);
            ImmutableHashSet<Stereotype> immutableStereotypes = IdentifyStereotypes(
                matcher,
                componentName
            );
            return new PlantUmlComponent(
                componentName,
                immutableStereotypes,
                matcher.Alias != null ? new Alias(matcher.Alias) : null
            );
        }

        private ImmutableHashSet<Stereotype> IdentifyStereotypes(
            PlantUmlComponentMatcher matcher,
            ComponentName componentName
        )
        {
            ImmutableHashSet<Stereotype> result = matcher
                .Stereotypes.Select(stereotype => new Stereotype(stereotype))
                .ToImmutableHashSet();
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
            return lines
                .Where(l => !l.TrimStart().StartsWith("'", StringComparison.Ordinal))
                .ToList();
        }
    }
}
