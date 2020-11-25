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
using ArchUnitNETTests.Dependencies;
using Xunit;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class KeepDependenciesInAnonymousTypesTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(KeepDependenciesInCompilerGeneratedTypesTests).Assembly).Build();

        private readonly Class _castType;
        private readonly Class _class1WithAnonymous;
        private readonly Class _class2WithAnonymous;
        private readonly Class _instantiatedType;


        public KeepDependenciesInAnonymousTypesTests()
        {
            _class1WithAnonymous = Architecture.GetClassOfType(typeof(Class1WithAnonymousType));
            _class2WithAnonymous = Architecture.GetClassOfType(typeof(Class2WithAnonymousType));
            _instantiatedType = Architecture.GetClassOfType(typeof(InstantiatedType));
            _castType = Architecture.GetClassOfType(typeof(CastType));
        }

        [Fact]
        public void DependenciesInAnonymousTypesNotLost()
        {
            var typeDependencies = _class1WithAnonymous.GetTypeDependencies().ToList();
            var ctor = _class1WithAnonymous.GetMethodMembers().First();
            var ctorTypeDependencies = ctor.GetTypeDependencies(Architecture).ToList();

            Assert.Contains(_instantiatedType, typeDependencies);
            Assert.Single(_class1WithAnonymous.GetMethodMembers());
            Assert.Contains(_instantiatedType, ctorTypeDependencies);
        }

        [Fact]
        public void CastDependenciesInAnonymousTypesNotLost()
        {
            var typeDependencies = _class2WithAnonymous.GetTypeDependencies().ToList();
            var ctor = _class2WithAnonymous.GetMethodMembers().First();
            var ctorTypeDependencies = ctor.GetTypeDependencies(Architecture).ToList();

            Assert.Contains(_instantiatedType, typeDependencies);
            Assert.Contains(_castType, typeDependencies);
            Assert.Single(_class2WithAnonymous.GetMethodMembers());
            Assert.Contains(_instantiatedType, ctorTypeDependencies);
            Assert.Contains(_castType, ctorTypeDependencies);
        }

        [Fact]
        public void BackwardDependenciesAssignedCorrectly()
        {
            var instantiatedTypeBackwardDependencies =
                _instantiatedType.BackwardsDependencies.Select(dep => dep.Origin).ToList();
            var castTypeBackwardDependencies =
                _castType.BackwardsDependencies.Select(dep => dep.Origin).ToList();

            Assert.Contains(_class1WithAnonymous, instantiatedTypeBackwardDependencies);
            Assert.Contains(_class2WithAnonymous, instantiatedTypeBackwardDependencies);

            Assert.Contains(_class2WithAnonymous, castTypeBackwardDependencies);
        }
    }

    internal class Class1WithAnonymousType
    {
        public Class1WithAnonymousType()
        {
            var anonymousType = new {referencedType = new InstantiatedType()};
        }
    }

    internal class Class2WithAnonymousType
    {
        public Class2WithAnonymousType()
        {
            var anonymousType = new {castType = (CastType) new InstantiatedType(), i = 3};
        }
    }

    internal class InstantiatedType
    {
    }

    internal class CastType : InstantiatedType
    {
    }
}