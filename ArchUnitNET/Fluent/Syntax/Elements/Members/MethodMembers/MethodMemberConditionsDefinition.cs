using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;

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

        public static ICondition<MethodMember> BeConstructor()
        {
            return new SimpleCondition<MethodMember>(
                member => member.IsConstructor(),
                "be a constructor",
                "is no constructor"
            );
        }

        public static ICondition<MethodMember> BeVirtual()
        {
            return new SimpleCondition<MethodMember>(
                member => member.IsVirtual,
                "be virtual",
                "is not virtual"
            );
        }

        public static ICondition<MethodMember> BeCalledBy(IType firstType, params IType[] moreTypes)
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return BeCalledBy(types);
        }

        public static ICondition<MethodMember> BeCalledBy(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return BeCalledBy(types);
        }

        public static ICondition<MethodMember> BeCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is not called by " + objectProvider.Description;
                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "be called by " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> BeCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is not called by one of no types (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => !type.Equals(firstType))
                        .Distinct()
                        .Aggregate(
                            "is not called by \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
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
                description = "be called by one of no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "be called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> BeCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
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
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember.GetCallingTypes().Intersect(archUnitTypeList).Any()
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is not called by one of no types (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => type != firstType)
                        .Distinct()
                        .Aggregate(
                            "is not called by \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
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
                description = "be called by one of no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "be called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Intersect(typeList)
                            .Any()
                    )
                    .ToList();
                var failDescription =
                    "does not have dependencies in method body to " + objectProvider.Description;
                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "have dependencies in method body to " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(
            IEnumerable<IType> types
        )
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Intersect(typeList)
                            .Any()
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription =
                        "does not have dependencies in method body to one of no types (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => !type.Equals(firstType))
                        .Distinct()
                        .Aggregate(
                            "does not have dependencies in method body to \""
                                + firstType.FullName
                                + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
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
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
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
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Intersect(archUnitTypeList)
                            .Any()
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription =
                        "does not have dependencies in method body to one of no types (always true)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => type != firstType)
                        .Distinct()
                        .Aggregate(
                            "does not have dependencies in method body to \""
                                + firstType.FullName
                                + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
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
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return HaveReturnType(types);
        }

        public static ICondition<MethodMember> HaveReturnType(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.FullName).ToList();
            var description =
                "have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            bool Condition(MethodMember member)
            {
                return typeList.Any(type => member.ReturnType.FullNameEquals(type.FullName));
            }

            return new SimpleCondition<MethodMember>(
                Condition,
                member => "has return type \"" + member.ReturnType.FullName + "\"",
                description
            );
        }

        public static ICondition<MethodMember> HaveReturnType(IObjectProvider<IType> types)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = types.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember =>
                        typeList.Any(type => methodMember.ReturnType.FullNameEquals(type.FullName))
                    )
                    .ToList();
                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "has return type \"" + failedObject.ReturnType.FullName + "\""
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "have return type " + types.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> HaveReturnType(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return HaveReturnType(types);
        }

        public static ICondition<MethodMember> HaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.ToString()).ToList();
            var description =
                "have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            bool Condition(MethodMember member)
            {
                return typeList.Any(type => member.ReturnTypeInstance.MatchesType(type));
            }

            return new SimpleCondition<MethodMember>(
                Condition,
                member => "has return type \"" + member.ReturnType.FullName + "\"",
                description
            );
        }

        //Negations

        public static ICondition<MethodMember> BeNoConstructor()
        {
            return new SimpleCondition<MethodMember>(
                member => !member.IsConstructor(),
                "be no constructor",
                "is a constructor"
            );
        }

        public static ICondition<MethodMember> NotBeVirtual()
        {
            return new SimpleCondition<MethodMember>(
                member => !member.IsVirtual,
                "not be virtual",
                "is virtual"
            );
        }

        public static ICondition<MethodMember> NotBeCalledBy(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return NotBeCalledBy(types);
        }

        public static ICondition<MethodMember> NotBeCalledBy(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotBeCalledBy(types);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                var failDescription = "is called by " + objectProvider.Description;
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not be called by " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember => methodMember.GetCallingTypes().Intersect(typeList).Any())
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is called by one of no types (always false)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => !type.Equals(firstType))
                        .Distinct()
                        .Aggregate(
                            "is called by \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be called by one of no types (always true)";
            }
            else
            {
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "not be called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotBeCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
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
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember.GetCallingTypes().Intersect(archUnitTypeList).Any()
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription = "is called by one of no types (always false)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => type != firstType)
                        .Distinct()
                        .Aggregate(
                            "is called by \"" + firstType.FullName + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "not be called by one of no types (always true)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "not be called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return NotHaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotHaveDependencyInMethodBodyTo(types);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = objectProvider.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Intersect(typeList)
                            .Any()
                    )
                    .ToList();
                var failDescription =
                    "does have dependencies in method body to " + objectProvider.Description;
                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description =
                "not have dependencies in method body to " + objectProvider.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(
            IEnumerable<IType> types
        )
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(IEnumerable<MethodMember> methodMembers)
            {
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Intersect(typeList)
                            .Any()
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription =
                        "does have dependencies in method body to one of no types (always false)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => !Equals(type, firstType))
                        .Distinct()
                        .Aggregate(
                            "does have dependencies in method body to \""
                                + firstType.FullName
                                + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description =
                    "not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                description = typeList
                    .Where(type => !Equals(type, firstType))
                    .Distinct()
                    .Aggregate(
                        "not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerableCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(
            IEnumerable<Type> types
        )
        {
            var typeList = types.ToList();
            var firstType = typeList.First();

            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
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
                var methodMemberList = methodMembers.ToList();
                var failedObjects = methodMemberList
                    .Where(methodMember =>
                        methodMember
                            .GetBodyTypeMemberDependencies()
                            .Select(dependency => dependency.Target)
                            .Intersect(archUnitTypeList)
                            .Any()
                    )
                    .ToList();
                string failDescription;
                if (typeList.IsNullOrEmpty())
                {
                    failDescription =
                        "does have dependencies in method body to one of no types (always false)";
                }
                else
                {
                    failDescription = typeList
                        .Where(type => type != firstType)
                        .Distinct()
                        .Aggregate(
                            "does have dependencies in method body to \""
                                + firstType.FullName
                                + "\"",
                            (current, type) => current + " or \"" + type.FullName + "\""
                        );
                }

                foreach (var failedObject in failedObjects)
                {
                    yield return new ConditionResult(failedObject, false, failDescription);
                }

                foreach (var passedObject in methodMemberList.Except(failedObjects))
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description =
                    "not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return NotHaveReturnType(types);
        }

        public static ICondition<MethodMember> NotHaveReturnType(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.FullName).ToList();
            var description =
                "not have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            bool Condition(MethodMember member)
            {
                return typeList.All(type => !member.ReturnType.FullNameEquals(type.FullName));
            }

            return new SimpleCondition<MethodMember>(
                Condition,
                member => "has return type \"" + member.ReturnType.FullName + "\"",
                description
            );
        }

        public static ICondition<MethodMember> NotHaveReturnType(IObjectProvider<IType> types)
        {
            IEnumerable<ConditionResult> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = types.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                var passedObjects = methodMemberList
                    .Where(methodMember =>
                        typeList.All(type => !methodMember.ReturnType.FullNameEquals(type.FullName))
                    )
                    .ToList();
                foreach (var failedObject in methodMemberList.Except(passedObjects))
                {
                    yield return new ConditionResult(
                        failedObject,
                        false,
                        "has return type \"" + failedObject.ReturnType.FullName + "\""
                    );
                }

                foreach (var passedObject in passedObjects)
                {
                    yield return new ConditionResult(passedObject, true);
                }
            }

            var description = "not have return type " + types.Description;
            return new ArchitectureCondition<MethodMember>(Condition, description);
        }

        public static ICondition<MethodMember> NotHaveReturnType(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return NotHaveReturnType(types);
        }

        public static ICondition<MethodMember> NotHaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.ToString()).ToList();
            var description =
                "not have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            bool Condition(MethodMember member)
            {
                return typeList.All(type => !member.ReturnTypeInstance.MatchesType(type));
            }

            return new SimpleCondition<MethodMember>(
                Condition,
                member => "has return type \"" + member.ReturnType.FullName + "\"",
                description
            );
        }
        
        /// <summary>
        /// Selects method members that have any parameters
        /// </summary>
        /// <returns>A condition that can be applied to method members</returns>
        public static ICondition<MethodMember> HaveAnyParameters()
        {
            return new SimpleCondition<MethodMember>(
                method => method.Parameters.Any(),
                "have any parameters",
                "does not have any parameters"
            );
        }

        /// <summary>
        /// Selects method members that do not have any parameters (parameterless)
        /// </summary>
        /// <returns>A condition that can be applied to method members</returns>
        public static ICondition<MethodMember> NotHaveAnyParameters()
        {
            return new SimpleCondition<MethodMember>(
                method => !method.Parameters.Any(),
                "not have any parameters",
                "has parameters"
            );
        }
    }
}
