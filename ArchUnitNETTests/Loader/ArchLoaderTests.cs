//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

extern alias LoaderTestAssemblyAlias;
extern alias OtherLoaderTestAssemblyAlias;

using System.Linq;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using ArchUnitNETTests.Domain.Dependencies.Members;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNETTests.StaticTestArchitectures;

using DuplicateClass = LoaderTestAssemblyAlias::DuplicateClassAcrossAssemblies.DuplicateClass;
using OtherDuplicateClass = OtherLoaderTestAssemblyAlias::DuplicateClassAcrossAssemblies.DuplicateClass;

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
        public void SameFullNameInMultipleAssemblies()
        {
            // Loaded as non-stub types
            var types = LoaderTestArchitecture
                .Types.Where(type => type.Namespace.FullName == "DuplicateClassAcrossAssemblies")
                .ToList();
            Assert.Equal(2, types.Count);
            Assert.Single(types, type => type.Assembly.Name.StartsWith("LoaderTestAssembly"));
            Assert.Single(types, type => type.Assembly.Name.StartsWith("OtherLoaderTestAssembly"));

            // Loaded as stub types

            // We create a direct dependency of ArchUnitNETTests on the duplicate type such that they are loaded as stub types

#pragma warning disable CS0219 // Variable is assigned but its value is never used
            DuplicateClass duplicateClass = null;
            OtherDuplicateClass otherDuplicateClass = null;
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            types = ArchUnitNETTestArchitecture
                .ReferencedTypes.Where(type =>
                    type.Namespace.FullName == "DuplicateClassAcrossAssemblies"
                )
                .ToList();
            var types2 = ArchUnitNETTestArchitecture
                .ReferencedTypes.Where(type => type.FullName.Contains("Duplicate"))
                .ToList();
            Assert.Equal(2, types.Count);
            Assert.Single(types, type => type.Assembly.Name.StartsWith("LoaderTestAssembly"));
            Assert.Single(types, type => type.Assembly.Name.StartsWith("OtherLoaderTestAssembly"));
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

            Assert.Single(architecture.Assemblies);
        }

        [Fact]
        public void TypesAreAssignedToCorrectAssemblies()
        {
            // https://github.com/TNG/ArchUnitNET/issues/302
            var architecture = FullArchUnitNETArchitecture;
            Types()
                .That()
                .ResideInAssembly(architecture.GetType().Assembly)
                .Should()
                .NotDependOnAnyTypesThat()
                .ResideInAssembly(GetType().Assembly)
                .Check(architecture);
        }
    }
}
