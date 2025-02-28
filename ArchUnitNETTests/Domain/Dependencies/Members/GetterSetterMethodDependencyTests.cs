using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class GetterSetterMethodDependencyTests
    {
        private readonly Architecture _architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        public GetterSetterMethodDependencyTests()
        {
            var getterExampleClass = _architecture.GetClassOfType(
                typeof(GetterMethodDependencyExamples)
            );
            getterExampleClass.RequiredNotNull();
        }

        [Theory]
        [ClassData(typeof(GetterSetterTestsBuild.SetterTestData))]
        public void AssertSetterMethodDependencies(
            PropertyMember backedProperty,
            Class expectedTarget
        )
        {
            if (backedProperty.Setter != null)
            {
                foreach (var dependency in backedProperty.Setter.Dependencies)
                {
                    Assert.Contains(dependency, backedProperty.Dependencies);
                }
            }
            Assert.Contains(expectedTarget, backedProperty.Dependencies.Select(t => t.Target));
        }

        [Theory]
        [ClassData(typeof(GetterSetterTestsBuild.GetterTestData))]
        public void AssertGetterMethodDependencies(
            PropertyMember propertyMember,
            IType mockTargetType,
            MethodCallDependency expectedDependency
        )
        {
            Assert.NotEmpty(propertyMember.MemberDependencies);
            Assert.Single(propertyMember.GetMethodCallDependencies());
            Assert.Contains(
                mockTargetType,
                propertyMember.GetMethodCallDependencies().Select(dependency => dependency.Target)
            );
            Assert.Contains(
                expectedDependency.TargetMember.FullName,
                propertyMember
                    .GetMethodCallDependencies()
                    .Select(dependency => dependency.TargetMember.FullName)
            );
        }

        [Theory]
        [ClassData(typeof(GetterSetterTestsBuild.AccessMethodDependenciesByPropertyTestData))]
        public void AccessorMethodDependenciesByProperty(
            PropertyMember accessedProperty,
            MethodMember accessorMethod
        )
        {
            accessorMethod.MemberDependencies.ForEach(dependency =>
            {
                Assert.Contains(dependency, accessedProperty.MemberDependencies);
            });
        }
    }
}
