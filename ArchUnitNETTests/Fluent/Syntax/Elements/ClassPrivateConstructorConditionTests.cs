using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using TestAssembly.Domain.Entities;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ClassPrivateConstructorConditionTests
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssembly(typeof(ClassWithPrivateParameterlessConstructor).Assembly).Build();

    [Fact]
    public void HavePrivateParameterlessConstructor_ClassWithPrivateParameterlessConstructor_DoesNotViolate()
    {
        var rule = Classes()
            .That().HaveName(nameof(ClassWithPrivateParameterlessConstructor))
            .Should().HavePrivateParameterlessConstructor();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void HavePrivateParameterlessConstructor_ClassWithoutPrivateParameterlessConstructor_Violates()
    {
        var rule = Classes()
            .That().HaveName(nameof(ClassWithoutPrivateParameterlessConstructor))
            .Should().HavePrivateParameterlessConstructor();

        Assert.False(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void NotHavePrivateParameterlessConstructor_ClassWithoutPrivateParameterlessConstructor_DoesNotViolate()
    {
        var rule = Classes()
            .That().HaveName(nameof(ClassWithoutPrivateParameterlessConstructor))
            .Should().NotHavePrivateParameterlessConstructor();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void NotHavePrivateParameterlessConstructor_ClassWithPrivateParameterlessConstructor_Violates()
    {
        var rule = Classes()
            .That().HaveName(nameof(ClassWithPrivateParameterlessConstructor))
            .Should().NotHavePrivateParameterlessConstructor();

        Assert.False(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void HavePrivateParameterlessConstructor_AbstractClass_DoesNotViolate()
    {
        var rule = Classes()
            .That().AreAbstract()
            .Should().HavePrivateParameterlessConstructor();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void HavePrivateParameterlessConstructor_ClassWithOnlyParameterizedConstructors_Violates()
    {
        var rule = Classes()
            .That().HaveName(nameof(ClassWithOnlyParameterizedConstructors))
            .Should().HavePrivateParameterlessConstructor();

        Assert.False(rule.HasNoViolations(Architecture));
    }
}