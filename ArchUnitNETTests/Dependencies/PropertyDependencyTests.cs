using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class PropertyDependencyTests
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Class _dependOnClass;
        private readonly Class _propertyTestDataClass;
        private readonly PropertyMember _testStringProperty;
        private readonly MethodMember _testStringPropertyGetter;

        public PropertyDependencyTests()
        {
            _dependOnClass = Architecture.GetClassOfType(typeof(PropertyDependOnClass));
            _propertyTestDataClass = Architecture.GetClassOfType(typeof(PropertyTestDataClass));
            _testStringProperty = _propertyTestDataClass
                .GetPropertyMembersWithName("TestStringProperty")
                .ToList()
                .First();
            _testStringPropertyGetter = _propertyTestDataClass
                .GetMethodMembersWithName("get_TestStringProperty()")
                .First();
        }

        [Fact]
        public void PropertyGetterClassGetterIdentical()
        {
            if (_testStringProperty.Getter != null)
            {
                Assert.Equal(_testStringPropertyGetter, _testStringProperty.Getter);
            }
            else
            {
                Assert.Fail("Property must have a getter");
            }
        }

        [Fact]
        public void PropertyGetterClassGetterSameDependencies()
        {
            if (_testStringProperty.Getter != null)
            {
                Assert.Equal(
                    _testStringPropertyGetter.Dependencies,
                    _testStringProperty.Getter.Dependencies
                );
            }
            else
            {
                Assert.Fail("Property must have a getter");
            }
        }

        [Fact]
        public void ClassHasDependencyFromProperty()
        {
            Assert.Contains(
                _dependOnClass,
                _propertyTestDataClass.Dependencies.Select(d => d.Target)
            );
        }

        [Fact]
        public void PropertyHasDependencyFromProperty()
        {
            Assert.Contains(
                _dependOnClass,
                _testStringProperty.Dependencies.Select(d => d.Target)
            );
        }

        [Fact]
        public void GetterHasDependencyFromProperty()
        {
            Assert.Contains(
                _dependOnClass,
                _testStringPropertyGetter.Dependencies.Select(d => d.Target)
            );
        }

        [Fact]
        public void ClassHasMethodCallDependencyFromProperty()
        {
            var methodCalls = _propertyTestDataClass
                .Dependencies.Where(d => d is MethodCallDependency)
                .ToList();
            if (methodCalls.IsNullOrEmpty())
            {
                Assert.Fail("Class must have Method Call Dependency");
            }
            Assert.Contains(_dependOnClass, methodCalls.Select(d => d.Target));
        }

        [Fact]
        public void PropertyHasMethodCallDependencyFromProperty()
        {
            var methodCalls = _testStringProperty
                .Dependencies.Where(d => d is MethodCallDependency)
                .ToList();
            if (methodCalls.IsNullOrEmpty())
            {
                Assert.Fail("Property must have Method Call Dependency");
            }
            Assert.Contains(_dependOnClass, methodCalls.Select(d => d.Target));
        }

        [Fact]
        public void GetterHasMethodCallDependencyFromProperty()
        {
            var methodCalls = _testStringPropertyGetter
                .Dependencies.Where(d => d is MethodCallDependency)
                .ToList();
            if (methodCalls.IsNullOrEmpty())
            {
                Assert.Fail("Getter must have Method Call Dependency");
            }
            Assert.Contains(_dependOnClass, methodCalls.Select(d => d.Target));
        }

        [Fact]
        public void PropertyDependencyPassedOn()
        {
            Assert.Equal(
                _testStringPropertyGetter.Dependencies,
                _testStringPropertyGetter.Dependencies.Intersect(
                    _testStringProperty.Dependencies
                )
            );
            Assert.Equal(
                _testStringProperty.Dependencies,
                _testStringProperty.Dependencies.Intersect(
                    _propertyTestDataClass.Dependencies
                )
            );
        }
    }

    public class PropertyTestDataClass
    {
        public string TestStringProperty
        {
            get
            {
                var b = new PropertyDependOnClass();
                b.PropertyDependOnClassMethod();
                return "";
            }
        }
    }

    public class PropertyDependOnClass
    {
        public void PropertyDependOnClassMethod() { }
    }
}
