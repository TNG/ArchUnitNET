//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Library.Rules
{
    public static class TargetFrameworkRules
    {
        public static IArchRule ShouldBeDotNetStandard()
        {
            IEnumerable<EvaluationResult> Evaluate(Architecture architecture, IArchRule archRule)
            {
                foreach (var assembly in architecture.Assemblies)
                {
                    var passed = assembly.IsDotNetStandard();
                    var description = "has target framework " + assembly.GetTargetFramework();
                    yield return new EvaluationResult(
                        assembly,
                        new StringIdentifier(assembly.FullName),
                        passed,
                        description,
                        archRule,
                        architecture
                    );
                }
            }

            return new CustomArchRule(Evaluate, "Should be .Net Standard");
        }

        public static IArchRule ShouldBeDotNetCore()
        {
            IEnumerable<EvaluationResult> Evaluate(Architecture architecture, IArchRule archRule)
            {
                foreach (var assembly in architecture.Assemblies)
                {
                    var passed = assembly.IsDotNetCore();
                    var description = "has target framework " + assembly.GetTargetFramework();
                    yield return new EvaluationResult(
                        assembly,
                        new StringIdentifier(assembly.FullName),
                        passed,
                        description,
                        archRule,
                        architecture
                    );
                }
            }

            return new CustomArchRule(Evaluate, "Should be .Net Core");
        }

        public static IArchRule ShouldBeDotNetFramework()
        {
            IEnumerable<EvaluationResult> Evaluate(Architecture architecture, IArchRule archRule)
            {
                foreach (var assembly in architecture.Assemblies)
                {
                    var passed = assembly.IsDotNetFramework();
                    var description = "has target framework " + assembly.GetTargetFramework();
                    yield return new EvaluationResult(
                        assembly,
                        new StringIdentifier(assembly.FullName),
                        passed,
                        description,
                        archRule,
                        architecture
                    );
                }
            }

            return new CustomArchRule(Evaluate, "Should be .Net Framework");
        }
    }
}
