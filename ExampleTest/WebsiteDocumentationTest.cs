//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//  
// 	SPDX-License-Identifier: Apache-2.0

// ReSharper disable InconsistentNaming
// ReSharper disable SuggestVarOrType_SimpleTypes


using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Model;
using View;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Slices.SliceRuleDefinition;


namespace ExampleTest
{
    public class WebsiteDocumentationTest
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(WebsiteDocumentationTest).Assembly).Build();

        [Fact]
        public void NamespaceDependency()
        {
            IArchRule rule = Types().That().ResideInNamespace("Model").Should()
                .NotDependOnAny(Types().That().ResideInNamespace("Controller"));
            rule.Check(Architecture);
        }

        [Fact]
        public void ClassDependency()
        {
            IArchRule rule = Classes().That().AreAssignableTo(typeof(ICar)).Should()
                .NotDependOnAny(Classes().That().AreAssignableTo(typeof(ICanvas)));
            Assert.False(rule.HasNoViolations(Architecture));
            //rule.Check(Architecture);
        }

        [Fact]
        public void InheritanceNaming()
        {
            IArchRule rule = Classes().That().AreAssignableTo(typeof(ICar)).Should()
                .HaveNameContaining("Car");
            Assert.False(rule.HasNoViolations(Architecture));
            //rule.Check(Architecture);
        }

        [Fact]
        public void ClassNamespaceContainment()
        {
            IArchRule rule = Classes().That().HaveNameContaining("Canvas").Should()
                .ResideInNamespace(typeof(ICanvas).Namespace);
            Assert.False(rule.HasNoViolations(Architecture));
            //rule.Check(Architecture);
        }

        [Fact]
        public void AttributeAccess()
        {
            IArchRule rule = Classes().That().DoNotHaveAnyAttributes(typeof(Display)).Should()
                .NotDependOnAny(Classes().That().AreAssignableTo(typeof(ICanvas)));
            //Assert.False(rule.HasNoViolations(Architecture));
            rule.Check(Architecture);
        }

        [Fact]
        public void Cycles()
        {
            IArchRule rule = Slices().Matching("Module.(*)").Should().BeFreeOfCycles();
            //Assert.False(rule.HasNoViolations(Architecture));
            rule.Check(Architecture);
        }
    }
}