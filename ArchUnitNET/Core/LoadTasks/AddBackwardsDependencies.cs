/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;

namespace ArchUnitNET.Core.LoadTasks
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