using System;
using System.IO;
using System.Linq;
using ArchUnitNET.Loader;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;
using static ArchUnitNET.Loader.MonoCecilMemberExtensions;
using static ArchUnitNET.Loader.MonoCecilTypeExtensions;

namespace ArchUnitNETTests.Loader
{
    /// <summary>
    /// Tests that loader code handles assemblies whose dependencies cannot be resolved
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

        private static string FilteredDirectoryLoaderTestAssemblyDir =>
            Path.Combine(RepoRoot, "TestAssemblies", "FilteredDirectoryLoaderTestAssembly");

        private static string FilteredDirectoryLoaderTestAssemblyPath =>
            Directory
                .EnumerateFiles(
                    Path.Combine(RepoRoot, "TestAssemblies", "FilteredDirectoryLoaderTestAssembly"),
                    "FilteredDirectoryLoaderTestAssembly.dll",
                    SearchOption.AllDirectories
                )
                .First();

        private static (DefaultAssemblyResolver resolver, ModuleDefinition module) LoadModule()
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
            return (resolver, module);
        }

        private static TypeDefinition GetDerivedAttribute(ModuleDefinition module)
        {
            var derivedAttribute = module.Types.FirstOrDefault(t =>
                t.FullName == "FilteredDirectoryLoaderTestAssembly.DerivedAttribute"
            );
            Assert.NotNull(derivedAttribute);
            return derivedAttribute;
        }

        /// <summary>
        /// DerivedAttribute extends BaseAttribute from FilteredDirectoryUnavailableTypesAssembly,
        /// which is not copied to the output directory (Private=False). IsAttribute must not
        /// throw when the base type's assembly is unavailable.
        ///
        /// Exercises <see cref="MonoCecilTypeExtensions.IsAttribute" /> →
        /// <see cref="MonoCecilResolveExtensions.TryResolve(Mono.Cecil.TypeReference)" />.
        /// </summary>
        [Fact]
        public void IsAttribute_HandlesUnresolvableBaseTypeAssembly()
        {
            var (resolver, module) = LoadModule();
            try
            {
                var derivedAttribute = GetDerivedAttribute(module);
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

        /// <summary>
        /// DerivedAttribute's constructor calls BaseAttribute..ctor(). The method reference
        /// points into the unavailable assembly. IsCompilerGenerated must not throw when
        /// resolving the member reference fails.
        ///
        /// Exercises <see cref="MonoCecilMemberExtensions.IsCompilerGenerated" /> →
        /// <see cref="MonoCecilResolveExtensions.TryResolve(Mono.Cecil.MemberReference)" />.
        /// </summary>
        [Fact]
        public void IsCompilerGenerated_HandlesUnresolvableMemberAssembly()
        {
            var (resolver, module) = LoadModule();
            try
            {
                var derivedCtor = GetDerivedAttribute(module).Methods.First(m => m.IsConstructor);

                // The constructor body contains a call to BaseAttribute..ctor()
                var baseCtorRef = derivedCtor
                    .Body.Instructions.Where(i => i.OpCode == OpCodes.Call)
                    .Select(i => i.Operand as MethodReference)
                    .First(m =>
                        m != null
                        && m.DeclaringType.FullName
                            == "FilteredDirectoryUnavailableTypesAssembly.BaseAttribute"
                    );

                Assert.False(baseCtorRef.IsCompilerGenerated());
            }
            finally
            {
                module.Dispose();
                resolver.Dispose();
            }
        }

        /// <summary>
        /// UnavailableFieldAssigner.AssignUnavailableField assigns the result of a method call
        /// to UnavailableFieldHolder.Value, a static field declared in the unavailable assembly.
        /// GetAssigneeFieldDefinition must not throw when resolving the field reference fails.
        ///
        /// Exercises <see cref="InstructionExtensions.GetAssigneeFieldDefinition" /> →
        /// <see cref="MonoCecilResolveExtensions.TryResolve(Mono.Cecil.FieldReference)" />.
        /// </summary>
        [Fact]
        public void GetAssigneeFieldDefinition_HandlesUnresolvableFieldAssembly()
        {
            var (resolver, module) = LoadModule();
            try
            {
                var assigner = module.Types.FirstOrDefault(t =>
                    t.FullName == "FilteredDirectoryLoaderTestAssembly.UnavailableFieldAssigner"
                );
                Assert.NotNull(assigner);
                var method = assigner.Methods.First(m => m.Name == "AssignUnavailableField");

                var callInstruction = method.Body.Instructions.First(i =>
                    i.IsMethodCallOp()
                    && i.Next?.Operand is FieldReference f
                    && f.DeclaringType.FullName
                        == "FilteredDirectoryUnavailableTypesAssembly.UnavailableFieldHolder"
                );

                Assert.Null(callInstruction.GetAssigneeFieldDefinition());
            }
            finally
            {
                module.Dispose();
                resolver.Dispose();
            }
        }

        /// <summary>
        /// End-to-end check that <see cref="ArchLoader" /> can load and build an architecture
        /// from FilteredDirectoryLoaderTestAssembly, including its direct dependencies,
        /// when one of those dependencies (FilteredDirectoryUnavailableTypesAssembly) cannot be
        /// resolved. That assembly is not copied to the output directory (Private=False), so
        /// base types such as BaseAttribute and member and field references into it cannot
        /// be resolved during loading.
        ///
        /// The test passes if loading and building finish without throwing an exception.
        ///
        /// Exercises <see cref="ArchLoader.LoadFilteredDirectoryIncludingDependencies" /> →
        /// ... → <see cref="DotNetCoreAssemblyResolver.Resolve(AssemblyNameReference, ReaderParameters)"/>
        /// </summary>
        [Fact]
        public void ArchLoader_HandlesFallbackToDefaultAssemblyResolverForUnavailableAssemblyWithoutException()
        {
            var assemblyDir = FilteredDirectoryLoaderTestAssemblyDir;
            Assert.True(
                Directory.Exists(assemblyDir),
                $"Test assembly directory not found at {assemblyDir}"
            );

            var architecture = new ArchLoader()
                .LoadFilteredDirectoryIncludingDependencies(
                    assemblyDir,
                    "FilteredDirectoryLoaderTestAssembly.dll",
                    false,
                    SearchOption.AllDirectories
                )
                .Build();
        }
    }
}
