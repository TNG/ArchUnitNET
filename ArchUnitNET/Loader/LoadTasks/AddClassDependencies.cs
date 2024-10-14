//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddClassDependencies : ILoadTask
    {
        private readonly List<ITypeDependency> _dependencies;
        private readonly IType _type;
        private readonly System.Type _systemType;
        private readonly TypeFactory _typeFactory;

        public AddClassDependencies(
            IType type,
            System.Type systemType,
            TypeFactory typeFactory,
            List<ITypeDependency> dependencies
        )
        {
            _type = type;
            _systemType = systemType;
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
                _dependencies.AddRange(member.Dependencies);
            });
        }

        private void AddInterfaceDependencies()
        {
            GetInterfacesImplementedByClass(_systemType)
                .ForEach(target =>
                {
                    var targetType = _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        target
                    );
                    _dependencies.Add(new ImplementsInterfaceDependency(_type, targetType));
                });
        }

        private static IEnumerable<System.Type> GetInterfacesImplementedByClass(
            System.Type systemType
        )
        {
            var baseType = systemType.BaseType;
            var baseInterfaces =
                baseType != null
                    ? GetInterfacesImplementedByClass(baseType)
                    : new List<System.Type>();

            return systemType.GetInterfaces().Concat(baseInterfaces);
        }
    }
}
