//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//  Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.ArchitectureTests
{
    public class ArchUnitArchitectureTests
    {
        private static readonly string DomainNamespace = typeof(Architecture).Namespace;
        private static readonly string LoaderNamespace = typeof(ArchLoader).Namespace;
        private static readonly string FluentNamespace = typeof(IArchRule).Namespace;

        private readonly Architecture _architecture = new ArchLoader()
            .LoadAssembly(typeof(Architecture).Assembly)
            .Build();

        [Fact]
        public void DomainHasNoDependencyOnFluent()
        {
            Types()
                .That()
                .ResideInNamespace(DomainNamespace)
                .Should()
                .NotDependOnAny(Types().That().ResideInNamespace(FluentNamespace))
                .Check(_architecture);
        }

        [Fact]
        public void DomainHasNoDependencyOnLoader()
        {
            Types()
                .That()
                .ResideInNamespace(DomainNamespace)
                .Should()
                .NotDependOnAny(Types().That().ResideInNamespace(LoaderNamespace))
                .Check(_architecture);
        }

        [Fact]
        public void LoaderHasNoDependencyOnFluent()
        {
            Types()
                .That()
                .ResideInNamespace(LoaderNamespace)
                .Should()
                .NotDependOnAny(Types().That().ResideInNamespace(FluentNamespace))
                .Check(_architecture);
        }

        [Fact]
        public void FluentHasNoDependencyOnLoader()
        {
            Types()
                .That()
                .ResideInNamespace(FluentNamespace)
                .Should()
                .NotDependOnAny(Types().That().ResideInNamespace(LoaderNamespace))
                .Check(_architecture);
        }
    }
}
