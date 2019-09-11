using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Syntax.Elements.Members;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using Xunit;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNETTests.Fluent.Syntax
{
    public class ActivatorHandlerTests
    {
        [Fact]
        public void CreateSyntaxElementTest()
        {
            var syntaxElement =
                CreateSyntaxElement<TypesShouldConjunction, IType>(
                    new ArchRuleCreator<IType>(architecture => architecture.Types));
            Assert.NotNull(syntaxElement);
        }

        [Fact]
        public void CreateSyntaxElementWithInvalidParametersThrowsExceptionTest()
        {
            Assert.Throws<MissingMethodException>(() =>
                CreateSyntaxElement<ClassesShouldThat<ClassesShouldConjunction, Class>, Class>(
                    new ArchRuleCreator<Class>(architecture => architecture.Classes)));
        }
    }
}