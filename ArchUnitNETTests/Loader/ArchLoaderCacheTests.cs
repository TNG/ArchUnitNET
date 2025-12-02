using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using System.Diagnostics;
using Xunit;

namespace ArchUnitNETTests.Loader
{
    public class ArchLoaderCacheTests
    {
        [Fact]
        public void LoadingSameAssemblyTwiceReturnsCachedArchitecture()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;

            // Act
            var architecture1 = new ArchLoader().LoadAssemblies(assembly).Build();
            var architecture2 = new ArchLoader().LoadAssemblies(assembly).Build();

            // Assert - Should return the same cached instance
            Assert.Same(architecture1, architecture2);
        }

        [Fact]
        public void LoadingSameAssemblyTwiceIsFasterOnSecondLoad()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;
            ArchitectureCache.Instance.Clear(); // Clear cache to ensure fair timing
            ArchitectureCacheManager.Instance.Clear();

            // Act
            var sw1 = Stopwatch.StartNew();
            var architecture1 = new ArchLoader().LoadAssemblies(assembly).Build();
            sw1.Stop();

            var sw2 = Stopwatch.StartNew();
            var architecture2 = new ArchLoader().LoadAssemblies(assembly).Build();
            sw2.Stop();

            // Assert - Second load should be significantly faster (at least 50% faster)
            Assert.True(
                sw2.ElapsedMilliseconds < sw1.ElapsedMilliseconds * 0.5,
                $"Expected second load ({sw2.ElapsedMilliseconds}ms) to be at least 50% faster than first load ({sw1.ElapsedMilliseconds}ms)"
            );
            Assert.Same(architecture1, architecture2);
        }

        [Fact]
        public void LoadingMultipleAssembliesReturnsCachedArchitecture()
        {
            // Arrange - Use two truly different assemblies
            var assembly1 = typeof(Architecture).Assembly; // ArchUnitNET
            var assembly2 = typeof(ArchLoaderCacheTests).Assembly; // ArchUnitNETTests

            // Act
            var architecture1 = new ArchLoader()
                .LoadAssemblies(assembly1, assembly2)
                .Build();
            var architecture2 = new ArchLoader()
                .LoadAssemblies(assembly1, assembly2)
                .Build();

            // Assert
            Assert.Same(architecture1, architecture2);
        }

        [Fact]
        public void LoadingAssembliesInDifferentOrderReturnsSameCachedArchitecture()
        {
            // Arrange - Use two truly different assemblies
            var assembly1 = typeof(Architecture).Assembly; // ArchUnitNET
            var assembly2 = typeof(ArchLoaderCacheTests).Assembly; // ArchUnitNETTests

            // Act - Load in different order
            var architecture1 = new ArchLoader()
                .LoadAssemblies(assembly1, assembly2)
                .Build();
            var architecture2 = new ArchLoader()
                .LoadAssemblies(assembly2, assembly1)
                .Build();

            // Assert - Should return same cached instance regardless of order
            Assert.Same(architecture1, architecture2);
        }

        [Fact]
        public void LoadingDifferentAssembliesReturnsDifferentArchitectures()
        {
            // Arrange - Use two truly different assemblies
            var assembly1 = typeof(Architecture).Assembly; // ArchUnitNET
            var assembly2 = typeof(ArchLoaderCacheTests).Assembly; // ArchUnitNETTests

            // Act
            var architecture1 = new ArchLoader().LoadAssemblies(assembly1).Build();
            var architecture2 = new ArchLoader().LoadAssemblies(assembly2).Build();

            // Assert - Different assemblies should return different architectures
            Assert.NotSame(architecture1, architecture2);
        }

        [Fact]
        public void LoadingWithNamespaceFilterReturnsCachedArchitecture()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;
            var namespaceName = "ArchUnitNET.Domain";

            // Act
            var architecture1 = new ArchLoader()
                .LoadNamespacesWithinAssembly(assembly, namespaceName)
                .Build();
            var architecture2 = new ArchLoader()
                .LoadNamespacesWithinAssembly(assembly, namespaceName)
                .Build();

            // Assert
            Assert.Same(architecture1, architecture2);
        }

        [Fact]
        public void LoadingDifferentNamespacesReturnsDifferentArchitectures()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;

            // Act
            var architecture1 = new ArchLoader()
                .LoadNamespacesWithinAssembly(assembly, "ArchUnitNET.Domain")
                .Build();
            var architecture2 = new ArchLoader()
                .LoadNamespacesWithinAssembly(assembly, "ArchUnitNET.Loader")
                .Build();

            // Assert - Different namespace filters should return different architectures
            Assert.NotSame(architecture1, architecture2);
        }

        [Fact]
        public void ClearingCacheForcesReloadOnNextBuild()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;
            var architecture1 = new ArchLoader().LoadAssemblies(assembly).Build();

            // Act
            ArchitectureCache.Instance.Clear();
            ArchitectureCacheManager.Instance.Clear();
            var architecture2 = new ArchLoader().LoadAssemblies(assembly).Build();

            // Assert - After clearing cache, should get a new instance
            Assert.NotSame(architecture1, architecture2);

            // But loading again should return cached version
            var architecture3 = new ArchLoader().LoadAssemblies(assembly).Build();
            Assert.Same(architecture2, architecture3);
        }

        [Fact]
        public void LoadingAssembliesIncludingDependenciesUsesCaching()
        {
            // Arrange
            var assembly = typeof(Architecture).Assembly;

            // Act
            var architecture1 = new ArchLoader()
                .LoadAssembliesIncludingDependencies(assembly)
                .Build();
            var architecture2 = new ArchLoader()
                .LoadAssembliesIncludingDependencies(assembly)
                .Build();

            // Assert
            Assert.Same(architecture1, architecture2);
        }
    }
}
