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
        private readonly Dictionary<string, TypeInstance<IType>> _allTypes =
            new Dictionary<string, TypeInstance<IType>>();

        public TypeInstance<IType> GetOrCreateTypeFromTypeReference([NotNull] TypeReference typeReference,
            [NotNull] Func<string, TypeInstance<IType>> createFunc)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(TypeFactory.GetTypeFullNameForTypeReference(typeReference),
                _allTypes, createFunc);
        }

        public IEnumerable<IType> GetAllTypes()
        {
            return _allTypes.Values.Select(instance => instance.Type).Distinct();
        }
    }
}