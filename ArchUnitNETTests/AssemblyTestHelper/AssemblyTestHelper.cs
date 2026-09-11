using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using VerifyXunit;
using Xunit;

namespace ArchUnitNETTests.AssemblyTestHelper;

public abstract class AssemblyTestHelper
{
    private readonly StringBuilder _snapshot = new StringBuilder();
    private bool _recordSnapshot = true;

    public readonly string NonExistentObjectName = "NotTheNameOfAnyObject";

    public abstract Architecture Architecture { get; }

    public AssemblyTestHelper WithoutSnapshot()
    {
        _recordSnapshot = false;
        return this;
    }

    private void Record(string output)
    {
        if (_recordSnapshot)
        {
            _snapshot.Append(output);
        }
    }

    public void AddSnapshotHeader(string header)
    {
        _snapshot.AppendLine("===== " + header + " =====\n");
    }

    public void AddSnapshotSubHeader(string subHeader)
    {
        _snapshot.AppendLine("----- " + subHeader + " -----\n");
    }

    /// <summary>
    /// Evaluation results come out in loader order, which derives from Mono.Cecil's traversal: deterministic
    /// per build, but not contractually stable across Cecil or runtime upgrades. Sorting here keeps the
    /// snapshots stable without touching <see cref="EvaluationResultExtensions.ToErrorMessage"/>, which is
    /// production output.
    /// </summary>
    private static string FormatSnapshot(IArchRule rule, IReadOnlyList<EvaluationResult> results)
    {
        var sorted = results.OrderBy(result => result.ToString(), StringComparer.Ordinal).ToList();
        var formatted = new StringBuilder();
        formatted.AppendLine("Query: " + rule.Description);
        foreach (var result in sorted)
        {
            formatted.AppendLine("Result: " + result.Passed.ToString());
            formatted.AppendLine("Description: " + result.ToString());
        }
        formatted.AppendLine("Message: ");
        formatted.AppendLine(sorted.ToErrorMessage());
        formatted.AppendLine();
        return formatted.ToString();
    }

    /// <summary>
    /// <see cref="ArchRule{TRuleType}.Evaluate" /> injects a synthetic failing result when a rule produces
    /// no results at all. That placeholder carries the rule itself as its evaluated object, which no real
    /// per-object result ever does.
    /// </summary>
    private static bool HasNoRealResults(IEnumerable<EvaluationResult> results)
    {
        return results.Any(result => result.EvaluatedObject is ICanBeEvaluated);
    }

    /// <summary>
    /// Asserts the rule's verdict -- <see cref="IArchRule.HasNoViolations" /> -- rather than the
    /// shape of its results. The distinction matters for
    /// <see cref="CombinedArchRule" />: its verdict applies the conjunction, while its
    /// <see cref="CombinedArchRule.Evaluate" /> merely concatenates both operands' results and
    /// never applies it. A passing <c>Or</c> whose first operand fails therefore has failing
    /// results, which is why this assertion cannot be expressed as
    /// <c>results.All(r =&gt; r.Passed)</c>, and why <see cref="AssertAnyViolations" /> and
    /// <see cref="AssertOnlyViolations" /> -- which do read the results -- say nothing about a
    /// combinator.
    /// </summary>
    public void AssertNoViolations(IArchRule rule)
    {
        var results = rule.Evaluate(Architecture).ToList();
        var output = FormatSnapshot(rule, results);
        if (!rule.HasNoViolations(Architecture))
        {
            Assert.Fail(output);
        }
        Record(output);
    }

    /// <summary>
    /// The exact negation of <see cref="AssertNoViolations" />, and like it a statement about the
    /// verdict, not the results -- see there for why the two differ. Use it wherever the point of
    /// the test is that a rule fails as a whole; prefer <see cref="AssertOnlyViolations" /> when
    /// the point is that every evaluated object was reported, since this assertion is also
    /// satisfied by the failing placeholder <see cref="ArchRule{TRuleType}.Evaluate" /> injects
    /// for a rule whose input set is empty.
    /// </summary>
    public void AssertHasViolations(IArchRule rule)
    {
        var results = rule.Evaluate(Architecture).ToList();
        var output = FormatSnapshot(rule, results);
        if (rule.HasNoViolations(Architecture))
        {
            Assert.Fail(output);
        }
        Record(output);
    }

    public void AssertAnyViolations(IArchRule rule)
    {
        var results = rule.Evaluate(Architecture).ToList();
        var output = FormatSnapshot(rule, results);
        if (results.All(result => !result.Passed))
        {
            Assert.Fail("AssertOnlyViolations should be used for tests without passing results.");
        }
        if (results.All(result => result.Passed))
        {
            Assert.Fail(output);
        }
        Record(output);
    }

    public void AssertOnlyViolations(IArchRule rule)
    {
        AssertOnlyViolations(rule, false);
    }

    public void AssertOnlyViolations(IArchRule rule, bool allowNoResults)
    {
        var results = rule.Evaluate(Architecture).ToList();
        var output = FormatSnapshot(rule, results);
        if (results.Any(result => result.Passed))
        {
            Assert.Fail(output);
        }
        if (!allowNoResults && HasNoRealResults(results))
        {
            Assert.Fail(
                "The rule produced no results, i.e. the input set is empty, so this assertion passes "
                    + "vacuously. Pass allowNoResults: true if that is intended.\n"
                    + output
            );
        }
        Record(output);
    }

    public void AssertException<T>(IArchRule rule)
        where T : Exception
    {
        var exception = Assert.Throws<T>(() => rule.Evaluate(Architecture));
        _snapshot.AppendLine("Query: " + rule.Description);
        _snapshot.AppendLine("Exception: " + exception.Message);
        _snapshot.AppendLine();
    }

    public Task AssertSnapshotMatches([CallerFilePath] string sourceFile = "")
    {
        if (!_recordSnapshot)
        {
            Assert.Fail(
                "This helper was created with WithoutSnapshot(), so nothing was recorded and the "
                    + "snapshot would verify as empty. Drop the WithoutSnapshot() call or the "
                    + "AssertSnapshotMatches() call."
            );
        }

        return Verifier
            .Verify(_snapshot.ToString(), null, sourceFile)
            .DisableDiff() // Don't open diff tool during the test
            .UseDirectory("Snapshots");
    }
}
