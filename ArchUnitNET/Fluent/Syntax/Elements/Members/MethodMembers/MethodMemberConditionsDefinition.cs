using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using static ArchUnitNET.Domain.Extensions.EnumerableExtensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberConditionsDefinition
    {
        public static RelationCondition<MethodMember, MethodMember> BeMethodMembersThat()
        {
            return new RelationCondition<MethodMember, MethodMember>(
                ObjectConditionsDefinition<MethodMember>.Be,
                "be method members that",
                "are not method members that"
            );
        }

        public static IOrderedCondition<MethodMember> BeConstructor()
        {
            return new SimpleCondition<MethodMember>(
                member => member.IsConstructor(),
                "be a constructor",
                "is no constructor"
            );
        }

        public static IOrderedCondition<MethodMember> BeVirtual()
        {
            return new SimpleCondition<MethodMember>(
                member => member.IsVirtual,
                "be virtual",
                "is not virtual"
            );
        }

        public static IOrderedCondition<MethodMember> BeCalledBy(
            IObjectProvider<IType> objectProvider
        )
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<IType>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isRequiredType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                foreach (var methodMember in methodMembers)
                {
                    if (methodMember.GetCallingTypes().Any(isRequiredType))
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                                ? "is not called by one of no types (impossible)"
                                : "is not called by " + objectProvider.Description
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "be called by one of no types (impossible)",
                "be called by",
                "be called by any"
            );
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        public static IOrderedCondition<MethodMember> HaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<IType>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isRequiredType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                foreach (var methodMember in methodMembers)
                {
                    if (
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Any(isRequiredType)
                    )
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                                ? "does not have dependencies in method body to one of no types (impossible)"
                                : "does not have dependencies in method body to "
                                    + objectProvider.Description
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "have dependencies in method body to one of no types (impossible)",
                "have dependencies in method body to",
                "have dependencies in method body to any"
            );
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        public static IOrderedCondition<MethodMember> HaveReturnType(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isRequiredType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                foreach (var methodMember in methodMembers)
                {
                    if (isRequiredType(methodMember.ReturnType))
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            BuildReturnTypeFailDescription(methodMember, architecture)
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "have return type of no types (impossible)",
                "have return type",
                "have return type"
            );
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        public static IOrderedCondition<MethodMember> HaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                foreach (var methodMember in methodMembers)
                {
                    if (typeList.Any(type => methodMember.ReturnTypeInstance.MatchesType(type)))
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            BuildReturnTypeFailDescription(methodMember, architecture)
                        );
                    }
                }
            }

            var typeDescription = string.Join(
                " or ",
                typeList.Select(type => $"\"{type.GetFullNameForDescription()}\"")
            );
            var description =
                typeList.Count == 0
                    ? "have return type of no types (impossible)"
                    : $"have return type {typeDescription}";
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        //Negations

        public static IOrderedCondition<MethodMember> BeNoConstructor()
        {
            return new SimpleCondition<MethodMember>(
                member => !member.IsConstructor(),
                "be no constructor",
                "is a constructor"
            );
        }

        public static IOrderedCondition<MethodMember> NotBeVirtual()
        {
            return new SimpleCondition<MethodMember>(
                member => !member.IsVirtual,
                "not be virtual",
                "is virtual"
            );
        }

        public static IOrderedCondition<MethodMember> NotBeCalledBy(
            IObjectProvider<IType> objectProvider
        )
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<IType>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                foreach (var methodMember in methodMembers)
                {
                    if (!methodMember.GetCallingTypes().Any(isForbiddenType))
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                                ? "is called by one of no types (always true)"
                                : "is called by " + objectProvider.Description
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "not be called by one of no types (always true)",
                "not be called by",
                "not be called by any"
            );
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        public static IOrderedCondition<MethodMember> NotHaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            var sizedObjectProvider = objectProvider as ISizedObjectProvider<IType>;
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                foreach (var methodMember in methodMembers)
                {
                    if (
                        !methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Any(isForbiddenType)
                    )
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            (sizedObjectProvider != null && sizedObjectProvider.Count == 0)
                                ? "does have dependencies in method body to one of no types (always true)"
                                : "does have dependencies in method body to "
                                    + objectProvider.Description
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "not have dependencies in method body to one of no types (always true)",
                "not have dependencies in method body to",
                "not have dependencies in method body to any"
            );
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        public static IOrderedCondition<MethodMember> NotHaveReturnType(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                foreach (var methodMember in methodMembers)
                {
                    if (!isForbiddenType(methodMember.ReturnType))
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            BuildReturnTypeFailDescription(methodMember, architecture)
                        );
                    }
                }
            }

            var description = objectProvider.FormatDescription(
                "not have return type of no types (always true)",
                "not have return type",
                "not have return type"
            );
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        public static IOrderedCondition<MethodMember> NotHaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                foreach (var methodMember in methodMembers)
                {
                    if (typeList.All(type => !methodMember.ReturnTypeInstance.MatchesType(type)))
                    {
                        yield return new ConditionResult(methodMember, true);
                    }
                    else
                    {
                        yield return new ConditionResult(
                            methodMember,
                            false,
                            BuildReturnTypeFailDescription(methodMember, architecture)
                        );
                    }
                }
            }

            var typeDescription = string.Join(
                " or ",
                typeList.Select(type => $"\"{type.GetFullNameForDescription()}\"")
            );
            var description =
                typeList.Count == 0
                    ? "not have return type of no types (always true)"
                    : $"not have return type {typeDescription}";
            return new OrderedArchitectureCondition<MethodMember>(Condition, description);
        }

        private static string BuildReturnTypeFailDescription(
            MethodMember methodMember,
            Architecture architecture
        )
        {
            var returnTypeName = methodMember.ReturnTypeInstance.GetFullNameForErrorMessage(
                architecture
            );
            var ambiguityNote = methodMember.ReturnType.HasAmbiguousFullName(architecture)
                ? " (the return type's full name is ambiguous in this architecture; its assembly is shown to disambiguate it)"
                : "";
            return "has return type \"" + returnTypeName + "\"" + ambiguityNote;
        }
    }
}
