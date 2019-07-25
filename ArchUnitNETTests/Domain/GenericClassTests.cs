/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
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


using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using Xunit;
using Type = ArchUnitNET.Core.Type;

namespace ArchUnitNETTests.Domain
{
    public class GenericClassTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(GenericClassTests).Assembly).Build();
        
        private readonly Class _classWithGenericParameters;
        private readonly FieldMember _genericallyTypedField;
        private readonly Class _expectedGenericArgument;

        public GenericClassTests()
        {
            _classWithGenericParameters = Architecture.GetClassOfType(typeof(ClassWithGenericParameters<>));
            var invokesGenericClass = Architecture.GetClassOfType(typeof(InvokesGenericClass));
            _genericallyTypedField = invokesGenericClass
                .GetFieldMembersWithName(nameof(InvokesGenericClass.GuidGenericArgument)).SingleOrDefault();
            var guidMock = new Type(StaticConstants.SystemGuidFullName, StaticConstants.GuidClassName,
                _classWithGenericParameters.Assembly,
                new Namespace(StaticConstants.SystemNamespace, new List<IType>()));
            _expectedGenericArgument = new Class(guidMock, false);
        }
        
        [Fact]
        public void GenericTypeParametersFound()
        {
            Assert.NotEmpty(_classWithGenericParameters.GenericTypeParameters);
            Assert.Single(_classWithGenericParameters.GenericTypeParameters);
        }

        [Fact]
        public void GenericTypeArgumentsFound()
        {
            Assert.Single(_genericallyTypedField.Type.GenericTypeArguments);
        }
        
        [Fact]
        public void GenericTypeArgumentsAsExpected()
        {
            //Setup
            var genericTypeArgumentClass = _genericallyTypedField.Type.GenericTypeArguments.First() as Class;
            
            //Assert
            Assert.NotNull(genericTypeArgumentClass);
            Assert.Equal(_expectedGenericArgument, genericTypeArgumentClass);
        }

        [Fact]
        public void GenericTypeAsExpected()
        {
            //Setup
            var invokedGenericType = _genericallyTypedField.Type;
            
            //Assert
            Assert.Equal(_classWithGenericParameters, invokedGenericType.GenericType);
        }
    }
    
    public class ClassWithGenericParameters<T>
    {
        public void Add(T item)
        {
        }
    }

    public class InvokesGenericClass
    {
        public ClassWithGenericParameters<Guid> GuidGenericArgument = new ClassWithGenericParameters<Guid>();
    }
}