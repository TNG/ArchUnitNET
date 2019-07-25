/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
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

namespace ArchUnitNETTests.Dependencies.Types
{
    public class ImplementingInterfacesTest
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Interface _testInterface;
        private readonly Interface _inheritedTestInterface;
        private readonly Class _inheritingType;

        public ImplementingInterfacesTest()
        {
            _testInterface = _architecture.GetInterfaceOfType(typeof(ITestInterface));
            _inheritedTestInterface = _architecture.GetInterfaceOfType(typeof(IInheritedTestInterface));
            _inheritingType = _architecture.GetClassOfType(typeof(InheritingType));
        }

        [Fact]
        public void InheritingTypeImplementsInheritedInterface()
        {
            var expectedDependency = new ImplementsInterfaceDependency(_inheritingType, _testInterface);

            Assert.True(_inheritingType.HasDependency(expectedDependency));
            Assert.True(_inheritingType.Implements(_testInterface));
            Assert.True(_inheritingType.Implements(_inheritedTestInterface));
        }

        [Fact]
        public void OriginAsExpected()
        {
            _inheritingType.GetImplementsInterfaceDependencies().ShouldAll(dependency =>
                dependency.Origin.Equals(_inheritingType));
        }
    }

    public interface IInheritedTestInterface
    {
    }

    public interface ITestInterface : IInheritedTestInterface
    {
    }

    public abstract class InheritedType : ITestInterface
    {
    }

    public class InheritingType : InheritedType
    {
    }
}