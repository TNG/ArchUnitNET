//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    internal class TypeRegistry
    {
        private readonly Dictionary<string, IType> _allTypes = new Dictionary<string, IType>();

        public IType GetOrCreateTypeFromTypeReference([NotNull] TypeReference typeReference,
            [NotNull] Func<string, IType> createFunc)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(TypeFactory.GetTypeFullNameForTypeReference(typeReference),
                _allTypes, createFunc);
        }

        public IEnumerable<IType> GetAllTypes()
        {
            return _allTypes.Values;
        }
    }
}