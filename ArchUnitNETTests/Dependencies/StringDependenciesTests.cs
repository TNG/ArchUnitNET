﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Dependencies.Members;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class StringDependenciesTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(KeepDependenciesInCompilerGeneratedTypesTests).Assembly).Build();

        private readonly Class _classWithConstString;
        private readonly Class _classWithLocalString;
        private readonly Class _classWithPropertyString;

        public StringDependenciesTests()
        {
            _classWithConstString = Architecture.GetClassOfType(typeof(ClassWithConstString));
            _classWithLocalString = Architecture.GetClassOfType(typeof(ClassWithLocalString));
            _classWithPropertyString = Architecture.GetClassOfType(typeof(ClassWithPropertyString));
        }

        [Fact]
        public void ConstStringDependencyFound()
        {
            var typeDependencies = _classWithConstString.GetTypeDependencies().ToList();
            Assert.Contains(typeof(string).FullName, typeDependencies.Select(dep => dep.FullName));
        }

        [Fact]
        public void LocalStringDependencyFound()
        {
            var typeDependencies = _classWithLocalString.GetTypeDependencies().ToList();
            var method = _classWithLocalString.GetMethodMembers().First();
            var methodTypeDependencies = method.GetTypeDependencies().ToList();

            Assert.Contains(typeof(string).FullName, typeDependencies.Select(dep => dep.FullName));
            Assert.Contains(typeof(string).FullName, methodTypeDependencies.Select(dep => dep.FullName));
        }

        [Fact]
        public void PropertyStringDependencyFound()
        {
            var typeDependencies = _classWithPropertyString.GetTypeDependencies().ToList();
            var property = _classWithPropertyString.GetMethodMembers().First();
            var propertyTypeDependencies = property.GetTypeDependencies().ToList();

            Assert.Contains(typeof(string).FullName, typeDependencies.Select(dep => dep.FullName));
            Assert.Contains(typeof(string).FullName, propertyTypeDependencies.Select(dep => dep.FullName));
        }
    }

    internal class ClassWithConstString
    {
        private const string Str = "ConstantString";
    }

    internal class ClassWithLocalString
    {
        public ClassWithLocalString()
        {
            var str = "LocalString";
        }
    }

    internal class ClassWithPropertyString
    {
        public object Prop => "PropertyString";
    }
}