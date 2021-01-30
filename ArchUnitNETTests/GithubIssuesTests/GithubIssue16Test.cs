//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.GithubIssuesTests
{
    public class GithubIssue16Test
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(typeof(Architecture).Assembly).Build();

        private static readonly IObjectProvider<IType> CoreLayer = ArchRuleDefinition.Types().That()
            .ResideInNamespace("ArchUnitNET.Core").As("ArchUnitNET.Core");

        private static readonly IObjectProvider<IType> DomainLayer = ArchRuleDefinition.Types().That()
            .ResideInNamespace("ArchUnitNET.Domain").As("ArchUnitNET.Domain");

        private static readonly IObjectProvider<IType> FluentLayer = ArchRuleDefinition.Types().That()
            .ResideInNamespace("ArchUnitNET.Fluent").As("ArchUnitNET.Fluent");

        [Fact]
        public void Issue16Test()
        {
            var rule1 = ArchRuleDefinition.Types()
                .That().Are(FluentLayer)
                .Should().OnlyDependOn(ArchRuleDefinition.Types().That().ResideInNamespace("System").Or()
                    .ResideInNamespace("ArchUnitNET.Core").Or().ResideInNamespace("ArchUnitNET.Domain").Or()
                    .ResideInNamespace("ArchUnitNET.Fluent"));
            rule1.Check(Architecture);
        }
    }
}