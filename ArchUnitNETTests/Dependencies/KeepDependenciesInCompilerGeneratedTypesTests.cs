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
    public class KeepDependenciesInCompilerGeneratedTypesTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(KeepDependenciesInCompilerGeneratedTypesTests).Assembly).Build();

        private readonly Class _argumentClass;
        private readonly Class _classWithIndexing;
        private readonly Class _classWithLambda;
        private readonly Class _classWithProperty;
        private readonly Class _genericArgumentClass;
        private readonly Class _returnedClass;

        public KeepDependenciesInCompilerGeneratedTypesTests()
        {
            _classWithProperty = Architecture.GetClassOfType(typeof(ClassWithPropertyDependency));
            _classWithLambda = Architecture.GetClassOfType(typeof(ClassWithLambdaDependency));
            _classWithIndexing = Architecture.GetClassOfType(typeof(ClassWithIndexingDependency));
            _returnedClass = Architecture.GetClassOfType(typeof(ReturnedClass));
            _argumentClass = Architecture.GetClassOfType(typeof(ArgumentClass));
            _genericArgumentClass = Architecture.GetClassOfType(typeof(GenericArgumentClass));
        }

        [Fact]
        public void PropertyDependenciesNotLost()
        {
            var typeDependencies = _classWithProperty.GetTypeDependencies().ToList();
            var property = _classWithProperty.GetPropertyMembers().First();
            var propertyTypeDependencies = property.GetTypeDependencies().ToList();


            Assert.Contains(_returnedClass, typeDependencies);
            Assert.Contains(_argumentClass, typeDependencies);
            Assert.Single(_classWithProperty.GetPropertyMembers());
            Assert.Contains(_returnedClass, propertyTypeDependencies);
            Assert.Contains(_argumentClass, propertyTypeDependencies);
        }

        [Fact]
        public void LambdaMethodCallDependenciesNotLost()
        {
            var typeDependencies = _classWithLambda.GetTypeDependencies().ToList();
            var method = _classWithLambda.GetMethodMembers().First();
            var methodTypeDependencies = method.GetTypeDependencies().ToList();

            Assert.Contains(_returnedClass, typeDependencies);
            Assert.Contains(_argumentClass, typeDependencies);
            Assert.Contains(_classWithIndexing, typeDependencies);
            Assert.Single(_classWithLambda.GetMethodMembers());
            Assert.Contains(_returnedClass, methodTypeDependencies);
            Assert.Contains(_argumentClass, methodTypeDependencies);
            Assert.Contains(_classWithIndexing, methodTypeDependencies);
        }

        [SkipInReleaseBuild]
        public void LambdaTypeDependenciesNotLost()
        {
            var typeDependencies = _classWithLambda.GetTypeDependencies().ToList();
            var method = _classWithLambda.GetMethodMembers().First();
            var methodTypeDependencies = method.GetTypeDependencies().ToList();

            Assert.Contains(_classWithProperty, typeDependencies);
            Assert.Contains(_classWithProperty, methodTypeDependencies);
        }

        [Fact]
        public void LambdaGenericArgumentDependenciesNotLost()
        {
            var typeDependencies = _classWithLambda.GetTypeDependencies().ToList();
            var method = _classWithLambda.GetMethodMembers().First();
            var methodTypeDependencies = method.GetTypeDependencies().ToList();

            Assert.Contains(_genericArgumentClass, typeDependencies);
            Assert.Contains(_genericArgumentClass, methodTypeDependencies);
        }

        [Fact(Skip = "Fails because the string is created with OpCode Ldstr which has no TypeReference as Operand")]
        public void LambdaPrimitiveDependenciesNotLost()
        {
            var typeDependencies = _classWithLambda.GetTypeDependencies().ToList();
            var method = _classWithLambda.GetMethodMembers().First();
            var methodTypeDependencies = method.GetTypeDependencies().ToList();

            Assert.Contains(typeDependencies, dep => dep.FullNameContains("System.String"));
            Assert.Contains(methodTypeDependencies, dep => dep.FullNameContains("System.String"));
        }

        [Fact]
        public void IndexingDependenciesNotLost()
        {
            var typeDependencies = _classWithIndexing.GetTypeDependencies().ToList();

            Assert.Contains(_returnedClass, typeDependencies);
            Assert.Contains(_argumentClass, typeDependencies);
        }

        [Fact]
        public void BackwardDependenciesAssignedCorrectly()
        {
            var argumentTypeBackwardDependencies =
                _argumentClass.BackwardsDependencies.Select(dep => dep.Origin).ToList();
            var returnedTypeBackwardDependencies =
                _returnedClass.BackwardsDependencies.Select(dep => dep.Origin).ToList();

            Assert.Contains(_classWithProperty, argumentTypeBackwardDependencies);
            Assert.Contains(_classWithLambda, argumentTypeBackwardDependencies);
            Assert.Contains(_classWithIndexing, argumentTypeBackwardDependencies);

            Assert.Contains(_classWithProperty, returnedTypeBackwardDependencies);
            Assert.Contains(_classWithLambda, returnedTypeBackwardDependencies);
            Assert.Contains(_classWithIndexing, returnedTypeBackwardDependencies);
        }
    }

    internal class ClassWithPropertyDependency
    {
        public object Property => new ReturnedClass(new ArgumentClass());
    }

    internal class ClassWithLambdaDependency
    {
        private Func<object, object> _lambda;

        public ClassWithLambdaDependency()
        {
            _lambda = argumentClass =>
            {
                Func<object, object> secondLambda = obj => new ClassWithIndexingDependency();
                Func<object, object> thirdLambda = obj => "testString";
                Func<GenericArgumentClass, bool> fourthLambda = genericArgumentClass => true;
                ClassWithPropertyDependency var = null;
                return new ReturnedClass(new ArgumentClass());
            };
        }
    }

    internal class GenericArgumentClass
    {
    }

    internal class ClassWithIndexingDependency
    {
        public object this[int index] => new ReturnedClass(new ArgumentClass());
    }

    internal class ReturnedClass
    {
        public ReturnedClass(object argument)
        {
        }
    }

    internal class ArgumentClass
    {
    }
}