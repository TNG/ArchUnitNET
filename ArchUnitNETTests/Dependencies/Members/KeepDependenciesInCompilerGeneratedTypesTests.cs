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

namespace ArchUnitNETTests.Dependencies.Members
{
    public class KeepDependenciesInCompilerGeneratedTypesTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(KeepDependenciesInCompilerGeneratedTypesTests).Assembly).Build();

        private readonly Class _argumentClass;
        private readonly Class _classWithIndexing;
        private readonly Class _classWithLambda;
        private readonly Class _classWithProperty;
        private readonly Class _returnedClass;

        public KeepDependenciesInCompilerGeneratedTypesTests()
        {
            _classWithProperty = Architecture.GetClassOfType(typeof(ClassWithPropertyDependency));
            _classWithLambda = Architecture.GetClassOfType(typeof(ClassWithLambdaDependency));
            _classWithIndexing = Architecture.GetClassOfType(typeof(ClassWithIndexingDependency));
            _returnedClass = Architecture.GetClassOfType(typeof(ReturnedClass));
            _argumentClass = Architecture.GetClassOfType(typeof(ArgumentClass));
        }

        [Fact]
        public void PropertyDependenciesNotLost()
        {
            var typeDependencies = _classWithProperty.GetTypeDependencies().ToList();
            var property = _classWithProperty.GetPropertyMembers().First();
            var propertyTypeDependencies = property.GetTypeDependencies(Architecture).ToList();


            Assert.Contains(_returnedClass, typeDependencies);
            Assert.Contains(_argumentClass, typeDependencies);
            Assert.Single(_classWithProperty.GetPropertyMembers());
            Assert.Contains(_returnedClass, propertyTypeDependencies);
            Assert.Contains(_argumentClass, propertyTypeDependencies);
        }

        [Fact]
        public void LambdaDependenciesNotLost()
        {
            var typeDependencies = _classWithLambda.GetTypeDependencies().ToList();
            var method = _classWithLambda.GetMethodMembers().First();
            var methodTypeDependencies = method.GetTypeDependencies(Architecture).ToList();

            Assert.Contains(_returnedClass, typeDependencies);
            Assert.Contains(_argumentClass, typeDependencies);
            Assert.Single(_classWithProperty.GetMethodMembers());
            Assert.Contains(_returnedClass, methodTypeDependencies);
            Assert.Contains(_argumentClass, methodTypeDependencies);
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
            _lambda = argumentClass => new ReturnedClass(argumentClass);
        }
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