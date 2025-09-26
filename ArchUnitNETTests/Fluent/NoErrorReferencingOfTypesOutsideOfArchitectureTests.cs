﻿using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using TestAssembly;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class NoErrorReferencingOfTypesOutsideOfArchitectureTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(System.Reflection.Assembly.Load("TestAssembly"))
            .Build();

        private readonly System.Type _classNotInArchitecture =
            typeof(NoErrorReferencingOfTypesOutsideOfArchitectureTests);

        public NoErrorReferencingOfTypesOutsideOfArchitectureTests()
        {
            Assert.Throws<TypeDoesNotExistInArchitecture>(() =>
                Architecture.GetClassOfType(_classNotInArchitecture)
            );
        }

        private static void AssertNoException(Action action)
        {
            var exception = Record.Exception(action);
            Assert.Null(exception);
        }

        [Fact]
        public void AreBeTest()
        {
            var rule = Classes()
                .That()
                .Are(typeof(EmptyTestClass))
                .Should()
                .Be(_classNotInArchitecture);
            var negation = Classes()
                .That()
                .Are(typeof(EmptyTestClass))
                .Should()
                .NotBe(_classNotInArchitecture);
            Assert.Throws<FailedArchRuleException>(() => rule.Check(Architecture));
            negation.Check(Architecture);

            AssertNoException(() =>
                Classes().That().Are(_classNotInArchitecture).GetObjects(Architecture)
            );
            AssertNoException(() =>
                Classes().That().AreNot(_classNotInArchitecture).GetObjects(Architecture)
            );
        }

        [Fact]
        public void AreAssignableTest()
        {
            var rule = Classes()
                .That()
                .Are(typeof(EmptyTestClass))
                .Should()
                .BeAssignableTo(_classNotInArchitecture);
            var negation = Classes()
                .That()
                .Are(typeof(EmptyTestClass))
                .Should()
                .NotBeAssignableTo(_classNotInArchitecture);
            Assert.Throws<FailedArchRuleException>(() => rule.Check(Architecture));
            negation.Check(Architecture);

            AssertNoException(() =>
                Classes().That().AreAssignableTo(_classNotInArchitecture).GetObjects(Architecture)
            );
            AssertNoException(() =>
                Classes()
                    .That()
                    .AreNotAssignableTo(_classNotInArchitecture)
                    .GetObjects(Architecture)
            );
        }

        [Fact]
        public void ImplementInterfaceTest()
        {
            var rule = Classes()
                .That()
                .Are(typeof(EmptyTestClass))
                .Should()
                .ImplementInterface(_classNotInArchitecture);
            var negation = Classes()
                .That()
                .Are(typeof(EmptyTestClass))
                .Should()
                .NotImplementInterface(_classNotInArchitecture);
            Assert.Throws<FailedArchRuleException>(() => rule.Check(Architecture));
            negation.Check(Architecture);

            AssertNoException(() =>
                Classes()
                    .That()
                    .ImplementInterface(_classNotInArchitecture)
                    .GetObjects(Architecture)
            );
            AssertNoException(() =>
                Classes()
                    .That()
                    .DoNotImplementInterface(_classNotInArchitecture)
                    .GetObjects(Architecture)
            );
        }

        [Fact]
        public void DeclaredInTest()
        {
            var rule = Members().Should().BeDeclaredIn(_classNotInArchitecture);
            var negation = Members().Should().NotBeDeclaredIn(_classNotInArchitecture);
            Assert.Throws<FailedArchRuleException>(() => rule.Check(Architecture));
            negation.Check(Architecture);

            AssertNoException(() =>
                Members().That().AreDeclaredIn(_classNotInArchitecture).GetObjects(Architecture)
            );
            AssertNoException(() =>
                Members().That().AreNotDeclaredIn(_classNotInArchitecture).GetObjects(Architecture)
            );
        }

        [Fact]
        public void CalledByTest()
        {
            var rule = MethodMembers().Should().BeCalledBy(_classNotInArchitecture);
            var negation = MethodMembers().Should().NotBeCalledBy(_classNotInArchitecture);
            Assert.Throws<FailedArchRuleException>(() => rule.Check(Architecture));
            negation.Check(Architecture);

            AssertNoException(() =>
                MethodMembers().That().AreCalledBy(_classNotInArchitecture).GetObjects(Architecture)
            );
            AssertNoException(() =>
                MethodMembers()
                    .That()
                    .AreNotCalledBy(_classNotInArchitecture)
                    .GetObjects(Architecture)
            );
        }
    }
}
