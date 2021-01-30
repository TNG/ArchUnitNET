//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;

namespace ArchUnitNET.Loader.LoadTasks
{
    public class AddGenericArgumentDependencies : ILoadTask
    {
        [NotNull] private readonly IType _type;

        public AddGenericArgumentDependencies([NotNull] IType type)
        {
            _type = type;
        }

        public void Execute()
        {
            AddTypeGenericArgumentDependencies();
            AddMemberGenericArgumentDependencies();

            var typeDependencies = new List<GenericArgumentTypeDependency>();
            foreach (var dependency in _type.Dependencies)
            {
                FindGenericArgumentsInTypeDependenciesRecursive(dependency.TargetGenericArguments, typeDependencies);
            }

            _type.Dependencies.AddRange(typeDependencies);


            foreach (var member in _type.Members)
            {
                var memberDependencies = new List<GenericArgumentMemberDependency>();
                foreach (var dependency in member.Dependencies)
                {
                    FindGenericArgumentsInMemberDependenciesRecursive(member, dependency.TargetGenericArguments,
                        memberDependencies);
                }

                member.MemberDependencies.AddRange(memberDependencies);
            }
        }

        private void AddTypeGenericArgumentDependencies()
        {
            foreach (var parameter in _type.GenericParameters)
            {
                _type.Dependencies.AddRange(parameter.Dependencies);
            }
        }

        private void FindGenericArgumentsInTypeDependenciesRecursive(
            IEnumerable<GenericArgument> targetGenericArguments,
            ICollection<GenericArgumentTypeDependency> createdDependencies)
        {
            foreach (var genericArgument in targetGenericArguments.Where(argument => !argument.Type.IsGenericParameter))
            {
                createdDependencies.Add(new GenericArgumentTypeDependency(_type, genericArgument));
                FindGenericArgumentsInTypeDependenciesRecursive(genericArgument.GenericArguments, createdDependencies);
            }
        }

        private void AddMemberGenericArgumentDependencies()
        {
            foreach (var member in _type.Members)
            {
                foreach (var parameter in member.GenericParameters)
                {
                    member.MemberDependencies.AddRange(parameter.Dependencies.Cast<IMemberTypeDependency>());
                }
            }
        }

        private static void FindGenericArgumentsInMemberDependenciesRecursive(IMember member,
            IEnumerable<GenericArgument> targetGenericArguments,
            ICollection<GenericArgumentMemberDependency> createdDependencies)
        {
            foreach (var genericArgument in targetGenericArguments.Where(argument => !argument.Type.IsGenericParameter))
            {
                createdDependencies.Add(new GenericArgumentMemberDependency(member, genericArgument));
                FindGenericArgumentsInMemberDependenciesRecursive(member, genericArgument.GenericArguments,
                    createdDependencies);
            }
        }
    }
}