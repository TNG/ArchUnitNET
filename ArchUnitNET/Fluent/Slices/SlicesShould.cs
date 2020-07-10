//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
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

        public SliceRule BeFreeOfCycles()
        {
            IEnumerable<EvaluationResult> Evaluate(IEnumerable<Slice> slices, ICanBeEvaluated archRule,
                Architecture architecture)
            {
                var slicesList = slices.ToList();

                IEnumerable<Slice> FindDependencies(Slice slice)
                {
                    var typeDependencies = slice.Dependencies.Select(dependency => dependency.Target).Distinct();
                    return typeDependencies.Select(type => slicesList.FirstOrDefault(slc => slc.Types.Contains(type)))
                        .Where(slc => slc != null);
                }

                var cycles = slicesList.DetectCycles(FindDependencies)
                    .Where(dependencyCycle => dependencyCycle.IsCyclic);

                return from slice in slicesList
                    let passed = cycles.SelectMany(cycle => cycle.Contents).Contains(slice)
                    let description = slice.Description + (passed ? " does not have" : " does have") +
                                      " cyclic dependencies."
                    select new EvaluationResult(slice, passed, description, archRule, architecture);
            }

            _ruleCreator.SetEvaluationFunction(Evaluate);
            _ruleCreator.AddToDescription("be free of cycles");
            return new SliceRule(_ruleCreator);
        }

        public SliceRule NotDependOnEachOther()
        {
            IEnumerable<EvaluationResult> Evaluate(IEnumerable<Slice> slices, ICanBeEvaluated archRule,
                Architecture architecture)
            {
                var slicesList = slices.ToList();

                IEnumerable<Slice> FindDependencies(Slice slice)
                {
                    var typeDependencies = slice.Dependencies.Select(dependency => dependency.Target).Distinct();
                    return typeDependencies.Select(type => slicesList.FirstOrDefault(slc => slc.Types.Contains(type)))
                        .Where(slc => slc != null);
                }

                return from slice in slicesList
                    let dependencies = FindDependencies(slice)
                    let passed = !dependencies.Any()
                    let description = slice.Description + (passed
                        ? " does not depend on another slice."
                        : " does depend on " + dependencies.First().Description + dependencies.Skip(1)
                            .Aggregate("",
                                (str, dependency) => str + " and " + dependency.Description)) + "."
                    select new EvaluationResult(slice, passed, description, archRule, architecture);
            }

            _ruleCreator.SetEvaluationFunction(Evaluate);
            _ruleCreator.AddToDescription("not depend on each other");
            return new SliceRule(_ruleCreator);
        }
    }
}