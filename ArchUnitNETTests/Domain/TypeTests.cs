using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNETTests.Domain
{
    public class TypeTests
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        [Fact]
        public void TypesAreClassesAndInterfaces()
        {
            var types = Architecture.Types;
            var classes = Architecture.Classes;
            var interfaces = Architecture.Interfaces;
            Assert.True(types.All(type => classes.Contains(type) ^ interfaces.Contains(type)));
            Assert.True(classes.All(cls => types.Contains(cls)));
            Assert.True(interfaces.All(intf => types.Contains(intf)));
        }

        [Fact]
        public void TypesMustHaveVisibility()
        {
            Assert.True(Architecture.Types.All(type => type.Visibility != NotAccessible));
        }
    }
}