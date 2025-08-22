using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberPredicatesDefinition
    {
        public static IPredicate<MethodMember> AreConstructors()
        {
            return new SimplePredicate<MethodMember>(
                member => member.IsConstructor(),
                "are constructors"
            );
        }

        public static IPredicate<MethodMember> AreVirtual()
        {
            return new SimplePredicate<MethodMember>(member => member.IsVirtual, "are virtual");
        }

        public static IPredicate<MethodMember> AreCalledBy(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return AreCalledBy(types);
        }

        public static IPredicate<MethodMember> AreCalledBy(Type firstType, params Type[] moreTypes)
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return AreCalledBy(types);
        }

        public static IPredicate<MethodMember> AreCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type => type.GetCallingTypes().Intersect(types).Any());
            }

            var description = "are called by " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type => type.GetCallingTypes().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are called by one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var archUnitTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                return ruleTypes.Where(type =>
                    type.GetCallingTypes().Intersect(archUnitTypeList).Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are called by one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "are called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return HaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type =>
                    type.GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Intersect(types)
                        .Any()
                );
            }

            var description = "have dependencies in method body to " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(
            IEnumerable<IType> types
        )
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type =>
                    type.GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Intersect(typeList)
                        .Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var archUnitTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                return ruleTypes.Where(type =>
                    type.GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Intersect(archUnitTypeList)
                        .Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "have dependencies in method body to one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return HaveReturnType(types);
        }

        public static IPredicate<MethodMember> HaveReturnType(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.FullName).ToList();
            var description =
                "have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            return new SimplePredicate<MethodMember>(
                member => typeList.Any(type => member.ReturnType.FullNameEquals(type.FullName)),
                description
            );
        }

        public static IPredicate<MethodMember> HaveReturnType(IObjectProvider<IType> types)
        {
            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = types.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                return methodMemberList.Where(methodMember =>
                    typeList.Any(type => methodMember.ReturnType.FullNameEquals(type.FullName))
                );
            }

            var description = "have return type " + types.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> HaveReturnType(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return HaveReturnType(types);
        }

        public static IPredicate<MethodMember> HaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.ToString()).ToList();
            var description =
                "have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            return new SimplePredicate<MethodMember>(
                member => typeList.Any(type => member.ReturnTypeInstance.MatchesType(type)),
                description
            );
        }

        //Negations

        public static IPredicate<MethodMember> AreNoConstructors()
        {
            return new SimplePredicate<MethodMember>(
                member => !member.IsConstructor(),
                "are no constructors"
            );
        }

        public static IPredicate<MethodMember> AreNotVirtual()
        {
            return new SimplePredicate<MethodMember>(
                member => !member.IsVirtual,
                "are not virtual"
            );
        }

        public static IPredicate<MethodMember> AreNotCalledBy(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return AreNotCalledBy(types);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return AreNotCalledBy(types);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type => !type.GetCallingTypes().Intersect(types).Any());
            }

            var description = "are not called by " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IEnumerable<IType> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type => !type.GetCallingTypes().Intersect(typeList).Any());
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not called by one of no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "are not called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> AreNotCalledBy(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var archUnitTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                return ruleTypes.Where(type =>
                    !type.GetCallingTypes().Intersect(archUnitTypeList).Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description = "are not called by one of no types (always false)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "are not called by \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return DoNotHaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return DoNotHaveDependencyInMethodBodyTo(types);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var types = objectProvider.GetObjects(architecture);
                return ruleTypes.Where(type =>
                    !type.GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Intersect(types)
                        .Any()
                );
            }

            var description =
                "do not have dependencies in method body to " + objectProvider.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(
            IEnumerable<IType> types
        )
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(IEnumerable<MethodMember> ruleTypes)
            {
                return ruleTypes.Where(type =>
                    !type.GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Intersect(typeList)
                        .Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description =
                    "do not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => !type.Equals(firstType))
                    .Distinct()
                    .Aggregate(
                        "do not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new EnumerablePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(
            IEnumerable<Type> types
        )
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var archUnitTypeList = typeList.Select(architecture.GetITypeOfType).ToList();
                return ruleTypes.Where(type =>
                    !type.GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Intersect(archUnitTypeList)
                        .Any()
                );
            }

            string description;
            if (typeList.IsNullOrEmpty())
            {
                description =
                    "do not have dependencies in method body to one of no types (always true)";
            }
            else
            {
                var firstType = typeList.First();
                description = typeList
                    .Where(type => type != firstType)
                    .Distinct()
                    .Aggregate(
                        "do not have dependencies in method body to \"" + firstType.FullName + "\"",
                        (current, type) => current + " or \"" + type.FullName + "\""
                    );
            }

            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(
            IType firstType,
            params IType[] moreTypes
        )
        {
            var types = new List<IType> { firstType };
            types.AddRange(moreTypes);
            return DoNotHaveReturnType(types);
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(IEnumerable<IType> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.FullName).ToList();
            var description =
                "do not have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            return new SimplePredicate<MethodMember>(
                member => typeList.Any(type => !member.ReturnType.FullNameEquals(type.FullName)),
                description
            );
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(IObjectProvider<IType> types)
        {
            IEnumerable<MethodMember> Condition(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var typeList = types.GetObjects(architecture).ToList();
                var methodMemberList = methodMembers.ToList();
                return methodMemberList.Where(methodMember =>
                    typeList.All(type => !methodMember.ReturnType.FullNameEquals(type.FullName))
                );
            }

            var description = "do not have return type " + types.Description;
            return new ArchitecturePredicate<MethodMember>(Condition, description);
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(
            Type firstType,
            params Type[] moreTypes
        )
        {
            var types = new List<Type> { firstType };
            types.AddRange(moreTypes);
            return DoNotHaveReturnType(types);
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();
            var typeStringList = typeList.Select(type => type.ToString()).ToList();
            var description =
                "do not have return type \"" + string.Join("\" or \"", typeStringList) + "\"";

            return new SimplePredicate<MethodMember>(
                member => typeList.All(type => !member.ReturnTypeInstance.MatchesType(type)),
                description
            );
        }
    }
}
