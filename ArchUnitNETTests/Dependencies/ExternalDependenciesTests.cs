//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using TestAssembly;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Dependencies
{
    public class ExternalDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(ExternalDependenciesTests).Assembly)
            .Build();

        private static readonly Architecture ArchitectureBothAssemblies = new ArchLoader()
            .LoadAssemblies(
                System.Reflection.Assembly.Load("ArchUnitNETTests"),
                System.Reflection.Assembly.Load("TestAssembly")
            )
            .Build();

        private static readonly Architecture ArchitectureExternalDependency = new ArchLoader()
            .LoadAssemblyIncludingDependencies(typeof(ExternalDependenciesTests).Assembly)
            .Build();

        [Fact]
        public void PropertyDependencyTest()
        {
            var notDependOnAnyRuleClass = Classes()
                .That()
                .HaveFullName(typeof(PropertyDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class2));
            var notDependOnAnyRuleString = Classes()
                .That()
                .HaveFullName(typeof(PropertyDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class2).FullName);
            Assert.False(notDependOnAnyRuleClass.HasNoViolations(Architecture)); //Class2 does not exist in Architecture
            Assert.False(notDependOnAnyRuleString.HasNoViolations(Architecture));
            Assert.False(notDependOnAnyRuleClass.HasNoViolations(ArchitectureExternalDependency));
        }

        [Fact]
        public void MethodBodyDependencyTest()
        {
            var notDependOnAnyRuleClass = Classes()
                .That()
                .HaveFullName(typeof(MethodBodyDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class3));
            var notDependOnAnyRuleString = Classes()
                .That()
                .HaveFullName(typeof(MethodBodyDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class3).FullName);
            Assert.False(notDependOnAnyRuleClass.HasNoViolations(Architecture)); //Class3 does not exist in Architecture
            Assert.False(notDependOnAnyRuleString.HasNoViolations(Architecture));
            Assert.False(notDependOnAnyRuleClass.HasNoViolations(ArchitectureBothAssemblies));
        }

        [Fact]
        public void MethodArgumentDependencyTest()
        {
            var notDependOnAnyRuleClass = Classes()
                .That()
                .HaveFullName(typeof(MethodArgumentDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class2));
            var notDependOnAnyRuleString = Classes()
                .That()
                .HaveFullName(typeof(MethodArgumentDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class2).FullName);
            Assert.False(notDependOnAnyRuleClass.HasNoViolations(Architecture)); //Class3 does not exist in Architecture
            Assert.False(notDependOnAnyRuleString.HasNoViolations(Architecture));
        }

        [Fact]
        public void FieldDependencyTest()
        {
            var notDependOnAnyRuleClass = Classes()
                .That()
                .HaveFullName(typeof(FieldDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class2));
            var notDependOnAnyRuleString = Classes()
                .That()
                .HaveFullName(typeof(FieldDependency).FullName)
                .Should()
                .NotDependOnAny(typeof(Class2).FullName);
            Assert.False(notDependOnAnyRuleClass.HasNoViolations(Architecture)); //Class3 does not exist in Architecture
            Assert.False(notDependOnAnyRuleString.HasNoViolations(Architecture));
        }
    }

#pragma warning disable 219
    public class PropertyDependency
    {
        public string Prop
        {
            get
            {
                var b = true;
                var a = new Class2();
                return "";
            }
        }
    }

    public class MethodBodyDependency
    {
        public void Method()
        {
            Class3.Class3StaticMethod();
        }
    }

    public class MethodArgumentDependency
    {
        public void Method(Class2 class2) { }
    }

    public class FieldDependency
    {
        private Class2 _class2 = new Class2();
    }
}
#pragma warning restore 219
