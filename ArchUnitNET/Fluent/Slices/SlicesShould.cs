using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Freeze;
using StronglyConnectedComponents;

namespace ArchUnitNET.Fluent.Slices
{
    public class SlicesShould
    {
        private readonly SliceRuleCreator _ruleCreator;

        public SlicesShould(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        private static IEnumerable<Slice> FindDependencies(
            Slice slice,
            IEnumerable<Slice> otherSlices
        )
        {
            var typeDependencies = slice
                .Dependencies.Select(dependency => dependency.Target)
                .Distinct();
            return typeDependencies
                .Select(type => otherSlices.FirstOrDefault(slc => slc.Types.Contains(type)))
                .Where(slc => slc != null);
        }

        public SliceRule BeFreeOfCycles()
        {
            _ruleCreator.SetEvaluationFunction(EvaluateBeFreeOfCycles);
            _ruleCreator.AddToDescription("be free of cycles");
            return new SliceRule(_ruleCreator);
        }

        private static IEnumerable<EvaluationResult> EvaluateBeFreeOfCycles(
            IEnumerable<Slice> slices,
            ICanBeEvaluated archRule,
            Architecture architecture
        )
        {
            var slicesList = slices.ToList();

            var cycles = slicesList
                .DetectCycles(slc => FindDependencies(slc, slicesList))
                .Where(dependencyCycle => dependencyCycle.IsCyclic)
                .ToList();

            if (cycles.Any())
            {
                foreach (var cycle in cycles)
                {
                    var depsBetweenSlices = new List<List<string>>();
                    foreach (var slice in cycle.Contents)
                    {
                        var dependencies = slice.Dependencies.ToList();
                        foreach (var otherSlice in cycle.Contents.Except(new[] { slice }))
                        {
                            var depsToSlice = dependencies
                                .Where(dependency => otherSlice.Types.Contains(dependency.Target))
                                .Distinct(new TypeDependencyComparer())
                                .ToList();
                            if (depsToSlice.Any())
                            {
                                var depsFromThisSliceToOtherSlice = new List<string>
                                {
                                    slice.Description + " -> " + otherSlice.Description,
                                };
                                depsFromThisSliceToOtherSlice.AddRange(
                                    depsToSlice.Select(dependency =>
                                        "\t" + dependency.Origin + " -> " + dependency.Target
                                    )
                                );
                                depsBetweenSlices.Add(depsFromThisSliceToOtherSlice);
                            }
                        }
                    }

                    var description = new StringBuilder();
                    description.AppendLine("Cycle found:");
                    var orderedDeps = depsBetweenSlices.OrderBy(l => l.Count);
                    foreach (var lines in orderedDeps)
                    {
                        foreach (var line in lines)
                        {
                            description.AppendLine(line);
                        }
                    }

                    var cycleIdentifier = new FrozenRuleEnumerableIdentifier(
                        cycle.Contents.Select(slice => slice.Identifier)
                    );
                    yield return new EvaluationResult(
                        cycle,
                        cycleIdentifier,
                        false,
                        description.ToString(),
                        archRule,
                        architecture
                    );
                }
            }
            else
            {
                var slicesIdentifier = new FrozenRuleEnumerableIdentifier(
                    slicesList.Select(slice => slice.Identifier)
                );
                yield return new EvaluationResult(
                    slicesList,
                    slicesIdentifier,
                    true,
                    "All Slices are free of cycles.",
                    archRule,
                    architecture
                );
            }
        }

        public SliceRule NotDependOnEachOther()
        {
            _ruleCreator.SetEvaluationFunction(EvaluateNotDependOnEachOther);
            _ruleCreator.AddToDescription("not depend on each other");
            return new SliceRule(_ruleCreator);
        }

        private static IEnumerable<EvaluationResult> EvaluateNotDependOnEachOther(
            IEnumerable<Slice> slices,
            ICanBeEvaluated archRule,
            Architecture architecture
        )
        {
            var slicesList = slices.ToList();

            foreach (var slice in slicesList)
            {
                var sliceDependencies = FindDependencies(slice, slicesList)
                    .Except(new[] { slice })
                    .ToList();
                var passed = !sliceDependencies.Any();
                var description = slice.Description + " does not depend on another slice.";
                if (!passed)
                {
                    description = slice.Description + " does depend on other slices:";
                    foreach (var sliceDependency in sliceDependencies)
                    {
                        var depsToSlice = slice
                            .Dependencies.Where(dependency =>
                                sliceDependency.Types.Contains(dependency.Target)
                            )
                            .Distinct(new TypeDependencyComparer())
                            .ToList();
                        description +=
                            "\n" + slice.Description + " -> " + sliceDependency.Description;
                        description = depsToSlice.Aggregate(
                            description,
                            (current, dependency) =>
                                current + ("\n\t" + dependency.Origin + " -> " + dependency.Target)
                        );
                    }

                    description += "\n";
                }

                yield return new EvaluationResult(
                    slice,
                    slice.Identifier,
                    passed,
                    description,
                    archRule,
                    architecture
                );
            }
        }
    }
}
