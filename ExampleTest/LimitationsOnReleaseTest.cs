using System.Linq;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

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

        [Fact]
        public void AsyncMethodDependencyTest()
        {
            Classes()
                .That()
                .Are(typeof(AsyncUser))
                .Should()
                .CallAny(MethodMembers().That().HaveName("MethodAsync()"))
                .Check(Architecture);
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

    internal class AsyncService
    {
        public async Task MethodAsync()
        {
            await Task.Delay(100);
        }
    }

    internal class AsyncUser
    {
        private readonly AsyncService _asyncService = new AsyncService();

        public async Task UseAsyncService()
        {
            await _asyncService.MethodAsync();
        }
    }
}
#pragma warning restore 219
