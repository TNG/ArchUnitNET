//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Core.LoadTasks
{
    internal class AddFieldAndPropertyDependencies : ILoadTask
    {
        private readonly IType _type;

        public AddFieldAndPropertyDependencies(IType type)
        {
            _type = type;
        }

        public void Execute()
        {
            _type.GetFieldMembers().ForEach(field =>
            {
                var dependency = new FieldTypeDependency(field);
                AddDependencyIfMissing(field, dependency);
            });

            _type.GetPropertyMembers().ForEach(property =>
            {
                var dependency = new PropertyTypeDependency(property);
                AddDependencyIfMissing(property, dependency);
            });
        }

        private static void AddDependencyIfMissing(IMember member, IMemberTypeDependency dependency)
        {
            if (!member.MemberDependencies.Contains(dependency))
            {
                member.MemberDependencies.Add(dependency);
            }
        }
    }
}