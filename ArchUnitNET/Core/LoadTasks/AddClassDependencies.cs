/*
 * Copyright 2019 TNG Technology Consulting GmbH
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

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;
using Mono.Cecil;

namespace ArchUnitNET.Core.LoadTasks
{
    internal class AddClassDependencies : ILoadTask
    {
        private readonly List<ITypeDependency> _dependencies;
        private readonly IType _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly TypeFactory _typeFactory;

        public AddClassDependencies(IType type, TypeDefinition typeDefinition, TypeFactory typeFactory,
            List<ITypeDependency> dependencies)
        {
            _type = type;
            _typeDefinition = typeDefinition;
            _typeFactory = typeFactory;
            _dependencies = dependencies;
        }

        public void Execute()
        {
            AddInterfaceDependencies();
            AddMemberDependencies();
        }

        private void AddMemberDependencies()
        {
            _type.Members.ForEach(member =>
            {
                _dependencies.AddRange(member.MemberDependencies);
                _dependencies.AddRange(member.Dependencies);
            });
        }

        private void AddInterfaceDependencies()
        {
            GetInterfacesImplementedByClass(_typeDefinition).ForEach(target =>
            {
                var targetType = _typeFactory.GetOrCreateStubTypeFromTypeReference(target);
                _dependencies.Add(new ImplementsInterfaceDependency(_type, targetType));
            });
        }

        private static IEnumerable<TypeReference> GetInterfacesImplementedByClass(TypeDefinition typeDefinition)
        {
            var baseType = typeDefinition.BaseType?.Resolve();
            var baseInterfaces = baseType != null
                ? GetInterfacesImplementedByClass(baseType)
                : new List<TypeReference>();

            return typeDefinition.Interfaces
                .Select(implementation => implementation.InterfaceType)
                .Concat(baseInterfaces);
        }
    }
}