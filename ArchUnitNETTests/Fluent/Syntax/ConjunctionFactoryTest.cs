//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using Xunit;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using static ArchUnitNET.Fluent.BasicObjectProviderDefinition;

namespace ArchUnitNETTests.Fluent.Syntax
{
    public class ConjunctionFactoryTest
    {
        [Fact]
        public void CreateSyntaxElementTest()
        {
            var syntaxElement =
                Create<TypesShouldConjunction, IType>(new ArchRuleCreator<IType>(Types));
            Assert.NotNull(syntaxElement);
            Assert.Equal(typeof(TypesShouldConjunction), syntaxElement.GetType());
        }

        [Fact]
        public void CreateSyntaxElementWithInvalidParametersThrowsExceptionTest()
        {
            Assert.Throws<MissingMethodException>(() =>
                Create<ConjunctionFactoryTest, IType>(new ArchRuleCreator<IType>(Types)));
        }
    }
}