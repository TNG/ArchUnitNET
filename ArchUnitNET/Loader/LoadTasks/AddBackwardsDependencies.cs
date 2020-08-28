//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddBackwardsDependencies : ILoadTask
    {
        private readonly IType _type;

        public AddBackwardsDependencies(IType type)
        {
            _type = type;
        }

        public void Execute()
        {
            _type.Dependencies.ForEach(dependency => dependency.Target.BackwardsDependencies.Add(dependency));

            var memberMemberDependencies = _type.Members.SelectMany(member => member.MemberDependencies)
                .OfType<IMemberMemberDependency>();
            memberMemberDependencies.ForEach(memberDependency =>
                memberDependency.TargetMember.MemberBackwardsDependencies.Add(memberDependency));
        }
    }
}