/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;
using ArchUnitNET.Matcher;
using ArchUnitNETTests.Fluent;
using Xunit;

namespace ArchUnitNETTests.Dependencies.Members
{
    public class BaseClassTest
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitCsTestArchitecture;

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
            var expectedDependency = new InheritsBaseClassDependency(_childClass, _baseClass);

            Assert.True(_childClass.HasDependency(expectedDependency));
        }

        [Fact]
        public void OriginAsExpected()
        {
            _childClass.GetInheritsBaseClassDependencies().ShouldAll(dependency =>
                dependency.Origin.Equals(_childClass));
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