using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class GivenObjectsThatTests
{
    [Fact]
    public void DependOnPatternTest()
    {
        var t = new DependencyAssemblyTestHelpers();
        var noTypeDependsOnFalseDependency = Types()
            .That()
            .DependOnAny(t.NonExistentObjectName)
            .Should()
            .NotExist();
        var typesDoNotDependsOnFalseDependency = Types()
            .That()
            .DoNotDependOnAny(t.NonExistentObjectName)
            .Should()
            .Exist();

        Assert.True(noTypeDependsOnFalseDependency.HasNoViolations(t.Architecture));
        Assert.True(typesDoNotDependsOnFalseDependency.HasNoViolations(t.Architecture));
    }
}
