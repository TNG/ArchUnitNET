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
                        new FrozenRuleIdentifier(assembly.FullName),
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
                        new FrozenRuleIdentifier(assembly.FullName),
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
                        new FrozenRuleIdentifier(assembly.FullName),
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
