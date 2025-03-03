using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class CastDependenciesTest
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(ClassWithCastDependency).Assembly)
            .Build();

        private readonly Class _castClassA;
        private readonly Class _castClassB;
        private readonly Interface _castInterfaceA;
        private readonly Class _classWithCastDependency;
        private readonly MethodMember _methodWithCastDependency;

        public CastDependenciesTest()
        {
            _castClassA = Architecture.GetClassOfType(typeof(CastClassA));
            _castClassB = Architecture.GetClassOfType(typeof(CastClassB));
            _castInterfaceA = Architecture.GetInterfaceOfType(typeof(ICastInterfaceA));
            _classWithCastDependency = Architecture.GetClassOfType(typeof(ClassWithCastDependency));
            _methodWithCastDependency = (MethodMember)
                _classWithCastDependency
                    .Members.WhereNameIs(
                        "MethodWithCastDependencies(ArchUnitNETTests.Dependencies.CastClassA)"
                    )
                    .ToList()
                    .First();
        }

        [SkipInReleaseBuild]
        public void CastTest()
        {
            var typeDependencies = _classWithCastDependency
                .GetTypeDependencies(Architecture)
                .ToList();

            Assert.Contains(_castClassA, typeDependencies);
            Assert.Contains(_castClassB, typeDependencies);
            Assert.Contains(_castInterfaceA, typeDependencies);
        }

        [Fact]
        public void MethodCastTest()
        {
            var typeDependencies = _methodWithCastDependency
                .GetTypeDependencies(Architecture)
                .ToList();
            Assert.Contains(_castClassB, typeDependencies);
        }
    }

    internal class ClassWithCastDependency
    {
        private CastClassB target;

        public ClassWithCastDependency()
        {
            var type = (CastClassA)new CastClassB();
            var type2 = (ICastInterfaceA)new CastClassB();
        }

        public void MethodWithCastDependencies(CastClassA value)
        {
            target = (CastClassB)value;
        }
    }

    internal class CastClassA { }

    internal interface ICastInterfaceA { }

    internal class CastClassB : CastClassA, ICastInterfaceA { }
}
