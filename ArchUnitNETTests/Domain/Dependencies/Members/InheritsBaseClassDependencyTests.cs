//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class BaseClassTest
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Class _baseClass;
        private readonly Class _childClass;

        public BaseClassTest()
        {
            _baseClass = _architecture.GetClassOfType(typeof(BaseClass));
            _childClass = _architecture.GetClassOfType(typeof(ChildClass));
        }

        [Fact]
        public void ChildClassHasAllMembers()
        {
            Assert.NotNull(_childClass.MembersIncludingInherited["BaseClassMember"]);
            Assert.NotNull(_childClass.MembersIncludingInherited["ChildClassMember"]);
        }

        [Fact]
        public void ChildClassHasBaseClass()
        {
            Assert.Equal(_baseClass, _childClass.BaseClass);

            Assert.True(_childClass.IsAssignableTo(_baseClass));
        }

        [Fact]
        public void ChildClassHasBaseClassDependency()
        {
            var expectedDependency =
                new InheritsBaseClassDependency(_childClass, new TypeInstance<Class>(_baseClass));

            Assert.True(_childClass.HasDependency(expectedDependency));
        }

        [Fact]
        public void OriginAsExpected()
        {
            Assert.All(_childClass.GetInheritsBaseClassDependencies(),
                dependency => Assert.Equal(_childClass, dependency.Origin));
        }
    }

    public abstract class BaseClass
    {
        public string BaseClassMember { get; set; }
    }

    public class ChildClass : BaseClass
    {
        public string ChildClassMember { get; set; }
    }

    // ReSharper disable once UnusedTypeParameter
    public abstract class GenericBaseClass<TSelf> where TSelf : class
    {
    }

    public class ChildClassOfGeneric : GenericBaseClass<ChildClassOfGeneric>
    {
    }
}