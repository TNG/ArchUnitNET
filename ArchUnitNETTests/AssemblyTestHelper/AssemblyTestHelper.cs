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
    private StringBuilder snapshot = new StringBuilder();

    public readonly string NonExistentObjectName = "NotTheNameOfAnyObject";

    public abstract Architecture Architecture { get; }

    public void AddSnapshotHeader(string header)
    {
        snapshot.AppendLine("===== " + header + " =====\n");
    }

    public void AddSnapshotSubHeader(string subHeader)
    {
        snapshot.AppendLine("----- " + subHeader + " -----\n");
    }

    private string FormatSnapshot(IArchRule rule, IEnumerable<EvaluationResult> results)
    {
        var formatted = new StringBuilder();
        formatted.AppendLine("Query: " + rule.Description);
        foreach (var result in results)
        {
            formatted.AppendLine("Result: " + result.Passed.ToString());
            formatted.AppendLine("Description: " + result.ToString());
        }
        formatted.AppendLine("Message: ");
        formatted.AppendLine(results.ToErrorMessage());
        formatted.AppendLine();
        return formatted.ToString();
    }

    public void AssertNoViolations(IArchRule rule)
    {
        var results = rule.Evaluate(Architecture);
        var output = FormatSnapshot(rule, results);
        if (!results.All(result => result.Passed))
        {
            Assert.Fail(output);
        }
        snapshot.Append(output);
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
        snapshot.Append(output);
    }

    public void AssertOnlyViolations(IArchRule rule)
    {
        var results = rule.Evaluate(Architecture).ToList();
        var output = FormatSnapshot(rule, results);
        if (results.Any(result => result.Passed))
        {
            Assert.Fail(output);
        }
        snapshot.Append(output);
    }

    public void AssertException<T>(IArchRule rule)
        where T : Exception
    {
        var exception = Assert.Throws<T>(() => rule.Evaluate(Architecture).ToList());
        snapshot.AppendLine("Query: " + rule.Description);
        snapshot.AppendLine("Exception: " + exception.Message);
        snapshot.AppendLine();
    }

    public Task AssertSnapshotMatches([CallerFilePath] string sourceFile = "")
    {
        return Verifier
            .Verify(snapshot.ToString(), null, sourceFile)
            .DisableDiff() // Don't open diff tool during the test
            .UseDirectory("Snapshots");
    }
}
