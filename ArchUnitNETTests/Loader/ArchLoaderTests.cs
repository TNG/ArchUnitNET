extern alias LoaderTestAssemblyAlias;
extern alias OtherLoaderTestAssemblyAlias;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using ArchUnitNETTests.Domain.Dependencies.Members;
using System;
using System.IO;
using System.Linq;
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
            // available in the current AppDomain. This test checks that the loader does not throw an exception in this
            // case.
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
        }

        [Fact]
        public void WithoutCachingDisablesCaching()
        {
            // Arrange & Act
            var architecture1 = new ArchLoader()
                .WithoutCaching()
                .LoadAssemblies(typeof(Architecture).Assembly)
                .Build();

            var architecture2 = new ArchLoader()
                .WithoutCaching()
                .LoadAssemblies(typeof(Architecture).Assembly)
                .Build();

            // Assert - Without caching, should get different instances
            Assert.NotSame(architecture1, architecture2);
        }

        [Fact]
        public void WithUserCacheKeySetsCustomCacheKey()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;

            // Act
            var architecture1 = new ArchLoader()
                .WithUserCacheKey("key1")
                .LoadAssemblies(assembly)
                .Build();

            var architecture2 = new ArchLoader()
                .WithUserCacheKey("key1")
                .LoadAssemblies(assembly)
                .Build();

            var architecture3 = new ArchLoader()
                .WithUserCacheKey("key2")
                .LoadAssemblies(assembly)
                .Build();

            // Assert - Same cache key should return same instance
            Assert.Same(architecture1, architecture2);
            // Different cache key should return different instance
            Assert.NotSame(architecture1, architecture3);
        }

        [Fact]
        public void FluentAPIChainingWorks()
        {
            // Arrange & Act
            var architecture = new ArchLoader()
                .WithUserCacheKey("test-chain")
                .LoadAssemblies(typeof(Architecture).Assembly)
                .Build();

            // Assert
            Assert.NotNull(architecture);
            Assert.NotEmpty(architecture.Types);
        }

        [Fact]
        public void WithCacheConfigThrowsOnNullConfig()
        {
            // Arrange
            var loader = new ArchLoader();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => loader.WithCacheConfig(null));
        }

        [Fact]
        public void WithCacheConfigClonesConfigToPreventExternalMutation()
        {
            // Arrange
            var config = new ArchLoaderCacheConfig
            {
                CachingEnabled = true,
                UseFileBasedInvalidation = true,
                UserCacheKey = "original-key"
            };

            var assembly = typeof(Architecture).Assembly;

            // Act
            var architecture1 = new ArchLoader()
                .WithCacheConfig(config)
                .LoadAssemblies(assembly)
                .Build();

            // Mutate the original config
            config.UserCacheKey = "modified-key";
            config.CachingEnabled = false;

            // Build again with a new loader using the SAME config object
            var architecture2 = new ArchLoader()
                .WithCacheConfig(config)
                .LoadAssemblies(assembly)
                .Build();

            // Assert - The first architecture should still be cached with original key
            // and the second should use the modified key, so they should be different
            Assert.NotSame(architecture1, architecture2);
        }
    }
}
