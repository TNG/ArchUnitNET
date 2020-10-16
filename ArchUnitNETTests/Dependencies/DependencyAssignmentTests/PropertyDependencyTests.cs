//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies.DependencyAssignmentTests
{
    public class PropertyDependencyTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(PropertyDependencyTests).Assembly).Build();

        private readonly Class _dependOnClass;
        private readonly PropertyMember _propertyMemberClassA;
        private readonly Class _propertyTestClass;

        public PropertyDependencyTests()
        {
            _propertyTestClass = Architecture.GetClassOfType(typeof(PropertyTestDataClass));
            _dependOnClass = Architecture.GetClassOfType(typeof(PropertyDependOnClass));
            _propertyMemberClassA =
                _propertyTestClass.GetPropertyMembersWithName("TestStringProperty").ToList().First();
        }

        [Fact]
        public void PropertyDependencyMethod()
        {
            Assert.Contains(_dependOnClass, _propertyMemberClassA.Dependencies.Select(d => d.Target));
        }

        [Fact]
        public void PropertyDependencyGetterFromProperty()
        {
            if (_propertyMemberClassA.Getter != null)
            {
                Assert.Contains(_dependOnClass,
                    _propertyMemberClassA.Getter.Dependencies.Select(d => d.Target));
            }
            else
            {
                Assert.True(false, "Property must have a getter");
            }
        }

        [Fact]
        public void PropertyDependencyGetterFromClassDependenciesOriginMember()
        {
            var getterMethod = _propertyTestClass.Dependencies
                .Where(t => t.Target.FullName.Contains("PropertyDependOnClass")).Cast<BodyTypeMemberDependency>()
                .ToList().First().OriginMember;
            Assert.Contains(_dependOnClass, getterMethod.Dependencies.Select(d => d.Target));
        }

        [Fact]
        public void PropertyDependencyClass()
        {
            Assert.Contains(_dependOnClass, _propertyTestClass.Dependencies.Select(d => d.Target));
        }
    }

    public class PropertyTestDataClass
    {
        public string TestStringProperty
        {
            get
            {
                var a = new PropertyDependOnClass();
                return "";
            }
        }
    }

    public class PropertyDependOnClass
    {
    }
}