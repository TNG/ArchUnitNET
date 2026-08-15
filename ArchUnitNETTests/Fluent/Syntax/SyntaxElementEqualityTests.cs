using System.Linq;
using ArchUnitNET.Loader;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax;

/// <summary>
/// Syntax elements are the identity of the rule evaluation cache: Architecture.GetOrCreateObjects keys on
/// the object provider's hash code and runtime type. A wrong equality or hash implementation therefore
/// hands one rule another rule's analyzed objects. The syntax element tests all run against architectures
/// built WithoutRuleEvaluationCache, so they never exercise that path.
/// </summary>
public class SyntaxElementEqualityTests
{
    [Fact]
    public void EqualsRejectsNullAndForeignTypes()
    {
        var element = Classes().That().AreAbstract();

        Assert.False(element.Equals(null));
        Assert.False(element.Equals("not a syntax element"));
        Assert.True(element.Equals(element));
    }

    [Fact]
    public void EqualityIsBasedOnTheRuleCreator()
    {
        var element = Classes().That().AreAbstract();
        var equivalent = Classes().That().AreAbstract();
        var different = Classes().That().AreSealed();

        Assert.True(element.Equals(equivalent));
        Assert.Equal(element.GetHashCode(), equivalent.GetHashCode());

        Assert.False(element.Equals(different));
        Assert.NotEqual(element.GetHashCode(), different.GetHashCode());
    }

    [Fact]
    public void ElementsSharingARuleCreatorAreEqualOnlyWithinTheirType()
    {
        var given = Classes();

        // That() hands the very same rule creator to a new element, so two of them are equal without
        // being the same object, while the element they came from is of a different type and is not.
        Assert.True(given.That().Equals(given.That()));
        Assert.Equal(given.That().GetHashCode(), given.That().GetHashCode());
        Assert.False(given.Equals(given.That()));
    }

    [Fact]
    public void CachedProvidersDoNotHandOutEachOthersObjects()
    {
        // Deliberately built with the rule evaluation cache enabled, unlike every architecture in
        // StaticTestArchitectures that the syntax element tests use.
        var architecture = new ArchLoader()
            .LoadAssemblies(typeof(ClassNamespace.RegularClass).Assembly)
            .Build();

        var abstractClasses = Classes().That().AreAbstract();
        var sealedClasses = Classes().That().AreSealed();

        var abstractResult = abstractClasses.GetObjects(architecture).ToList();
        var sealedResult = sealedClasses.GetObjects(architecture).ToList();

        Assert.NotEmpty(abstractResult);
        Assert.NotEmpty(sealedResult);
        Assert.Empty(abstractResult.Intersect(sealedResult));

        // Re-asking the same provider, and an equal but distinct one, must both still yield the same set.
        Assert.Equal(abstractResult, abstractClasses.GetObjects(architecture).ToList());
        Assert.Equal(
            abstractResult,
            Classes().That().AreAbstract().GetObjects(architecture).ToList()
        );
    }
}
