using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
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
            (ISet<PlantUmlComponent> components, IList<PlantUmlGroup> groups) = ParseComponents(
                diagramLines
            );

            var plantUmlComponents = new PlantUmlComponents(components);

            IEnumerable<ParsedDependency> dependencies = ParseDependencies(
                plantUmlComponents,
                groups,
                diagramLines
            );

            return new PlantUmlDiagramBuilder(plantUmlComponents)
                .WithDependencies(dependencies)
                .Build();
        }

        private IEnumerable<ParsedDependency> ParseDependencies(
            PlantUmlComponents plantUmlComponents,
            IList<PlantUmlGroup> groups,
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
                foreach (
                    ComponentIdentifier origin in ResolveEndpoint(
                        plantUmlComponents,
                        groups,
                        matcher.Origin
                    )
                )
                {
                    foreach (
                        ComponentIdentifier target in ResolveEndpoint(
                            plantUmlComponents,
                            groups,
                            matcher.Target
                        )
                    )
                    {
                        if (!origin.Equals(target))
                        {
                            result.Add(new ParsedDependency(origin, target));
                        }
                    }
                }
            }
            return result;
        }

        // A dependency endpoint is either a component or a group. A group stands for
        // all components nested inside it, unless the endpoint is written in brackets,
        // which always refers to a component.
        private IEnumerable<ComponentIdentifier> ResolveEndpoint(
            PlantUmlComponents plantUmlComponents,
            IList<PlantUmlGroup> groups,
            PlantUmlEndpoint endpoint
        )
        {
            if (endpoint.MayReferToGroup)
            {
                PlantUmlGroup group = groups.FirstOrDefault(g => g.IsReferencedBy(endpoint.Name));
                if (group != null)
                {
                    return group.Members;
                }
            }
            return new[] { plantUmlComponents.FindComponentWith(endpoint.Name).Identifier };
        }

        private (ISet<PlantUmlComponent> Components, IList<PlantUmlGroup> Groups) ParseComponents(
            IEnumerable<string> plantUmlDiagramLines
        )
        {
            var components = new HashSet<PlantUmlComponent>();
            var groups = new List<PlantUmlGroup>();
            // Every '{' opens a block, but only some blocks are groups (others are e.g. skinparam),
            // so non-group blocks are tracked as null to keep braces balanced.
            var openBlocks = new Stack<PlantUmlGroup>();
            foreach (string line in plantUmlDiagramLines)
            {
                if (PlantUmlGroupMatcher.TryParse(line) is PlantUmlGroupMatcher groupMatcher)
                {
                    var group = new PlantUmlGroup(groupMatcher.Name, groupMatcher.Alias);
                    groups.Add(group);
                    openBlocks.Push(group);
                }
                else if (PlantUmlPatterns.IsBlockOpen(line))
                {
                    openBlocks.Push(null);
                }
                else if (PlantUmlPatterns.IsBlockClose(line))
                {
                    if (openBlocks.Count > 0)
                    {
                        openBlocks.Pop();
                    }
                }
                else if (
                    PlantUmlComponentMatcher.TryParse(line) is PlantUmlComponentMatcher matcher
                )
                {
                    PlantUmlComponent component = CreateNewComponent(matcher);
                    components.Add(component);
                    foreach (PlantUmlGroup group in openBlocks.Where(g => g != null))
                    {
                        group.AddMember(component.Identifier);
                    }
                }
            }
            PlantUmlGroup unclosedGroup = openBlocks.FirstOrDefault(g => g != null);
            if (unclosedGroup != null)
            {
                throw new IllegalDiagramException(
                    string.Format("Group '{0}' is never closed", unclosedGroup.Name)
                );
            }
            return (components, groups);
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
