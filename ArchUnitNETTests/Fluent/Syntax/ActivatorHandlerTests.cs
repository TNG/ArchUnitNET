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
        public void CreateComplexSyntaxElementWithInvalidParametersThrowsExceptionTest()
        {
            Assert.Throws<MissingMethodException>(() =>
                CreateSyntaxElement<ClassesShouldThat<ClassesShouldConjunction, Class>,
                    ClassesShouldConjunction, Class, Class>(
                    new ArchRuleCreator<Class>(architecture => architecture.Classes),
                    architecture => architecture.Classes, (cls1, cls2) => true));
        }

        [Fact]
        public void CreateMembersShouldThatSyntaxElementTest()
        {
            var syntaxElement =
                CreateSyntaxElement<MembersShouldThat<MembersShouldConjunction, IMember, Class>,
                    MembersShouldConjunction, IMember, Class>(
                    new ArchRuleCreator<Class>(architecture => architecture.Classes),
                    architecture => architecture.Members, (member, cls) => true);
            Assert.NotNull(syntaxElement);
        }

        [Fact]
        public void CreateSimpleSyntaxElementTest()
        {
            var syntaxElement =
                CreateSyntaxElement<TypesShouldConjunction, IType>(
                    new ArchRuleCreator<IType>(architecture => architecture.Types));
            Assert.NotNull(syntaxElement);
        }

        [Fact]
        public void CreateSimpleSyntaxElementWithInvalidParametersThrowsExceptionTest()
        {
            Assert.Throws<MissingMethodException>(() =>
                CreateSyntaxElement<ClassesShouldThat<ClassesShouldConjunction, Class>, Class>(
                    new ArchRuleCreator<Class>(architecture => architecture.Classes)));
        }

        [Fact]
        public void CreateTypesShouldThatSyntaxElementTest()
        {
            var syntaxElement =
                CreateSyntaxElement<TypesShouldThat<TypesShouldConjunction, IType, IType>,
                    TypesShouldConjunction, IType, IType>(
                    new ArchRuleCreator<IType>(architecture => architecture.Types),
                    architecture => architecture.Types, (type1, type2) => true);
            Assert.NotNull(syntaxElement);
        }
    }
}