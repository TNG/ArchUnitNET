using System;
using System.Linq;
using ArchUnitNET.Fluent.Slices;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    ///     Pins which slice patterns are rejected and with what message. The patterns are
    ///     validated while the slices are enumerated rather than when the rule is defined, so
    ///     every case here has to consume the result before the exception surfaces.
    /// </summary>
    public class PatternValidationTests
    {
        [Theory]
        [InlineData("Foo.Bar")]
        [InlineData("Foo.*")]
        [InlineData("Foo..Bar")]
        public void PatternWithoutCaptureGroupThrows(string pattern)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                SliceRuleDefinition.Slices().Matching(pattern).GetObjects(Architecture).ToList()
            );
            Assert.Contains(
                "have to contain (*) or (**)",
                ex.Message,
                StringComparison.OrdinalIgnoreCase
            );
        }

        [Fact]
        public void PatternMixingSingleAndDoubleAsteriskThrows()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("Foo.(*).(**)")
                    .GetObjects(Architecture)
                    .ToList()
            );
            Assert.Contains(
                "can't contain both (*) and (**)",
                ex.Message,
                StringComparison.OrdinalIgnoreCase
            );
        }

        [Fact]
        public void PatternWithRepeatedDoubleAsteriskThrows()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                SliceRuleDefinition
                    .Slices()
                    .Matching("Foo.(**).(**)")
                    .GetObjects(Architecture)
                    .ToList()
            );
            Assert.Contains(
                "can contain (**) only once",
                ex.Message,
                StringComparison.OrdinalIgnoreCase
            );
        }

        [Theory]
        [InlineData("Foo.(*).Bar")]
        [InlineData("Foo.(**).Bar")]
        [InlineData("Foo.(**)..")]
        [InlineData("Foo.(*)..")]
        [InlineData("Foo.(*).(*)")]
        public void ValidPatternDoesNotThrow(string pattern)
        {
            SliceRuleDefinition.Slices().Matching(pattern).GetObjects(Architecture).ToList();
        }

        private static ArchUnitNET.Domain.Architecture Architecture =>
            StaticTestArchitectures.SlicesTestArchitecture;
    }
}
