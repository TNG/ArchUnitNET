//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
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

            var baseType = _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeDefinitionBaseType);
            if (!(baseType.Type is Class baseClass))
            {
                return;
            }

            var dependency =
                new InheritsBaseClassDependency(_cls, new TypeInstance<Class>(baseClass, baseType.GenericArguments));
            _type.Dependencies.Add(dependency);
        }
    }
}