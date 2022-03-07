//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Domain;
using Xunit;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNETTests.Domain
{
    public class TypeTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        [Fact]
        public void TypesAreClassesAndInterfacesAndStructsAndEnums()
        {
            var types = Architecture.Types.ToList();
            var classes = Architecture.Classes;
            var interfaces = Architecture.Interfaces;
            var structs = Architecture.Structs;
            var enums = Architecture.Enums;
            Assert.True(types.All(type => classes.Contains(type) || interfaces.Contains(type) || structs.Contains(type) || enums.Contains(type)));
            Assert.True(classes.All(cls => types.Contains(cls)));
            Assert.True(interfaces.All(intf => types.Contains(intf)));
            Assert.True(structs.All(str => types.Contains(str)));
            Assert.True(enums.All(en => types.Contains(en)));
        }

        [Fact]
        public void TypesMustHaveVisibility()
        {
            Assert.True(Architecture.Types.All(type => type.Visibility != NotAccessible));
        }
        
        [Fact]
        public void AssignEnumsCorrectly()
        {
            Assert.True(StaticTestTypes.TestEnum is Enum);
        }

        [Fact]
        public void AssignStructsCorrectly()
        {
            Assert.True(StaticTestTypes.TestStruct is Struct);
        }
    }
}