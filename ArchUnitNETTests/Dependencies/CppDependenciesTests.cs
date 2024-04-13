//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class CppDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembliesRecursively(
                new[] { typeof(CppExampleClassUserNamespace.CppExampleClassUser).Assembly },
                filterFunc => FilterResult.LoadAndContinue
            )
            .Build();

        [Fact]
        public void CppClassUserFound()
        {
            var exampleCppUser = Architecture.GetClassOfType(
                typeof(CppExampleClassUserNamespace.CastClassA)
            );
            Assert.Contains(exampleCppUser, Architecture.Classes);
        }
    }
}
