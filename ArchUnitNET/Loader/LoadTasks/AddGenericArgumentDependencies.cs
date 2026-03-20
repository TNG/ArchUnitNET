using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Propagates generic-parameter dependencies to the owning type and member,
    /// then recursively discovers nested generic-argument dependencies in all
    /// existing type and member dependencies.
    /// </summary>
    internal static class AddGenericArgumentDependencies
    {
        internal static void Execute(IType type)
        {
            // Type-level generic argument dependencies
            foreach (var parameter in type.GenericParameters)
            {
                type.Dependencies.AddRange(parameter.Dependencies);
            }

            // Member-level generic argument dependencies
            foreach (var member in type.Members)
            {
                foreach (var parameter in member.GenericParameters)
                {
                    member.MemberDependencies.AddRange(
                        parameter.Dependencies.Cast<IMemberTypeDependency>()
                    );
                }
            }

            // Recursive generic arguments in type dependencies
            var typeDependencies = new List<GenericArgumentTypeDependency>();
            foreach (var dependency in type.Dependencies)
            {
                FindGenericArgumentsInTypeDependenciesRecursive(
                    type,
                    dependency.TargetGenericArguments,
                    typeDependencies
                );
            }

            type.Dependencies.AddRange(typeDependencies);

            // Recursive generic arguments in member dependencies
            foreach (var member in type.Members)
            {
                var memberDependencies = new List<GenericArgumentMemberDependency>();
                foreach (var dependency in member.Dependencies)
                {
                    FindGenericArgumentsInMemberDependenciesRecursive(
                        member,
                        dependency.TargetGenericArguments,
                        memberDependencies
                    );
                }

                member.MemberDependencies.AddRange(memberDependencies);
            }
        }

        private static void FindGenericArgumentsInTypeDependenciesRecursive(
            IType type,
            IEnumerable<GenericArgument> targetGenericArguments,
            ICollection<GenericArgumentTypeDependency> createdDependencies
        )
        {
            foreach (
                var genericArgument in targetGenericArguments.Where(argument =>
                    !argument.Type.IsGenericParameter
                )
            )
            {
                createdDependencies.Add(new GenericArgumentTypeDependency(type, genericArgument));
                FindGenericArgumentsInTypeDependenciesRecursive(
                    type,
                    genericArgument.GenericArguments,
                    createdDependencies
                );
            }
        }

        private static void FindGenericArgumentsInMemberDependenciesRecursive(
            IMember member,
            IEnumerable<GenericArgument> targetGenericArguments,
            ICollection<GenericArgumentMemberDependency> createdDependencies
        )
        {
            foreach (
                var genericArgument in targetGenericArguments.Where(argument =>
                    !argument.Type.IsGenericParameter
                )
            )
            {
                createdDependencies.Add(
                    new GenericArgumentMemberDependency(member, genericArgument)
                );
                FindGenericArgumentsInMemberDependenciesRecursive(
                    member,
                    genericArgument.GenericArguments,
                    createdDependencies
                );
            }
        }
    }
}
