//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Domain.Dependencies.Types
{
    public class ImplementingInterfacesTest
    {
        public ImplementingInterfacesTest()
        {
            _implementingInterface = InheritingInterface;
            _inheritedTestInterface = InheritedTestInterface;
            _inheritingType = StaticTestTypes.InheritingType;
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
            Assert.True(_inheritingType.ImplementsInterface(_implementingInterface));
            Assert.True(_inheritingType.ImplementsInterface(_inheritedTestInterface));
        }

        [Fact]
        public void OriginAsExpected()
        {
            Assert.All(_inheritingType.GetImplementsInterfaceDependencies(),
                dependency => Assert.Equal(_inheritingType, dependency.Origin));
        }
    }
}