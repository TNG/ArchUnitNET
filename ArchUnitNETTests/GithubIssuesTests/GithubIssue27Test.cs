//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using ArchUnitNET.Fluent;
using Newtonsoft.Json;
using TestAssembly.GithubIssuesTests;
using Xunit;

namespace ArchUnitNETTests.GithubIssuesTests
{
    public class GithubIssue27Test
    {
        [Fact]
        public void Issue16Test()
        {
            var rule1 = ArchRuleDefinition.Classes().That().AreNot(typeof(ClassDependingOnJsonConvert)).Should()
                .NotDependOnAny(typeof(JsonConvert));
            rule1.Check(StaticTestArchitectures.ArchUnitNETTestAssemblyArchitecture);
        }
    }
}