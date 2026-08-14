using System;
using System.Linq;
using ArchUnitNET.Fluent.Slices;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    ///     Pins which slice patterns are rejected and with what message. Patterns are validated
    ///     when the rule is defined, so no architecture is needed to provoke the failure.
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
                SliceRuleDefinition.Slices().Matching(pattern)
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
                SliceRuleDefinition.Slices().Matching("Foo.(*).(**)")
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
                SliceRuleDefinition.Slices().Matching("Foo.(**).(**)")
            );
            Assert.Contains(
                "can contain (**) only once",
                ex.Message,
                StringComparison.OrdinalIgnoreCase
            );
        }

        [Theory]
        [InlineData("Foo...(*)", "more than two '.' in a row")]
        [InlineData("Foo.**.(*)", "more than one '*' in a row")]
        [InlineData("Foo.(..).Bar", "does not support capturing via (..)")]
        [InlineData("Foo.[Bar].(*)", "without specifying any alternative via '|'")]
        [InlineData("Foo.Bar|Baz.(*)", "only supports '|' inside of '[]' or '()'")]
        [InlineData("Foo.((*)).Bar", "does not support nesting")]
        public void MalformedPatternThrows(string pattern, string expectedMessage)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                SliceRuleDefinition.Slices().Matching(pattern)
            );
            Assert.Contains(expectedMessage, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("Foo.(*).Bar")]
        [InlineData("Foo.(**).Bar")]
        [InlineData("Foo.(**)..")]
        [InlineData("Foo.(*)..")]
        [InlineData("Foo.(*).(*)")]
        [InlineData("Foo.[Bar|Baz].(*)")]
        public void ValidPatternDoesNotThrow(string pattern)
        {
            SliceRuleDefinition.Slices().Matching(pattern).GetObjects(Architecture).ToList();
        }

        private static ArchUnitNET.Domain.Architecture Architecture =>
            StaticTestArchitectures.SlicesTestArchitecture;
    }
}
