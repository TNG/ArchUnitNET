//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using TestAssembly;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class ObjectConditionsErrorMessagesTests
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture;

        private void AssertFailsWithErrorMessage(IArchRule rule, string errorMessage)
        {
            var evaluationResults = rule.Evaluate(Architecture);
            var failedResults = evaluationResults.Where(r => !r.Passed);
            Assert.Equal(errorMessage, failedResults.Single().Description);
        }

        [Fact]
        public void OnlyListForbiddenITypesInError()
        {
            var rule = Classes()
                .That()
                .Are(typeof(Class1))
                .Should()
                .NotDependOnAnyTypesThat()
                .Are(typeof(Class2));
            AssertFailsWithErrorMessage(
                rule,
                "TestAssembly.Class1 does depend on TestAssembly.Class2"
            );
        }

        [Fact]
        public void OnlyListForbiddenTypesInError()
        {
            var rule = Classes().That().Are(typeof(Class1)).Should().NotDependOnAny(typeof(Class2));
            AssertFailsWithErrorMessage(
                rule,
                "TestAssembly.Class1 does depend on TestAssembly.Class2"
            );
        }

        [Fact]
        public void OnlyListForbiddenMethodsInErrors()
        {
            var rule = MethodMembers()
                .That()
                .AreDeclaredIn(typeof(ClassCallingOtherMethod))
                .Should()
                .NotCallAny(MethodMembers().That().AreDeclaredIn(typeof(Class1)));
            AssertFailsWithErrorMessage(
                rule,
                "System.Void TestAssembly.ClassCallingOtherMethod::CallingOther(TestAssembly.Class1)"
                    + " does call System.String TestAssembly.Class1::AccessClass2(System.Int32)"
            );
        }

        [Fact]
        public void OnlyListForbiddenMethodsFromEnumerableInErrors()
        {
            var methodsInClass1 = MethodMembers()
                .That()
                .AreDeclaredIn(typeof(Class1))
                .GetObjects(Architecture);
            var rule = MethodMembers()
                .That()
                .AreDeclaredIn(typeof(ClassCallingOtherMethod))
                .Should()
                .NotCallAny(methodsInClass1);
            AssertFailsWithErrorMessage(
                rule,
                "System.Void TestAssembly.ClassCallingOtherMethod::CallingOther(TestAssembly.Class1)"
                    + " does call System.String TestAssembly.Class1::AccessClass2(System.Int32)"
            );
        }
    }
}
