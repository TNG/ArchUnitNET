using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Domain
{
    public class FunctionPointerTests
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly IType _functionPointer;

        public FunctionPointerTests()
        {
            _functionPointer = Architecture
                .GetClassOfType(typeof(ClassWithFunctionPointerField))
                .GetFieldMembers()
                .Single()
                .Type;
        }

        [Fact]
        public void FunctionPointerHasNoNamespaceOrAssembly()
        {
            Assert.IsType<FunctionPointer>(_functionPointer);
            Assert.Contains(_functionPointer, Architecture.ReferencedTypes);
            Assert.Null(_functionPointer.Namespace);
            Assert.Null(_functionPointer.Assembly);
        }

        [Fact]
        public void NamespaceExtensionsReturnFalseForTypeWithoutNamespace()
        {
            Assert.False(_functionPointer.ResidesInNamespace("System"));
            Assert.False(_functionPointer.ResidesInNamespaceMatching(".*"));
        }

        [Fact]
        public void AssemblyExtensionsReturnFalseForTypeWithoutAssembly()
        {
            Assert.False(_functionPointer.ResidesInAssembly("System"));
            Assert.False(_functionPointer.ResidesInAssemblyMatching(".*"));
        }

        [Fact]
        public void NamespacePredicatesHandleTypesWithoutNamespace()
        {
            var namespaceName = typeof(ClassWithFunctionPointerField).Namespace;
            Assert.DoesNotContain(
                _functionPointer,
                Types(true).That().ResideInNamespace(namespaceName).GetObjects(Architecture)
            );
            Assert.DoesNotContain(
                _functionPointer,
                Types(true).That().ResideInNamespaceMatching(".*").GetObjects(Architecture)
            );
            Assert.Contains(
                _functionPointer,
                Types(true).That().DoNotResideInNamespace(namespaceName).GetObjects(Architecture)
            );
            Assert.Contains(
                _functionPointer,
                Types(true).That().DoNotResideInNamespaceMatching(".*").GetObjects(Architecture)
            );
        }

        [Fact]
        public void AssemblyPredicatesHandleTypesWithoutAssembly()
        {
            var assembly = typeof(ClassWithFunctionPointerField).Assembly;
            Assert.DoesNotContain(
                _functionPointer,
                Types(true).That().ResideInAssembly(assembly.FullName).GetObjects(Architecture)
            );
            Assert.DoesNotContain(
                _functionPointer,
                Types(true).That().ResideInAssemblyMatching(".*").GetObjects(Architecture)
            );
            Assert.Contains(
                _functionPointer,
                Types(true).That().DoNotResideInAssembly(assembly.FullName).GetObjects(Architecture)
            );
        }

        [Fact]
        public void NamespaceConditionsReportTypesWithoutNamespace()
        {
            var functionPointers = Types(true).That().Are(_functionPointer);
            AssertViolation(
                functionPointers.Should().ResideInNamespace("System"),
                "does not reside in a namespace"
            );
            AssertViolation(
                functionPointers.Should().ResideInNamespaceMatching(".*"),
                "does not reside in a namespace"
            );
            Assert.True(
                functionPointers
                    .Should()
                    .NotResideInNamespace("System")
                    .HasNoViolations(Architecture)
            );
        }

        [Fact]
        public void AssemblyConditionsReportTypesWithoutAssembly()
        {
            var assembly = typeof(ClassWithFunctionPointerField).Assembly;
            var functionPointers = Types(true).That().Are(_functionPointer);
            AssertViolation(
                functionPointers.Should().ResideInAssembly(assembly.FullName),
                "does not reside in an assembly"
            );
            AssertViolation(
                functionPointers.Should().ResideInAssemblyMatching(".*"),
                "does not reside in an assembly"
            );
            AssertViolation(
                functionPointers.Should().ResideInAssembly(assembly),
                "does not reside in an assembly"
            );
            AssertViolation(
                functionPointers
                    .Should()
                    .ResideInAssembly(Architecture.GetAssemblyOfAssembly(assembly)),
                "does not reside in an assembly"
            );
            Assert.True(
                functionPointers
                    .Should()
                    .NotResideInAssembly(assembly)
                    .HasNoViolations(Architecture)
            );
        }

        private static void AssertViolation(
            ArchUnitNET.Fluent.IArchRule rule,
            string expectedDescription
        )
        {
            var result = rule.Evaluate(Architecture).Single();
            Assert.False(result.Passed);
            Assert.Contains(expectedDescription, result.Description);
        }
    }

    public unsafe class ClassWithFunctionPointerField
    {
        public delegate* <int, void> FunctionPointerField;
    }
}
