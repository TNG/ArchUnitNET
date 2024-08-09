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
        private readonly TypeDefinition _typeDefinition;
        private readonly TypeFactory _typeFactory;

        public AddClassDependencies(
            IType type,
            TypeDefinition typeDefinition,
            TypeFactory typeFactory,
            List<ITypeDependency> dependencies
        )
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
                _dependencies.AddRange(
                    member.Dependencies.Where(dependency =>
                        // E.g. static members with initializer lead to a dependency
                        // to the declaring type which we want to ignore
                        // https://github.com/TNG/ArchUnitNET/issues/229
                        dependency.Target.FullName != _type.FullName
                    )
                );
            });
        }

        private void AddInterfaceDependencies()
        {
            GetInterfacesImplementedByClass(_typeDefinition)
                .ForEach(target =>
                {
                    var targetType = _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        target
                    );
                    _dependencies.Add(new ImplementsInterfaceDependency(_type, targetType));
                });
        }

        private static IEnumerable<TypeReference> GetInterfacesImplementedByClass(
            TypeDefinition typeDefinition
        )
        {
            var baseType = typeDefinition.BaseType?.Resolve();
            var baseInterfaces =
                baseType != null
                    ? GetInterfacesImplementedByClass(baseType)
                    : new List<TypeReference>();

            return typeDefinition
                .Interfaces.Select(implementation => implementation.InterfaceType)
                .Concat(baseInterfaces);
        }
    }
}
