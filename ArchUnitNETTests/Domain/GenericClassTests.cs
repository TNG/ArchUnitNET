/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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