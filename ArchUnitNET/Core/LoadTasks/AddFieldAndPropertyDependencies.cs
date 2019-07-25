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

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;

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