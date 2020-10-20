//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;
using static ArchUnitNET.Domain.Visibility;
using Type = ArchUnitNET.Loader.Type;

namespace ArchUnitNETTests.Domain
{
    public class GenericClassTests
    {
        private const string GuidClassName = "Guid";
        private const string SystemGuidFullName = StaticConstants.SystemNamespace + "." + GuidClassName;

        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(GenericClassTests).Assembly).Build();

        private readonly Class _classWithGenericParameters;
        private readonly Class _expectedGenericArgument;
        private readonly FieldMember _genericallyTypedField;

        public GenericClassTests()
        {
            _classWithGenericParameters = Architecture.GetClassOfType(typeof(ClassWithGenericParameters<>));
            var invokesGenericClass = Architecture.GetClassOfType(typeof(InvokesGenericClass));
            _genericallyTypedField = invokesGenericClass
                .GetFieldMembersWithName(nameof(InvokesGenericClass.GuidGenericArgument)).SingleOrDefault();
            var guidMock = new Type(SystemGuidFullName, GuidClassName,
                _classWithGenericParameters.Assembly,
                new Namespace(StaticConstants.SystemNamespace, new List<IType>()), Public, false, false, true);
            _expectedGenericArgument = new Class(guidMock, false, true, true, false);
        }

        //TODO update tests
        // [Fact]
        // public void GenericTypeArgumentsAsExpected()
        // {
        //     //Setup
        //     var genericTypeArgumentClass = _genericallyTypedField.Type.GenericTypeArguments.First() as Class;
        //
        //     //Assert
        //     Assert.NotNull(genericTypeArgumentClass);
        //     Assert.Equal(_expectedGenericArgument, genericTypeArgumentClass);
        // }
        //
        // [Fact]
        // public void GenericTypeArgumentsFound()
        // {
        //     Assert.Single(_genericallyTypedField.Type.GenericTypeArguments);
        // }
        //
        // [Fact]
        // public void GenericTypeAsExpected()
        // {
        //     //Setup
        //     var invokedGenericType = _genericallyTypedField.Type;
        //
        //     //Assert
        //     Assert.Equal(_classWithGenericParameters, invokedGenericType.GenericType);
        // }

        [Fact]
        public void GenericTypeParametersFound()
        {
            Assert.NotEmpty(_classWithGenericParameters.GenericTypeParameters);
            Assert.Single(_classWithGenericParameters.GenericTypeParameters);
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