//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using System.Collections.Generic;
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
        private readonly Class _classAccessingStaticMethodInGenerator;
        private readonly Class _classAccessingStaticMethodWithoutGenerator;
        private readonly Class _classWithStaticMethod;
        private readonly MethodMember _staticMethodMemberWithBody;
        private readonly FieldMember _staticMethodMemberAsField;


        public DependenciesToStaticMethodsTests()
        {
            _accessingClass = Architecture.GetClassOfType(typeof(ClassAccessingStaticMethod));
            _classAccessingStaticMethodInGenerator =
                Architecture.GetClassOfType(typeof(ClassAccessingStaticMethodInGenerator));
            _classAccessingStaticMethodWithoutGenerator =
                Architecture.GetClassOfType(typeof(ClassAccessingStaticMethodWithoutGenerator));
            _classWithStaticMethod = Architecture.GetClassOfType(typeof(ClassWithStaticMethods));
            _staticMethodMemberWithBody = _classWithStaticMethod
                .GetMethodMembersWithName(nameof(ClassWithStaticMethods.StaticMethodWithBody) + "(System.Int32)").First();
            _staticMethodMemberAsField = _classWithStaticMethod
                .GetFieldMembersWithName(nameof(ClassWithStaticMethods.StaticMethodAsField)).First();
        }

        [Fact]
        public void StaticMethodCallFound()
        {
            var typeDependencies = _accessingClass.GetTypeDependencies().ToList();
            var methodDependencies = _accessingClass.GetCalledMethods().ToList();

            Assert.Contains(_classWithStaticMethod, typeDependencies);
            Assert.Contains(_staticMethodMemberWithBody, methodDependencies);
        }

        [Fact]
        public void StaticMethodCallToFieldFound()
        {
            var typeDependencies = _accessingClass.GetTypeDependencies().ToList();
            var fieldDependencies = _accessingClass.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticMethod, typeDependencies);
            Assert.Contains(_staticMethodMemberAsField, fieldDependencies);
        }

        [Fact]
        public void StaticReferencesInGeneratorFound()
        {
            var typeDependencies = _classAccessingStaticMethodInGenerator.GetTypeDependencies().ToList();
            var methodDependencies = _classAccessingStaticMethodInGenerator.GetCalledMethods().ToList();
            var fieldDependencies = _classAccessingStaticMethodInGenerator.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticMethod, typeDependencies);
            Assert.Contains(_staticMethodMemberWithBody, methodDependencies);
            Assert.Contains(_staticMethodMemberAsField, fieldDependencies);
        }
        
        [Fact]
        public void StaticReferencesWithoutGeneratorFound()
        {
            var typeDependencies = _classAccessingStaticMethodWithoutGenerator.GetTypeDependencies().ToList();
            var methodDependencies = _classAccessingStaticMethodWithoutGenerator.GetCalledMethods().ToList();
            var fieldDependencies = _classAccessingStaticMethodWithoutGenerator.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticMethod, typeDependencies);
            Assert.Contains(_staticMethodMemberWithBody, methodDependencies);
            Assert.Contains(_staticMethodMemberAsField, fieldDependencies);
        }
    }

    internal static class ClassWithStaticMethods
    {
        public static int StaticMethodWithBody(int a)
        {
            return a + 1;
        }

        public static Func<int, int> StaticMethodAsField = i => i + 2;
    }


    internal class ClassAccessingStaticMethod
    {
        private int _a;
        private int _b;

        public ClassAccessingStaticMethod(int a)
        {
            _a = ClassWithStaticMethods.StaticMethodWithBody(a);
            _b = ClassWithStaticMethods.StaticMethodAsField(a);
        }
    }

    internal class ClassAccessingStaticMethodInGenerator
    {
        public IEnumerable<int> Generator(bool useField)
        {
            var generatingFunction = useField
                ? ClassWithStaticMethods.StaticMethodAsField
                : ClassWithStaticMethods.StaticMethodWithBody;

            var a = 0;

            for (var i = 0; i < 10; i++)
            {
                yield return a;
                a = generatingFunction(a);
            }
        }
    }

    internal class ClassAccessingStaticMethodWithoutGenerator
    {
        public IEnumerable<int> WithoutGenerator(bool useField)
        {
            var results = new List<int>();
            var generatingFunction = useField
                ? ClassWithStaticMethods.StaticMethodAsField
                : ClassWithStaticMethods.StaticMethodWithBody;

            var a = 0;

            for (var i = 0; i < 10; i++)
            {
                results[i] = a;
                a = generatingFunction(a);
            }

            return results;
        }
    }
}