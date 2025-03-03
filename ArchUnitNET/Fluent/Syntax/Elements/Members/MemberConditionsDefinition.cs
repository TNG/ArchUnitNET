using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberConditionsDefinition<TRuleType>
        where TRuleType : IMember
    {
        public static ICondition<TRuleType> BeDeclaredIn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            return new SimpleCondition<TRuleType>(
                member => member.IsDeclaredIn(pattern, useRegularExpressions),
                member => "is declared in " + member.DeclaringType.FullName,
                "be declared in types with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> BeDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.Any(pattern =>
                    ruleType.IsDeclaredIn(pattern, useRegularExpressions)
                );
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList
                    .Where(obj => !obj.Equals(firstPattern))
                    .Distinct()
                    .Aggregate(
                        "be declared in types with full name "
                            + (useRegularExpressions ? "matching " : "")
                            + "\""
                            + firstPattern
                            + "\"",
                        (current, pattern) => current + " or \"" + pattern + "\""
                    );
            }

            return new SimpleCondition<TRuleType>(
                Condition,
                member => "is declared in " + member.DeclaringType.FullName,
                description
            );
        }

        public static ICondition<TRuleType> BeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return BeDeclaredIn(types);
        }

        public static ICondition<TRuleType> BeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return BeDeclaredIn(types);
        }

        public static ICondition<TRuleType> BeDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> methods,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodList = methods.ToList();
                var passedObjects = methodList
                    .Where(method => typeList.Contains(method.DeclaringType))
                    .ToList();
                foreach (var failedObject in methodList.Except(passedObjects))
                {
                    var failDescription = "is declared in " + failedObject.DeclaringType.FullName;
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "be declared in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> methods)
            {
                var methodList = methods.ToList();
                var passedObjects = methodList
                    .Where(method => typeList.Contains(method.DeclaringType))
                    .ToList();
                foreach (var failedObject in methodList.Except(passedObjects))
                {
                    var failDescription = "is declared in " + failedObject.DeclaringType.FullName;
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => !obj.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "be declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> methods,
                Architecture architecture
            )
            {
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var methodList = methods.ToList();
                var passedObjects = methodList
                    .Where(method => archUnitTypeList.Contains(method.DeclaringType))
                    .ToList();
                foreach (var failedObject in methodList.Except(passedObjects))
                {
                    var failDescription = "is declared in " + failedObject.DeclaringType.FullName;
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "be declared in no type (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => obj != firstType)
                    .Distinct()
                    .Aggregate(
                        "be declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> BeStatic()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.IsStatic.HasValue || member.IsStatic.Value,
                "be static",
                "is not static"
            );
        }

        public static ICondition<TRuleType> BeReadOnly()
        {
            return new SimpleCondition<TRuleType>(
                member => member.Writability == Writability.ReadOnly,
                "be read only",
                "is not read only"
            );
        }

        public static ICondition<TRuleType> BeImmutable()
        {
            return new SimpleCondition<TRuleType>(
                member => member.Writability.IsImmutable(),
                "be immutable",
                "is not immutable"
            );
        }

        //Relation Conditions


        public static RelationCondition<TRuleType, IType> BeDeclaredInTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                BeDeclaredIn,
                "be declared in types that",
                "is not declared in types that"
            );
        }

        //Negations


        public static ICondition<TRuleType> NotBeDeclaredIn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            return new SimpleCondition<TRuleType>(
                member => !member.IsDeclaredIn(pattern, useRegularExpressions),
                member => "is declared in " + member.DeclaringType.FullName,
                "not be declared in types with full name "
                    + (useRegularExpressions ? "matching " : "")
                    + "\""
                    + pattern
                    + "\""
            );
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            var patternList = patterns.ToList();

            bool Condition(TRuleType ruleType)
            {
                return patternList.All(pattern =>
                    !ruleType.IsDeclaredIn(pattern, useRegularExpressions)
                );
            }

            string description;
            if (patternList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
            }
            else
            {
                var firstPattern = patternList.First();
                description = patternList
                    .Where(obj => !obj.Equals(firstPattern))
                    .Distinct()
                    .Aggregate(
                        "not be declared in types with full name "
                            + (useRegularExpressions ? "matching " : "")
                            + "\""
                            + firstPattern
                            + "\"",
                        (current, pattern) => current + " or \"" + pattern + "\""
                    );
            }

            return new SimpleCondition<TRuleType>(
                Condition,
                member => "is declared in " + member.DeclaringType.FullName,
                description
            );
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return NotBeDeclaredIn(types);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotBeDeclaredIn(types);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> methods,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodList = methods.ToList();
                var failedObjects = methodList
                    .Where(method => typeList.Contains(method.DeclaringType))
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var failDescription = "is declared in " + failedObject.DeclaringType.FullName;
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not be declared in " + objectProvider.Description;
            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(IEnumerable<TRuleType> methods)
            {
                var methodList = methods.ToList();
                var failedObjects = methodList
                    .Where(method => typeList.Contains(method.DeclaringType))
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var failDescription = "is declared in " + failedObject.DeclaringType.FullName;
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => !obj.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "not be declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeDeclaredIn(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<TRuleType> methods,
                Architecture architecture
            )
            {
                var archUnitTypeList = new List<IType>();
                foreach (var type in typeList)
                {
                    try
                    {
                        var archUnitType = architecture.GetITypeOfType(type);
                        archUnitTypeList.Add(archUnitType);
                    }
                    catch (TypeDoesNotExistInArchitecture)
                    {
                        //ignore, can't have a dependency anyways
                    }
                }

                var methodList = methods.ToList();
                var failedObjects = methodList
                    .Where(method => archUnitTypeList.Contains(method.DeclaringType))
                    .ToList();
                foreach (var failedObject in failedObjects)
                {
                    var failDescription = "is declared in " + failedObject.DeclaringType.FullName;
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be declared in no type (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(obj => obj != firstType)
                    .Distinct()
                    .Aggregate(
                        "not be declared in \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<TRuleType>(Condition, description);
        }

        public static ICondition<TRuleType> NotBeStatic()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.IsStatic.HasValue || !member.IsStatic.Value,
                "not be static",
                "is static"
            );
        }

        public static ICondition<TRuleType> NotBeReadOnly()
        {
            return new SimpleCondition<TRuleType>(
                member => member.Writability != Writability.ReadOnly,
                "not be read only",
                "is read only"
            );
        }

        public static ICondition<TRuleType> NotBeImmutable()
        {
            return new SimpleCondition<TRuleType>(
                member => !member.Writability.IsImmutable(),
                "not be immutable",
                "is immutable"
            );
        }

        //Relation Condition Negations


        public static RelationCondition<TRuleType, IType> NotBeDeclaredInTypesThat()
        {
            return new RelationCondition<TRuleType, IType>(
                NotBeDeclaredIn,
                "not be declared in types that",
                "is declared in types that"
            );
        }
    }
}
