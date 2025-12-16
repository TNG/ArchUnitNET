using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;
using static ArchUnitNET.Domain.Visibility;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class PropertyDependencyTests
    {
        public PropertyDependencyTests()
        {
            _classWithPropertyA = _architecture.GetClassOfType(typeof(ClassWithPropertyA));
            _propertyType = _architecture.GetClassOfType(typeof(PropertyType));
            _propertyAMember = _classWithPropertyA.Members["PropertyA"] as PropertyMember;
            _privatePropertyAMember =
                _classWithPropertyA.Members["PrivatePropertyA"] as PropertyMember;
        }

        private readonly Architecture _architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Class _classWithPropertyA;
        private readonly PropertyMember _propertyAMember;
        private readonly PropertyMember _privatePropertyAMember;
        private readonly Class _propertyType;

        [Fact]
        public void ClassDependencyForPropertyMemberTypesAreCreated()
        {
            var expectedDependency = new PropertyTypeDependency(_propertyAMember);

            Assert.True(_classWithPropertyA.HasDependency(expectedDependency));
        }

        [Fact]
        public void PrivatePropertyMembersAreCreatedWithCorrectVisibility()
        {
            Assert.Equal(Private, _privatePropertyAMember?.Visibility);
            Assert.Equal(Private, _privatePropertyAMember?.SetterVisibility);
        }

        [Fact]
        public void PropertyDependenciesAddedToClassDependencies()
        {
            var propertyMembers = _classWithPropertyA.GetPropertyMembers();
            propertyMembers.ForEach(propertyMember =>
                Assert.True(_classWithPropertyA.HasDependencies(propertyMember.Dependencies))
            );
        }

        [Fact]
        public void PropertyMembersAreCreated()
        {
            Assert.Equal(_classWithPropertyA, _propertyAMember?.DeclaringType);
            Assert.Equal(Public, _propertyAMember?.Visibility);
            Assert.Equal(_propertyType, _propertyAMember?.Type);
        }
    }

    public class ClassWithPropertyA
    {
        public PropertyType PropertyA { get; private set; }

        private PropertyType PrivatePropertyA { get; set; }
    }

    public class PropertyType
    {
        private object _field;

        public PropertyType()
        {
            _field = null;
        }

        public PropertyType(object field)
        {
            _field = field;
        }
    }
}
