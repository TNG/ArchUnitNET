//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    internal class MethodMemberRegistry
    {
        private readonly Dictionary<string, MethodMemberInstance> _allMethods =
            new Dictionary<string, MethodMemberInstance>();

        public MethodMemberInstance GetOrCreateMethodFromMethodReference([NotNull] MethodReference methodReference,
            [NotNull] Func<string, MethodMemberInstance> createFunc)
        {
            return RegistryUtils.GetFromDictOrCreateAndAdd(methodReference.BuildFullName(), _allMethods, createFunc);
        }

        public IEnumerable<MethodMember> GetAllMethodMembers()
        {
            return _allMethods.Values.Select(instance => instance.Member).Distinct();
        }
    }
}