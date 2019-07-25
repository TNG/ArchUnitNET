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
using ArchUnitNET.Domain.Dependencies.Types;
using Mono.Cecil;

namespace ArchUnitNET.Core.LoadTasks
{
    internal class AddBaseClassDependency : ILoadTask
    {
        private readonly Class _cls;
        private readonly Type _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly TypeFactory _typeFactory;

        public AddBaseClassDependency(Class cls, Type type, TypeDefinition typeDefinition, TypeFactory typeFactory)
        {
            _cls = cls;
            _type = type;
            _typeDefinition = typeDefinition;
            _typeFactory = typeFactory;
        }

        public void Execute()
        {
            var typeDefinitionBaseType = _typeDefinition?.BaseType;

            if (typeDefinitionBaseType == null)
            {
                return;
            }

            var baseType = _typeFactory.GetOrCreateStubTypeFromTypeReference(typeDefinitionBaseType);
            if (!(baseType is Class baseClass))
            {
                return;
            }

            var dependency = new InheritsBaseClassDependency(_cls, baseClass);
            _type.Dependencies.Add(dependency);
        }
    }
}