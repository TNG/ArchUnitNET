//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Core
{
    internal class NamespaceRegistry
    {
        private readonly Dictionary<string, Namespace> _namespaces = new Dictionary<string, Namespace>();

        public IEnumerable<Namespace> Namespaces => _namespaces.Values;

        public Namespace GetOrCreateNamespace(string typeNamespaceName)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(typeNamespaceName, _namespaces,
                s => new Namespace(typeNamespaceName, new List<IType>()));
        }
    }
}