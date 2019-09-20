﻿using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using Xunit;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;
using static ArchUnitNET.Fluent.ObjectProviderDefinition;

namespace ArchUnitNETTests.Fluent.Syntax
{
    public class ActivatorHandlerTests
    {
        [Fact]
        public void CreateSyntaxElementTest()
        {
            var syntaxElement =
                CreateSyntaxElement<TypesShouldConjunction, IType>(new ArchRuleCreator<IType>(Types));
            Assert.NotNull(syntaxElement);
        }

        [Fact]
        public void CreateSyntaxElementWithInvalidParametersThrowsExceptionTest()
        {
            Assert.Throws<MissingMethodException>(() =>
                CreateSyntaxElement<ActivatorHandlerTests, IType>(new ArchRuleCreator<IType>(Types)));
        }
    }
}