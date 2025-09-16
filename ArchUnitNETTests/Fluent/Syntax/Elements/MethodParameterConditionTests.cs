using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using TestAssembly.Domain.Methods;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class MethodParameterConditionTests
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssembly(typeof(ClassWithPrivateParameterlessConstructor).Assembly).Build();

    [Fact]
    public void HaveAnyParameters_MethodWithParameters_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That()
            .AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithMethods)))
            .And()
            .HaveName("MethodWithParameters(System.String,System.Int32)")
            .Should()
            .HaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void HaveAnyParameters_MethodWithoutParameters_Violates()
    {
        var rule = MethodMembers()
            .That()
            .AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithMethods)))
            .And()
            .HaveName("MethodWithoutParameters()")
            .Should()
            .HaveAnyParameters();

        Assert.False(rule.HasNoViolations(Architecture));
        
        var evaluation = rule.Evaluate(Architecture);
        var violations = evaluation.ToList();
        Assert.Single(violations);
        Assert.Contains("does not have any parameters", violations.First().Description);
    }

    [Fact]
    public void NotHaveAnyParameters_MethodWithoutParameters_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That()
            .AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithMethods)))
            .And().HaveName("PrivateMethodWithoutParameters()")
            .Should().NotHaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void NotHaveAnyParameters_MethodWithParameters_Violates()
    {
        var rule = MethodMembers()
            .That().HaveName("MethodWithParameters(System.String,System.Int32)")
            .Should().NotHaveAnyParameters();

        Assert.False(rule.HasNoViolations(Architecture));
        
        var evaluation = rule.Evaluate(Architecture);
        var violations = evaluation.ToList();
        Assert.Single(violations);
        Assert.Contains("has parameters", violations.First().Description);
    }

    [Fact]
    public void HaveAnyParameters_ConstructorWithParameters_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That().AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithOnlyParameterizedConstructors)))
            .And().AreConstructors()
            .Should().HaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact] 
    public void HaveAnyParameters_ParameterlessConstructor_Violates()
    {
        var rule = MethodMembers()
            .That().AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithPublicParameterlessConstructor)))
            .And().AreConstructors()
            .And().HaveName(".ctor()")
            .Should().HaveAnyParameters();

        Assert.False(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void PrivateConstructorWithoutParameters_CompositeRule_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That().AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithPrivateParameterlessConstructor)))
            .And().AreConstructors()
            .And().ArePrivate()
            .Should().NotHaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void PublicConstructorWithParameters_CompositeRule_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That().AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithPrivateParameterlessConstructor)))
            .And().AreConstructors()
            .And().ArePublic()
            .Should().HaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void PrivateMethodWithoutParameters_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That()
            .AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithMethods)))
            .And()
            .HaveName("PrivateMethodWithoutParameters()")
            .And()
            .ArePrivate()
            .Should()
            .NotHaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void SpecificClass_AllConstructorsHaveParameters_DoesNotViolate()
    {
        var rule = MethodMembers()
            .That().AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithOnlyParameterizedConstructors)))
            .And().AreConstructors()
            .Should().HaveAnyParameters();

        Assert.True(rule.HasNoViolations(Architecture));
    }

    [Fact]
    public void SpecificClass_HasParameterlessConstructor_Violates()
    {
        var rule = MethodMembers()
            .That().AreDeclaredIn(Classes().That().HaveName(nameof(ClassWithPublicParameterlessConstructor)))
            .And().AreConstructors()
            .Should().HaveAnyParameters();

        Assert.False(rule.HasNoViolations(Architecture));
    }
}
