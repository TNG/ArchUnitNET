using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class TypeOfDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(TypeOfDependenciesTests).Assembly)
            .Build();

        private readonly Class _classWithTypeOfDependency;
        private readonly Class _dependingTypeOfClass;

        public TypeOfDependenciesTests()
        {
            _classWithTypeOfDependency = Architecture.GetClassOfType(
                typeof(ClassWithTypeOfDependency)
            );
            _dependingTypeOfClass = Architecture.GetClassOfType(typeof(DependingTypeOfClass));
        }

        [SkipInReleaseBuild]
        public void TypeOfDependencyTest()
        {
            var methodMember = _classWithTypeOfDependency.Members.First(member =>
                member.NameContains("MethodWithTypeOfDependency")
            );
            var typeDependencies = _classWithTypeOfDependency.GetTypeDependencies().ToList();
            var methodTypeDependencies = methodMember.GetTypeDependencies().ToList();

            Assert.Contains(_dependingTypeOfClass, typeDependencies);
            Assert.Contains(_dependingTypeOfClass, methodTypeDependencies);
        }
    }

    internal class ClassWithTypeOfDependency
    {
        public void MethodWithTypeOfDependency()
        {
            object a = typeof(DependingTypeOfClass);
        }
    }

    internal class DependingTypeOfClass { }
}
