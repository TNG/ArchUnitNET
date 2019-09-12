/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Matcher;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Dependencies.Types
{
    public class ImplementingInterfacesTest
    {
        public ImplementingInterfacesTest()
        {
            _implementingInterface = InheritingInterface;
            _inheritedTestInterface = InheritedTestInterface;
            _inheritingType = InheritingType;
        }

        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Interface _implementingInterface;
        private readonly Interface _inheritedTestInterface;
        private readonly Class _inheritingType;

        [Fact]
        public void InheritingTypeImplementsInheritedInterface()
        {
            var expectedDependency = new ImplementsInterfaceDependency(_inheritingType, _implementingInterface);

            Assert.True(_inheritingType.HasDependency(expectedDependency));
            Assert.True(_inheritingType.Implements(_implementingInterface));
            Assert.True(_inheritingType.Implements(_inheritedTestInterface));
        }

        [Fact]
        public void OriginAsExpected()
        {
            _inheritingType.GetImplementsInterfaceDependencies().ShouldAll(dependency =>
                dependency.Origin.Equals(_inheritingType));
        }
    }
}