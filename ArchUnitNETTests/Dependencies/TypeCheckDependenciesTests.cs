//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class TypeCheckDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(TypeCheckDependenciesTests).Assembly)
            .Build();

        private readonly Class _classWithTypeDepencency;
        private readonly Class _dependingClass;

        public TypeCheckDependenciesTests()
        {
            _classWithTypeDepencency = Architecture.GetClassOfType(typeof(ClassWithTypeDependency));
            _dependingClass = Architecture.GetClassOfType(typeof(DependingClass));
        }

        [Fact]
        public void TypeCheckDependencyTest()
        {
            var methodMember = _classWithTypeDepencency.Members.First(member =>
                member.NameContains("MethodWithTypeDependency")
            );
            var typeDependencies = _classWithTypeDepencency.GetTypeDependencies().ToList();
            var methodTypeDependencies = methodMember.GetTypeDependencies().ToList();

            Assert.Contains(_dependingClass, typeDependencies);
            Assert.Contains(_dependingClass, methodTypeDependencies);
        }
    }

    internal class ClassWithTypeDependency
    {
        public void MethodWithTypeDependency(object obj)
        {
            if (obj is DependingClass) { }
        }
    }

    internal class DependingClass { }
}
