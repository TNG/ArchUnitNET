using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Domain.Extensions.EnumerableExtensions;

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

        public static IPredicate<MethodMember> AreCalledBy(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var isRequiredType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                return ruleTypes.Where(methodMember =>
                    methodMember.GetCallingTypes().Any(isRequiredType)
                );
            }

            var description = objectProvider.FormatDescription(
                "are called by one of no types (impossible)",
                "are called by",
                "are called by any"
            );
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }

        public static IPredicate<MethodMember> HaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var isRequiredType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                return ruleTypes.Where(methodMember =>
                    methodMember
                        .GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Any(isRequiredType)
                );
            }

            var description = objectProvider.FormatDescription(
                "have dependencies in method body to one of no types (impossible)",
                "have dependencies in method body to",
                "have dependencies in method body to any"
            );
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }

        public static IPredicate<MethodMember> HaveReturnType(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isRequiredType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                return methodMembers.Where(methodMember =>
                    isRequiredType(methodMember.ReturnType)
                );
            }

            var description = objectProvider.FormatDescription(
                "have return type of no types (impossible)",
                "have return type",
                "have return type"
            );
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }

        public static IPredicate<MethodMember> HaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                return methodMembers.Where(methodMember =>
                    typeList.Any(type => methodMember.ReturnTypeInstance.MatchesType(type))
                );
            }

            var typeDescription = string.Join(
                " or ",
                typeList.Select(type => $"\"{type.FullName}\"")
            );
            var description = typeList.Count == 0
                ? "have return type of no types (impossible)"
                : $"have return type {typeDescription}";
            return new ArchitecturePredicate<MethodMember>(Filter, description);
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
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                return ruleTypes.Where(methodMember =>
                    !methodMember.GetCallingTypes().Any(isForbiddenType)
                );
            }

            var description = objectProvider.FormatDescription(
                "are not called by one of no types (always true)",
                "are not called by",
                "are not called by any"
            );
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }

        public static IPredicate<MethodMember> DoNotHaveDependencyInMethodBodyTo(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> ruleTypes,
                Architecture architecture
            )
            {
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                return ruleTypes.Where(methodMember =>
                    !methodMember
                        .GetBodyTypeMemberDependencies()
                        .Select(dependency => dependency.Target)
                        .Any(isForbiddenType)
                );
            }

            var description = objectProvider.FormatDescription(
                "do not have dependencies in method body to one of no types (always true)",
                "do not have dependencies in method body to",
                "do not have dependencies in method body to any"
            );
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(
            IObjectProvider<IType> objectProvider
        )
        {
            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                var isForbiddenType = CreateLookupFn(
                    objectProvider.GetObjects(architecture).ToList()
                );
                return methodMembers.Where(methodMember =>
                    !isForbiddenType(methodMember.ReturnType)
                );
            }

            var description = objectProvider.FormatDescription(
                "do not have return type of no types (always true)",
                "do not have return type",
                "do not have return type"
            );
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }

        public static IPredicate<MethodMember> DoNotHaveReturnType(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            IEnumerable<MethodMember> Filter(
                IEnumerable<MethodMember> methodMembers,
                Architecture architecture
            )
            {
                return methodMembers.Where(methodMember =>
                    typeList.All(type => !methodMember.ReturnTypeInstance.MatchesType(type))
                );
            }

            var typeDescription = string.Join(
                " or ",
                typeList.Select(type => $"\"{type.FullName}\"")
            );
            var description = typeList.Count == 0
                ? "do not have return type of no types (always true)"
                : $"do not have return type {typeDescription}";
            return new ArchitecturePredicate<MethodMember>(Filter, description);
        }
    }
}
