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
    public class DependenciesToStaticMethodsTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(DependenciesToStaticMethodsTests).Assembly).Build();

        private readonly Class _accessingClass;
        private readonly Class _classWithStaticMethod;
        private readonly MethodMember _staticMethodMember;


        public DependenciesToStaticMethodsTests()
        {
            _accessingClass = Architecture.GetClassOfType(typeof(ClassAccessingStaticMethod));
            _classWithStaticMethod = Architecture.GetClassOfType(typeof(ClassWithStaticMethod));
            _staticMethodMember = _classWithStaticMethod.GetMethodMembersWithName("StaticMethod(System.Int32)").First();
        }

        [Fact]
        public void StaticMethodCallFound()
        {
            var typeDependencies = _accessingClass.GetTypeDependencies().ToList();
            var methodDependencies = _accessingClass.GetCalledMethods().ToList();

            Assert.Contains(_classWithStaticMethod, typeDependencies);
            Assert.Contains(_staticMethodMember, methodDependencies);
        }
    }

    internal static class ClassWithStaticMethod
    {
        public static int StaticMethod(int a)
        {
            return a + 1;
        }
    }


    internal class ClassAccessingStaticMethod
    {
        private int _a;

        public ClassAccessingStaticMethod(int a)
        {
            _a = ClassWithStaticMethod.StaticMethod(a);
        }
    }
}