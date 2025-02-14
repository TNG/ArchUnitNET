using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using TestAssembly.DependencyTargets;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent.Syntax
{
    public class DependenciesToOtherAssembliesTests
    {
        public class TestDependencyIssue
        {
            private static readonly Architecture Architecture = new ArchLoader()
                .LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNETTests"))
                .Build();

            [Fact]
            public void DependOnAnyTypesThatTest()
            {
                Types()
                    .That()
                    .Are(typeof(DependingClass))
                    .Should()
                    .DependOnAnyTypesThat()
                    .Are(typeof(ClassInTestAssembly))
                    .Check(Architecture);
                Assert.Throws<FailedArchRuleException>(
                    () =>
                        Types()
                            .That()
                            .Are(typeof(DependingClass))
                            .Should()
                            .NotDependOnAnyTypesThat()
                            .Are(typeof(ClassInTestAssembly))
                            .Check(Architecture)
                );
            }

            [Fact]
            public void BeAssignableToTypesThatTest()
            {
                Types()
                    .That()
                    .Are(typeof(DependingClass))
                    .Should()
                    .BeAssignableToTypesThat()
                    .Are(typeof(IInterfaceInTestAssembly))
                    .Check(Architecture);
                Assert.Throws<FailedArchRuleException>(
                    () =>
                        Types()
                            .That()
                            .Are(typeof(DependingClass))
                            .Should()
                            .NotBeAssignableToTypesThat()
                            .Are(typeof(IInterfaceInTestAssembly))
                            .Check(Architecture)
                );
            }

            [Fact]
            public void OnlyDependOnTypesThatTest()
            {
                Types()
                    .That()
                    .Are(typeof(DependingClass))
                    .Should()
                    .OnlyDependOnTypesThat()
                    .Are(
                        typeof(object),
                        typeof(AttributeInTestAssembly),
                        typeof(IInterfaceInTestAssembly),
                        typeof(ClassInTestAssembly),
                        typeof(DependingClass)
                    )
                    .Check(Architecture);
            }

            [Fact]
            public void HaveAnyAttributesThatTest()
            {
                Types()
                    .That()
                    .Are(typeof(DependingClass))
                    .Should()
                    .HaveAnyAttributesThat()
                    .Are(typeof(AttributeInTestAssembly))
                    .Check(Architecture);
                Assert.Throws<FailedArchRuleException>(
                    () =>
                        Types()
                            .That()
                            .Are(typeof(DependingClass))
                            .Should()
                            .NotHaveAnyAttributesThat()
                            .Are(typeof(AttributeInTestAssembly))
                            .Check(Architecture)
                );
            }

            [Fact]
            public void OnlyHaveAttributesThatTest()
            {
                Types()
                    .That()
                    .Are(typeof(DependingClass))
                    .Should()
                    .OnlyHaveAttributesThat()
                    .Are(typeof(AttributeInTestAssembly))
                    .Check(Architecture);
            }

            [Fact]
            public void IncludeReferencedTypesTest()
            {
                var classInTestAssembly = Architecture.GetClassOfType(typeof(ClassInTestAssembly));
                Assert.Contains(classInTestAssembly, Types(true).GetObjects(Architecture));
                Assert.Contains(classInTestAssembly, Classes(true).GetObjects(Architecture));

                var attributeInTestAssembly = Architecture.GetAttributeOfType(
                    typeof(AttributeInTestAssembly)
                );
                Assert.Contains(attributeInTestAssembly, Types(true).GetObjects(Architecture));
                Assert.Contains(attributeInTestAssembly, Attributes(true).GetObjects(Architecture));

                var interfaceInTestAssembly = Architecture.GetInterfaceOfType(
                    typeof(IInterfaceInTestAssembly)
                );
                Assert.Contains(interfaceInTestAssembly, Types(true).GetObjects(Architecture));
                Assert.Contains(interfaceInTestAssembly, Interfaces(true).GetObjects(Architecture));
            }
        }
    }

    [AttributeInTestAssembly]
    public class DependingClass : IInterfaceInTestAssembly
    {
        private ClassInTestAssembly dep;

        public DependingClass()
        {
            dep = new ClassInTestAssembly();
            dep.MethodInTestAssembly();
        }
    }
}
