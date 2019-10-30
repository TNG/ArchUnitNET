//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using TestAssembly;
using Xunit.Sdk;

// ReSharper disable InconsistentNaming

namespace ArchUnitNETTests.Fluent.Extensions
{
    public static class StaticTestArchitectures
    {
        public static readonly Architecture AttributeDependencyTestArchitecture =
            new ArchLoader().LoadAssemblies(typeof(ClassWithExampleAttribute).Assembly, typeof(Class1).Assembly)
                .Build();

        public static readonly Architecture ArchUnitNETTestArchitecture =
            new ArchLoader().LoadAssemblies(typeof(BaseClass).Assembly).Build();

        public static readonly Architecture ArchUnitNETTestAssemblyArchitecture =
            new ArchLoader().LoadAssemblies(typeof(Class1).Assembly).Build();

        public static readonly Architecture FullArchUnitNETArchitecture =
            new ArchLoader().LoadAssemblies(typeof(Architecture).Assembly, typeof(BaseClass).Assembly,
                typeof(Class1).Assembly, typeof(FailedArchRuleException).Assembly).Build();

        public static readonly Architecture ArchUnitNETTestArchitectureWithDependencies =
            new ArchLoader().LoadAssembliesIncludingDependencies(typeof(BaseClass).Assembly).Build();

        public static readonly Architecture ArchUnitNETTestAssemblyArchitectureWithDependencies =
            new ArchLoader().LoadAssembliesIncludingDependencies(typeof(Class1).Assembly).Build();

        public static readonly Architecture FullArchUnitNETArchitectureWithDependencies =
            new ArchLoader().LoadAssembliesIncludingDependencies(typeof(Architecture).Assembly,
                typeof(BaseClass).Assembly,
                typeof(Class1).Assembly, typeof(FailedArchRuleException).Assembly).Build();
    }
}