using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax.Elements;

public class ShouldRelateToObjectsThatTests
{
    [Fact]
    public async void AreTest()
    {
        var t = new DependencyAssemblyTestHelpers();
        var should = Types().That().Are(t.ChildClass).Should();

        // Checks with actual base type
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are(t.BaseClass.FullName));
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are([t.BaseClass.FullName]));
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are(t.BaseClass));
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are(t.BaseClassSystemType));
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are(Classes().That().Are(t.BaseClass)));
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are([t.BaseClass]));
        t.AssertNoViolations(should.DependOnAnyTypesThat().Are([t.BaseClassSystemType]));

        // Check with non-existent type using string matching
        t.AssertViolations(should.DependOnAnyTypesThat().Are(t.NonExistentObjectName));
        t.AssertViolations(should.DependOnAnyTypesThat().Are([t.NonExistentObjectName]));

        // Checks with non-base type
        t.AssertViolations(should.DependOnAnyTypesThat().Are(t.ClassWithoutDependencies.FullName));
        t.AssertViolations(
            should.DependOnAnyTypesThat().Are([t.ClassWithoutDependencies.FullName])
        );
        t.AssertViolations(should.DependOnAnyTypesThat().Are(t.ClassWithoutDependencies));
        t.AssertViolations(should.DependOnAnyTypesThat().Are(t.ClassWithoutDependenciesSystemType));
        t.AssertViolations(
            should.DependOnAnyTypesThat().Are(Classes().That().Are(t.ClassWithoutDependencies))
        );
        t.AssertViolations(should.DependOnAnyTypesThat().Are([t.ClassWithoutDependencies]));
        t.AssertViolations(
            should.DependOnAnyTypesThat().Are([t.ClassWithoutDependenciesSystemType])
        );

        // Should work with empty lists
        t.AssertViolations(should.DependOnAnyTypesThat().Are(new List<string>()));
        t.AssertViolations(should.DependOnAnyTypesThat().Are(new List<IType>()));
        t.AssertViolations(should.DependOnAnyTypesThat().Are(new List<Type>()));

        // Should work with multiple arguments
        t.AssertNoViolations(
            should
                .DependOnAnyTypesThat()
                .Are([t.ClassWithoutDependencies.FullName, t.BaseClass.FullName])
        );
        t.AssertNoViolations(
            should.DependOnAnyTypesThat().Are(t.ClassWithoutDependencies, t.BaseClass)
        );
        t.AssertNoViolations(
            should.DependOnAnyTypesThat().Are([t.ClassWithoutDependencies, t.BaseClass])
        );
        t.AssertNoViolations(
            should
                .DependOnAnyTypesThat()
                .Are(t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType)
        );
        t.AssertNoViolations(
            should
                .DependOnAnyTypesThat()
                .Are([t.ClassWithoutDependenciesSystemType, t.BaseClassSystemType])
        );

        await t.AssertSnapshotMatches();
    }
}
