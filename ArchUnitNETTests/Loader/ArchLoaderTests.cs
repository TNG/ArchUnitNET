//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Domain.Dependencies.Members;
using Xunit;
using static ArchUnitNETTests.StaticTestArchitectures;

namespace ArchUnitNETTests.Loader
{
    public class ArchLoaderTests
    {
        [Fact]
        public void LoadAssemblies()
        {
            Assert.Equal(4, FullArchUnitNETArchitecture.Assemblies.Count());
            Assert.Single(ArchUnitNETTestArchitecture.Assemblies);
            Assert.Single(ArchUnitNETTestAssemblyArchitecture.Assemblies);
            Assert.NotEmpty(FullArchUnitNETArchitectureWithDependencies.Assemblies);
            Assert.NotEmpty(ArchUnitNETTestArchitectureWithDependencies.Assemblies);
            Assert.NotEmpty(ArchUnitNETTestAssemblyArchitectureWithDependencies.Assemblies);
        }

        [Fact]
        public void LoadAssembliesIncludingRecursiveDependencies()
        {
            var archUnitNetTestArchitectureWithRecursiveDependencies = new ArchLoader()
                .LoadAssembliesIncludingDependencies(new[] { typeof(BaseClass).Assembly }, true)
                .Build();

            Assert.True(
                archUnitNetTestArchitectureWithRecursiveDependencies.Assemblies.Count() > 100
            );
        }

        [Fact]
        public void LoadAssembliesRecursivelyWithCustomFilter()
        {
            FilterFunc filterFunc = assembly =>
                assembly.Name.Name.StartsWith("ArchUnit")
                    ? FilterResult.LoadAndContinue
                    : FilterResult.DontLoadAndStop;
            var loader = new ArchLoader();
            var architecture = loader
                .LoadAssembliesRecursively(new[] { typeof(BaseClass).Assembly }, filterFunc)
                .Build();

            Assert.Equal(3, architecture.Assemblies.Count());
        }

        [Fact]
        public void LoadAssembliesRecursively_NestedDependencyOnly()
        {
            FilterFunc filterFunc = assembly =>
            {
                if (assembly.Name.Name == "ArchUnitNet")
                    return FilterResult.LoadAndStop;

                return FilterResult.SkipAndContinue;
            };
            var loader = new ArchLoader();
            var architecture = loader
                .LoadAssembliesRecursively(new[] { typeof(BaseClass).Assembly }, filterFunc)
                .Build();

            Assert.Equal(1, architecture.Assemblies.Count());
        }
    }
}
