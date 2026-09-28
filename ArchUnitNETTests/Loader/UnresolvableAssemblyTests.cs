using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Xunit;
using static ArchUnitNET.Loader.MonoCecilTypeExtensions;

namespace ArchUnitNETTests.Loader
{
    /// <summary>
    /// Tests that the loader handles assemblies whose dependencies cannot be resolved
    /// (e.g. when a referenced assembly is not present in the output directory).
    /// </summary>
    public class UnresolvableAssemblyTests
    {
        private static string RepoRoot =>
            AppDomain.CurrentDomain.BaseDirectory[
                ..AppDomain.CurrentDomain.BaseDirectory.IndexOf(
                    @"ArchUnitNETTests",
                    StringComparison.InvariantCulture
                )
            ];

        private static string FilteredDirectoryLoaderTestAssemblyPath =>
            Directory
                .EnumerateFiles(
                    Path.Combine(RepoRoot, "TestAssemblies", "FilteredDirectoryLoaderTestAssembly"),
                    "FilteredDirectoryLoaderTestAssembly.dll",
                    SearchOption.AllDirectories
                )
                .First();

        /// <summary>
        /// DerivedAttribute extends BaseAttribute from FilteredDirectoryUnavailableTypesAssembly,
        /// which is not copied to the output directory (Private=False). IsAttribute must not
        /// throw when the base type's assembly is unavailable.
        /// </summary>
        [Fact]
        public void IsAttributeHandlesUnresolvableBaseTypeAssembly()
        {
            var assemblyPath = FilteredDirectoryLoaderTestAssemblyPath;
            Assert.True(File.Exists(assemblyPath), $"Test assembly not found at {assemblyPath}");

            // DefaultAssemblyResolver has no search directories, so resolving
            // FilteredDirectoryUnavailableTypesAssembly will fail.
            var resolver = new DefaultAssemblyResolver();
            var module = ModuleDefinition.ReadModule(
                assemblyPath,
                new ReaderParameters { AssemblyResolver = resolver }
            );

            try
            {
                var derivedAttribute = module.Types.FirstOrDefault(t =>
                    t.FullName == "FilteredDirectoryLoaderTestAssembly.DerivedAttribute"
                );
                Assert.NotNull(derivedAttribute);
                Assert.Equal(
                    "FilteredDirectoryUnavailableTypesAssembly.BaseAttribute",
                    derivedAttribute.BaseType.FullName
                );

                Assert.False(derivedAttribute.IsAttribute());
            }
            finally
            {
                module.Dispose();
                resolver.Dispose();
            }
        }
    }
}
