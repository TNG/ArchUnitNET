using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArchUnitNET.Fluent.Slices;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    /// A slice is identified by its name, so grouping types by name has to give the same slices
    /// as grouping them by what the pattern's capture groups matched. Otherwise two types with
    /// different captures would end up in one slice. <see cref="SliceRuleInitializer"/> explains
    /// why this holds; these tests pin it down for each kind of pattern that argument covers.
    /// </summary>
    public class SliceNameTests
    {
        private const string TestRoot = "SlicesTestAssembly.MultipleSubnamespaces.";
        private const string DotDotRoot = "SlicesTestAssembly.DotDotSemantics.";

        /// <summary>
        /// Near-misses: namespaces that share segments in different positions, so that a name
        /// which could be split into its captures in more than one way would show up.
        /// </summary>
        private static readonly string[] MadeUpNamespaces =
        {
            "App.A",
            "App.AB",
            "App.ABC",
            "App.A.B",
            "App.AB.C",
            "App.A.BC",
            "App.A.B.C",
            "App.A.A.B",
            "App.A.B.B",
            "App.X.A.B",
            "App.A.B.A.B",
            "App.A.Service.B",
            "App.A.Service.B.C",
            "App.A.B.Service.C",
            "App.Orders.Service.Http",
            "App.Orders.Web.Http",
            "App.Orders.Web.Mid.Http",
            "App.A.Web",
            "App.A.B.Web",
            "App.A.B.X.Web",
            "App.A.Api",
            "Lib.A.Web",
            "Lib.App.A.B",
        };

        [Theory]
        // A single capture group
        [InlineData("App.(*)")]
        [InlineData("App.(**)")]
        // Plain text between the groups, including none
        [InlineData("App.(*).(*)")]
        [InlineData("App.(*).Service.(*)")]
        [InlineData("App.(*)(*)")]
        [InlineData("App.(*).(*)..")]
        // "..", "*" or an alternation between the groups
        [InlineData("App.(*)..(*)")]
        [InlineData("App.(*).*.(*)")]
        [InlineData("App.(*).[Service|Web].(*)")]
        [InlineData("App.(**)..(Web|Api)")]
        // A dotted alternative next to "(**)"
        [InlineData("App.(**).(A|A.B)")]
        [InlineData("App.(A|A.B).(**)")]
        [InlineData("App.(B|A.B).(**)")]
        // A prefix that matches more than one way
        [InlineData("..(*).(*)")]
        [InlineData("*.(**)..(Web|Api)")]
        public void SameName_OnlyForSameCaptures(string pattern)
        {
            var (regex, separators) = SliceRuleInitializer.ConvertPatternToRegex(pattern);
            var matches = Namespaces()
                .Select(nameSpace => regex.Match(nameSpace))
                .Where(match => match.Success)
                .ToList();
            Assert.NotEmpty(matches);

            foreach (
                var slice in matches.GroupBy(match =>
                    SliceRuleInitializer.SliceName(match, separators)
                )
            )
            {
                var captures = slice.Select(Captures).Distinct().ToList();
                Assert.True(
                    captures.Count == 1,
                    $"\"{pattern}\" names different captures \"{slice.Key}\": "
                        + string.Join(", ", captures.Select(c => "[" + c + "]"))
                );
            }
        }

        /// <summary>
        /// The made-up namespaces, plus every namespace of the slice test assembly with its root
        /// replaced by "App.", so that the real test namespaces are checked too.
        /// </summary>
        private static IEnumerable<string> Namespaces()
        {
            var real = StaticTestArchitectures
                .SlicesTestArchitecture.Namespaces.Select(nameSpace => nameSpace.FullName)
                .SelectMany(nameSpace =>
                    new[] { TestRoot, DotDotRoot }
                        .Where(root => nameSpace.StartsWith(root))
                        .Select(root => "App." + nameSpace.Substring(root.Length))
                );
            return MadeUpNamespaces.Concat(real).Distinct();
        }

        private static string Captures(Match match)
        {
            // Group 0 is the whole match; the rest are the pattern's capture groups in order.
            return string.Join(
                "|",
                match.Groups.Cast<Group>().Skip(1).Select(group => group.Value)
            );
        }
    }
}
