/*
 * Copyright 2019 TNG Technology Consulting GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Fluent;
using Xunit;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable UnusedMember.Global

namespace ArchUnitNETTests.Dependencies.Members
{
    public class MethodSignatureMemberDependencyTests
    {
        [Theory]
        [ClassData(typeof(MethodDependencyTestBuild.MethodSignatureDependencyTestData))]
        public void MethodSignatureDependenciesAreFound(MethodMember originMember, MethodSignatureDependency expectedDependency)
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