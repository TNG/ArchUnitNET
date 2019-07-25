/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Core
{
    internal class TypeRegistry
    {
        private readonly Dictionary<string, IType> _allTypes = new Dictionary<string, IType>();

        public IType GetOrCreateTypeFromTypeReference([NotNull] TypeReference typeReference, [NotNull] Func<string, IType> createFunc)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(typeReference.FullName, _allTypes,
                createFunc);
        }
    }
}