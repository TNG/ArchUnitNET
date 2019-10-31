//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent.Extensions;
using Xunit;

// ReSharper disable NotAccessedField.Local
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable UnusedMember.Global

namespace ArchUnitNETTests.Dependencies.Members
{
    public class MethodSignatureMemberDependencyTests
    {
        [Theory]
        [ClassData(typeof(MethodDependencyTestBuild.MethodSignatureDependencyTestData))]
        public void MethodSignatureDependenciesAreFound(MethodMember originMember,
            MethodSignatureDependency expectedDependency)
        {
            Assert.True(originMember.HasMethodSignatureDependency(expectedDependency));
        }
    }

    public class ClassWithMethodSignatureA
    {
        public ClassWithMethodSignatureC MethodA(ClassWithMethodSignatureB parameter)
        {
            return new ClassWithMethodSignatureC(parameter);
        }
    }

    public class ClassWithMethodSignatureB
    {
        public ClassWithMethodSignatureA MethodB(ClassWithMethodSignatureA parameter)
        {
            return parameter;
        }
    }

    public class ClassWithMethodSignatureC
    {
        private ClassWithMethodSignatureB _innerField;

        public ClassWithMethodSignatureC(ClassWithMethodSignatureB classWithMethodSignatureB)
        {
            _innerField = classWithMethodSignatureB;
        }

        public void OverloadedMethod(string s)
        {
        }

        public void OverloadedMethod(int i)
        {
        }
    }
}