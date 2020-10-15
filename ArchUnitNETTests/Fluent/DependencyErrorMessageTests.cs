//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Fluent
{
    public class DependencyErrorMessageTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(DependencyErrorMessageTests).Assembly).Build();

        [Fact]
        public void ErrorMessageType()
        {
            var rule = Classes().That()
                .HaveFullNameMatching(typeof(DependencyErrorMessageTestClass).FullName)
                .Should().NotDependOnAny(typeof(ErrorMessageClass1), typeof(ErrorMessageClass2));
            var failDescription = rule.Evaluate(Architecture).ToList().First().Description;
            Assert.DoesNotContain(typeof(string).FullName, failDescription);
            Assert.DoesNotContain(typeof(ErrorMessageClass2).FullName, failDescription);
            Assert.Contains(typeof(ErrorMessageClass1).FullName, failDescription);
        }

        [Fact]
        public void ErrorMessageIType()
        {
            var dependencyClass = Classes().That().HaveFullNameMatching(typeof(ErrorMessageClass1).FullName).Or().HaveFullNameMatching(typeof(ErrorMessageClass2).FullName)
                .GetObjects(Architecture).ToList();
            var rule = Classes().That()
                .HaveFullNameMatching(typeof(DependencyErrorMessageTestClass).FullName)
                .Should().NotDependOnAny(dependencyClass);
            var failDescription = rule.Evaluate(Architecture).ToList().First().Description;
            Assert.DoesNotContain(typeof(string).FullName, failDescription);
            Assert.DoesNotContain(typeof(ErrorMessageClass2).FullName, failDescription);
            Assert.Contains(typeof(ErrorMessageClass1).FullName, failDescription);
        }

        [Fact]
        public void ErrorMessageIObjectProvider()
        {
            var dependencyClass = Classes().That().HaveFullNameMatching(typeof(ErrorMessageClass1).FullName).Or()
                .HaveFullNameMatching(typeof(ErrorMessageClass2).FullName);
            var rule = Classes().That()
                .HaveFullNameMatching(typeof(DependencyErrorMessageTestClass).FullName)
                .Should().NotDependOnAny(dependencyClass);
            var failDescription = rule.Evaluate(Architecture).ToList().First().Description;
            Assert.DoesNotContain(typeof(string).FullName, failDescription);
            Assert.DoesNotContain(typeof(ErrorMessageClass2).FullName, failDescription);
            Assert.Contains(typeof(ErrorMessageClass1).FullName, failDescription);
        }
    }

    public class DependencyErrorMessageTestClass
    {
        ErrorMessageClass1 testData = new ErrorMessageClass1();
        private string testString = "";
    }

    public class ErrorMessageClass1
    {
        
    }

    public class ErrorMessageClass2
    {
        
    }
}
