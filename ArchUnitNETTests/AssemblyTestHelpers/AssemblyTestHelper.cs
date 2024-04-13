using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Extensions;
using VerifyXunit;
using Xunit;

namespace ArchUnitNETTests;

public abstract class AssemblyTestHelper
{
    private StringBuilder snapshot = new StringBuilder();

    public readonly string NonExistentObjectName = "NotTheNameOfAnyObject";

    public abstract Architecture Architecture { get; }

    public void AssertNoViolations(IArchRule rule)
    {
        snapshot.Append("Query: ");
        snapshot.AppendLine(rule.Description);
        var results = rule.Evaluate(Architecture);
        foreach (var result in results)
        {
            snapshot.Append("Result: ");
            snapshot.AppendLine(result.Passed.ToString());
            snapshot.Append("Description: ");
            snapshot.AppendLine(result.ToString());
        }
        snapshot.AppendLine("Message: ");
        snapshot.AppendLine(results.ToErrorMessage());
        snapshot.AppendLine();
        Assert.True(results.All(result => result.Passed));
    }

    public void AssertViolations(IArchRule rule)
    {
        snapshot.Append("Query: ");
        snapshot.AppendLine(rule.Description);
        var results = rule.Evaluate(Architecture);
        foreach (var result in results)
        {
            snapshot.Append("Result: ");
            snapshot.AppendLine(result.Passed.ToString());
            snapshot.Append("Description: ");
            snapshot.AppendLine(result.ToString());
        }
        snapshot.AppendLine("Message: ");
        snapshot.AppendLine(results.ToErrorMessage());
        snapshot.AppendLine();
        Assert.False(results.All(result => result.Passed));
    }

    public Task AssertSnapshotMatches([CallerFilePath] string sourceFile = "")
    {
        return Verifier
            .Verify(snapshot.ToString(), null, sourceFile)
            .AutoVerify()
            .UseDirectory("Snapshots");
    }
}
