//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class CollectAssemblyAttributes : ILoadTask
    {
        private readonly Assembly _assembly;
        private readonly AssemblyDefinition _assemblyDefinition;
        private readonly TypeFactory _typeFactory;

        public CollectAssemblyAttributes(Assembly assembly, AssemblyDefinition assemblyDefinition,
            TypeFactory typeFactory)
        {
            _assembly = assembly;
            _assemblyDefinition = assemblyDefinition;
            _typeFactory = typeFactory;
        }

        public void Execute()
        {
            var attributeInstances =
                _assemblyDefinition.CustomAttributes
                    .Select(attr => attr.CreateAttributeFromCustomAttribute(_typeFactory)).ToList();
            _assembly.AttributeInstances.AddRange(attributeInstances);
        }
    }
}