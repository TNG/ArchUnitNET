using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ExampleTest
{
    public class LimitationsOnReleaseTest
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(LimitationsOnReleaseTest).Assembly)
            .Build();

        private static IType _edgeCaseDataClass = Architecture.GetClassOfType(typeof(EdgeCaseData));
        private static IType _missingDependencyClass = Architecture.GetClassOfType(
            typeof(MissingDependencyClass)
        );

        [Fact]
        public void CastTest()
        {
            var typeDependencies = _edgeCaseDataClass
                .Members.Where(t =>
                    t.FullName.Contains(nameof(EdgeCaseData.MethodWithCastDependency))
                )
                .ToList()
                .First()
                .GetTypeDependencies()
                .ToList();

            Assert.Contains(_missingDependencyClass, typeDependencies);
        }

        [Fact]
        public void NullVariableTest()
        {
            var methodWithCastDependencyDependencies = _edgeCaseDataClass
                .Members.Where(t =>
                    t.FullName.Contains(nameof(EdgeCaseData.MethodWithNullVariable))
                )
                .ToList()
                .First()
                .GetTypeDependencies()
                .ToList();

            Assert.Contains(_missingDependencyClass, methodWithCastDependencyDependencies);
        }

        [Fact]
        public void TypeOfDependencyTest()
        {
            var methodWithTypeOfDependencyDependencies = _edgeCaseDataClass
                .Members.Where(t =>
                    t.FullName.Contains(nameof(EdgeCaseData.MethodWithTypeOfDependency))
                )
                .ToList()
                .First()
                .GetTypeDependencies()
                .ToList();

            Assert.Contains(_missingDependencyClass, methodWithTypeOfDependencyDependencies);
        }
    }

#pragma warning disable 219
    internal class EdgeCaseData
    {
        public void MethodWithTypeOfDependency()
        {
            var a = typeof(MissingDependencyClass);
        }

        public void MethodWithCastDependency()
        {
            var a = (MissingDependencyClass)new SubClass();
        }

        public void MethodWithNullVariable()
        {
            MissingDependencyClass a = null;
        }
    }

    internal class MissingDependencyClass { }

    internal class SubClass : MissingDependencyClass { }
}
#pragma warning restore 219
