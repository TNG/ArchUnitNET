//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class ArrayTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(ArrayTests).Assembly).Build();

        private readonly Class _bool;

        private readonly Class _classWithArrayMethod;
        private readonly Class _classWithBoolArrayFields;

        private readonly Class _int;


        public ArrayTests()
        {
            _bool = Architecture.GetClassOfType(typeof(bool));
            _int = Architecture.GetClassOfType(typeof(int));
            _classWithBoolArrayFields = Architecture.GetClassOfType(typeof(ClassWithBoolArrayFields));
            _classWithArrayMethod = Architecture.GetClassOfType(typeof(ClassWithArrayMethod));
        }

        [Fact]
        public void FindDependenciesInArrayFields()
        {
            var fieldTypeDependencies = _classWithBoolArrayFields.Dependencies.OfType<FieldTypeDependency>();
            var fieldMembers = _classWithBoolArrayFields.GetFieldMembers();

            Assert.DoesNotContain(fieldMembers, member => !Equals(member.Type, _bool));
            Assert.DoesNotContain(fieldTypeDependencies, dependency => !dependency.Target.Equals(_bool));
        }

        [Fact]
        public void FindDependenciesInArrayMethods()
        {
            var typeDependencies = _classWithArrayMethod.GetTypeDependencies().ToList();
            var method = _classWithArrayMethod.GetMethodMembers().First(member => member.NameContains("ArrayMethod"));

            Assert.Contains(_bool, typeDependencies);
            Assert.Contains(_int, typeDependencies);

            Assert.Equal(_bool, method.ReturnType);
            Assert.Equal(_int, method.Parameters.First());

            Assert.DoesNotContain(method.ParameterInstances, parameter => !parameter.IsArray);
        }

        [Fact]
        public void FindArrayDimensions()
        {
            var bool1Array = _classWithBoolArrayFields.GetFieldMembersWithName("_bool1Array").First();
            var bool11Array = _classWithBoolArrayFields.GetFieldMembersWithName("_bool11Array").First();
            var bool2Array = _classWithBoolArrayFields.GetFieldMembersWithName("_bool2Array").First();
            var bool21Array = _classWithBoolArrayFields.GetFieldMembersWithName("_bool21Array").First();
            var bool412Array = _classWithBoolArrayFields.GetFieldMembersWithName("_bool412Array").First();

            Assert.True(bool1Array.IsArray);
            Assert.True(bool11Array.IsArray);
            Assert.True(bool2Array.IsArray);
            Assert.True(bool21Array.IsArray);
            Assert.True(bool412Array.IsArray);

            Assert.Equal(new[] {1}, bool1Array.ArrayDimensions);
            Assert.Equal(new[] {1, 1}, bool11Array.ArrayDimensions);
            Assert.Equal(new[] {2}, bool2Array.ArrayDimensions);
            Assert.Equal(new[] {2, 1}, bool21Array.ArrayDimensions);
            Assert.Equal(new[] {4, 1, 2}, bool412Array.ArrayDimensions);
        }
    }

#pragma warning disable 169
    internal class ClassWithBoolArrayFields
    {
        private bool[][] _bool11Array;
        private bool[] _bool1Array;
        private bool[,][] _bool21Array;
        private bool[,] _bool2Array;
        private bool[,,,][][,] _bool412Array;
    }

    internal class ClassWithArrayMethod
    {
        public bool[] ArrayMethod(int[,] i)
        {
            return new bool[i[1, 0]];
        }
    }
}
#pragma warning restore 169