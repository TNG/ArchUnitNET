/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using TestAssembly;
// ReSharper disable InconsistentNaming

namespace ArchUnitNETTests.Fluent
{
    public static class StaticTestArchitectures
    {
        public static readonly Architecture AttributeDependencyTestArchitecture = 
            new ArchLoader().LoadAssemblies(typeof(ClassWithExampleAttribute).Assembly, typeof(Class1).Assembly)
                .Build();
        
        public static readonly Architecture ArchUnitNETTestArchitecture =
            new ArchLoader().LoadAssemblies(typeof(BaseClass).Assembly).Build();
    }
}