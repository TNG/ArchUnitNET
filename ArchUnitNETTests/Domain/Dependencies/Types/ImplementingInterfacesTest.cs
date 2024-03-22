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
using static ArchUnitNETTests.Domain.StaticTestTypes;

namespace ArchUnitNETTests.Domain.Dependencies.Types
{
    public class ImplementingInterfacesTest
    {
        private readonly Interface _implementingInterface;
        private readonly Interface _inheritedTestInterface;
        private readonly Class _inheritingType;

        public ImplementingInterfacesTest()
        {
            _implementingInterface = InheritingInterface;
            _inheritedTestInterface = InheritedTestInterface;
            _inheritingType = StaticTestTypes.InheritingType;
        }

        [Fact]
        public void InheritingTypeImplementsInheritedInterface()
        {
            var expectedDependency = new ImplementsInterfaceDependency(
                _inheritingType,
                new TypeInstance<Interface>(_implementingInterface)
            );

            Assert.True(_inheritingType.HasDependency(expectedDependency));
            Assert.True(_inheritingType.ImplementsInterface(_implementingInterface));
            Assert.True(_inheritingType.ImplementsInterface(_inheritedTestInterface));
        }

        [Fact]
        public void OriginAsExpected()
        {
            Assert.All(
                _inheritingType.GetImplementsInterfaceDependencies(),
                dependency => Assert.Equal(_inheritingType, dependency.Origin)
            );
        }
    }
}
