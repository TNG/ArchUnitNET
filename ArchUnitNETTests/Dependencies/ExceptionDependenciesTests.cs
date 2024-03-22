//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class ExceptionDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(ExceptionDependenciesTests).Assembly)
            .Build();

        private readonly Class _classWithException;
        private readonly Class _throwingClass;

        public ExceptionDependenciesTests()
        {
            _classWithException = Architecture.GetClassOfType(typeof(ClassWithException));
            _throwingClass = Architecture.GetClassOfType(typeof(ThrowingClass));
        }

        [Fact]
        public void ThrowDependencyFound()
        {
            var typeDependencies = _throwingClass.GetTypeDependencies().ToList();
            var method = _throwingClass
                .GetMethodMembers()
                .First(member => member.FullNameContains("ThrowingMethod"));
            var methodTypeDependencies = method.GetTypeDependencies().ToList();

            Assert.Contains(_classWithException, typeDependencies);
            Assert.Contains(_classWithException, methodTypeDependencies);
        }
    }

    internal static class ClassWithException
    {
        public static readonly Exception Exception = new Exception();
    }

    internal class ThrowingClass
    {
        public void ThrowingMethod()
        {
            throw ClassWithException.Exception;
        }
    }
}
