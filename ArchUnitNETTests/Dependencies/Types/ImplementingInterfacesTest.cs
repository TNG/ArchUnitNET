/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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