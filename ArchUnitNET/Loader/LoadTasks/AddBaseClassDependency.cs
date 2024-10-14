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
        private readonly IType _cls;
        private readonly Type _type;
        private readonly System.Type _systemType;
        private readonly TypeFactory _typeFactory;

        public AddBaseClassDependency(
            IType cls,
            Type type,
            System.Type systemType,
            TypeFactory typeFactory
        )
        {
            _cls = cls;
            _type = type;
            _systemType = systemType;
            _typeFactory = typeFactory;
        }

        public void Execute()
        {
            var systemTypeBaseType = _systemType.BaseType;

            if (systemTypeBaseType == null)
            {
                return;
            }

            var baseType = _typeFactory.GetOrCreateStubTypeFromSystemType(
                systemTypeBaseType
            );
            if (!(baseType.Type is Class baseClass))
            {
                return;
            }
            var dependency = new InheritsBaseClassDependency(
                _cls,
                new TypeInstance<Class>(
                    baseClass,
                    baseType.GenericArguments,
                    baseType.ArrayDimensions
                )
            );
            _type.Dependencies.Add(dependency);
        }
    }
}
