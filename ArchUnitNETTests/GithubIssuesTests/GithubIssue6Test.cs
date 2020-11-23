//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.GithubIssuesTests
{
    public class GithubIssue6Test
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(typeof(ClassDependingOnStaticExtensionMethod).Assembly).Build();

        private readonly Class _dependantClass;
        private readonly Class _extensionClass;
        private readonly MethodMember _extensionMethod;

        public GithubIssue6Test()
        {
            _dependantClass = Architecture.GetClassOfType(typeof(ClassDependingOnStaticExtensionMethod));
            _extensionClass = Architecture.GetClassOfType(typeof(StaticExtensionClass));
            _extensionMethod = _extensionClass.GetMethodMembers()
                .First(member => member.FullNameContains("ExtensionMethod"));
        }

        [Fact]
        public void Issue6Test()
        {
            var typeDependencies = _dependantClass.GetTypeDependencies().ToList();
            var calledMethods = _dependantClass.GetCalledMethods().ToList();

            Assert.Contains(_extensionClass, typeDependencies);
            Assert.Contains(_extensionMethod, calledMethods);
        }
    }

    internal class ClassDependingOnStaticExtensionMethod
    {
        public bool MethodWithDependency()
        {
            var list = new List<string>();
            return list.ExtensionMethod();
        }
    }

    internal static class StaticExtensionClass
    {
        public static bool ExtensionMethod(this List<string> list)
        {
            return list.Contains("test");
        }
    }
}