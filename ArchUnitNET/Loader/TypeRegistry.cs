//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    internal class TypeRegistry
    {
        private readonly Dictionary<string, ITypeInstance<IType>> _allTypes =
            new Dictionary<string, ITypeInstance<IType>>();

        public ITypeInstance<IType> GetOrCreateTypeFromTypeReference(
            [NotNull] TypeReference typeReference,
            [NotNull] Func<string, ITypeInstance<IType>> createFunc
        )
        {
            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                typeReference.Module.Assembly.FullName,
                typeReference.BuildFullName()
            );
            return RegistryUtils.GetFromDictOrCreateAndAdd(
                assemblyQualifiedName,
                _allTypes,
                createFunc
            );
        }

        public IEnumerable<IType> GetAllTypes()
        {
            // if (
            //     _allTypes.Values.Select(instance => instance.Type).Count()
            //     != _allTypes.Values.Select(instance => instance.Type).Distinct().Count()
            // )
            // {
            //     var sorted = _allTypes.Values.GroupBy(instance => instance.Type.FullName).ToList();
            //     foreach (var group in sorted)
            //     {
            //         foreach (var left in group)
            //         {
            //             foreach (var right in group)
            //             {
            //                 if (!ReferenceEquals(left.Type, right.Type))
            //                 {
            //                     throw new InvalidOperationException(
            //                         $"Type {left.Type.AssemblyQualifiedName} is already registered with a different instance"
            //                     );
            //                 }
            //             }
            //         }
            //     }
            // }
            return _allTypes.Values.Select(instance => instance.Type).Distinct();
        }
    }
}
