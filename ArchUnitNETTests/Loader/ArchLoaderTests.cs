extern alias LoaderTestAssemblyAlias;
extern alias OtherLoaderTestAssemblyAlias;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
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
        public void WithoutRuleEvaluationCacheDisablesCaching()
        {
            // Build an architecture with caching disabled and verify that
            // GetOrCreateObjects always invokes the providing function.
            var architecture = new ArchLoader()
                .WithoutRuleEvaluationCache()
                .WithoutArchitectureCache()
                .LoadAssembly(typeof(BaseClass).Assembly)
                .Build();

            var callCount = 0;
            var provider = new TestObjectProvider("same-description");

            architecture.GetOrCreateObjects(
                provider,
                _ =>
                {
                    callCount++;
                    return Enumerable.Empty<IType>();
                }
            );
            architecture.GetOrCreateObjects(
                provider,
                _ =>
                {
                    callCount++;
                    return Enumerable.Empty<IType>();
                }
            );

            // Caching disabled — function should be invoked every time
            Assert.Equal(2, callCount);
        }

        [Fact]
        public void DefaultCacheCachesRuleEvaluationResults()
        {
            // Build an architecture with the default cache and verify that
            // GetOrCreateObjects caches the result for the same provider.
            var architecture = new ArchLoader()
                .WithoutArchitectureCache()
                .LoadAssembly(typeof(BaseClass).Assembly)
                .Build();

            var callCount = 0;
            var provider = new TestObjectProvider("default-cache-test");

            architecture.GetOrCreateObjects(
                provider,
                _ =>
                {
                    callCount++;
                    return Enumerable.Empty<IType>();
                }
            );
            architecture.GetOrCreateObjects(
                provider,
                _ =>
                {
                    callCount++;
                    return Enumerable.Empty<IType>();
                }
            );

            // Default caching — function should be invoked only once
            Assert.Equal(1, callCount);
        }

        [Fact]
        public void WithoutArchitectureCacheCreatesFreshArchitecture()
        {
            // Two architectures built with WithoutArchitectureCache for the same assembly
            // should be distinct object instances.
            var arch1 = new ArchLoader()
                .WithoutArchitectureCache()
                .LoadAssembly(typeof(BaseClass).Assembly)
                .Build();

            var arch2 = new ArchLoader()
                .WithoutArchitectureCache()
                .LoadAssembly(typeof(BaseClass).Assembly)
                .Build();

            Assert.NotSame(arch1, arch2);
        }

        [Fact]
        public void WithoutRuleEvaluationCacheAndWithoutArchitectureCacheCombined()
        {
            // Build two architectures with caching disabled + WithoutArchitectureCache
            // from the same assembly. They should be distinct instances, each with no caching.
            var arch1 = new ArchLoader()
                .WithoutRuleEvaluationCache()
                .WithoutArchitectureCache()
                .LoadAssembly(typeof(BaseClass).Assembly)
                .Build();

            var arch2 = new ArchLoader()
                .WithoutRuleEvaluationCache()
                .WithoutArchitectureCache()
                .LoadAssembly(typeof(BaseClass).Assembly)
                .Build();

            Assert.NotSame(arch1, arch2);

            // Verify caching is disabled
            var callCount = 0;
            var provider = new TestObjectProvider("combined-test");

            arch1.GetOrCreateObjects(
                provider,
                _ =>
                {
                    callCount++;
                    return Enumerable.Empty<IType>();
                }
            );
            arch1.GetOrCreateObjects(
                provider,
                _ =>
                {
                    callCount++;
                    return Enumerable.Empty<IType>();
                }
            );

            Assert.Equal(2, callCount);
        }

        /// <summary>
        /// Minimal IObjectProvider implementation for ArchLoader integration tests.
        /// </summary>
        private class TestObjectProvider : IObjectProvider<IType>
        {
            public TestObjectProvider(string description)
            {
                Description = description;
            }

            public string Description { get; }

            public IEnumerable<IType> GetObjects(Architecture architecture)
            {
                return Enumerable.Empty<IType>();
            }

            public string FormatDescription(
                string emptyDescription,
                string singleDescription,
                string multipleDescription
            )
            {
                return Description;
            }

            public override int GetHashCode()
            {
                return Description != null ? Description.GetHashCode() : 0;
            }
        }

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
            Assert.Equal(2, types.Count);
            Assert.Single(types, type => type.Assembly.Name.StartsWith("LoaderTestAssembly"));
            Assert.Single(types, type => type.Assembly.Name.StartsWith("OtherLoaderTestAssembly"));
        }

        [Fact]
        public void LoadAssembliesIncludingRecursiveDependencies()
        {
            var architecture = new ArchLoader()
                .LoadAssembliesIncludingDependencies(new[] { typeof(BaseClass).Assembly }, true)
                .Build();

            Assert.True(architecture.Assemblies.Count() > 100);

            // Check for well-known assemblies, that ReferencedAssemblyNames is not empty
            Assert.All(
                architecture.Assemblies.Where(x => x.Name.StartsWith("ArchUnit")),
                x => Assert.NotEmpty(x.ReferencedAssemblyNames)
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

            // Check that ReferencedAssemblyNames is not empty
            Assert.All(architecture.Assemblies, x => Assert.NotEmpty(x.ReferencedAssemblyNames));
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

        [Fact]
        public void UnavailableTypeTest()
        {
            // When loading an assembly from a file, there are situations where the assemblies dependencies are not
            // available in the current AppDomain. This test checks that the loader does not throw an exception
            // and that the unavailable types contain the correct assembly they come from.
            var currentAssemblyPath = AppDomain.CurrentDomain.BaseDirectory[
                ..AppDomain.CurrentDomain.BaseDirectory.IndexOf(
                    @"ArchUnitNETTests",
                    StringComparison.InvariantCulture
                )
            ];
            var assemblySearchPath = Path.Combine(
                [currentAssemblyPath, "TestAssemblies", "FilteredDirectoryLoaderTestAssembly"]
            );
            var architecture = new ArchLoader()
                .LoadFilteredDirectory(
                    assemblySearchPath,
                    "FilteredDirectoryLoaderTestAssembly.dll",
                    System.IO.SearchOption.AllDirectories
                )
                .Build();
            Assert.Equal(3, architecture.Types.Count());
            var loggerType = architecture.ReferencedTypes.WhereFullNameIs("Serilog.ILogger");
            Assert.NotNull(loggerType);
            Assert.True(loggerType is UnavailableType);
            Assert.Equal("Serilog", loggerType.Assembly.Name);
        }
    }
}
