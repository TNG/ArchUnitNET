//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using Xunit;
using Xunit.Sdk;

// ReSharper disable UnusedTypeParameter

namespace ArchUnitNETTests.Domain.Dependencies.Types
{
    public class GenericInterfaceTests
    {
        public GenericInterfaceTests()
        {
            _genericInterface = _architecture.GetInterfaceOfType(typeof(IGenericInterface<>));
            _genericInterfaceImplementation = _architecture.GetClassOfType(typeof(GenericInterfaceImplementation));
            _genericType = _architecture.GetClassOfType(typeof(GenericType));
        }

        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Interface _genericInterface;
        private readonly Class _genericInterfaceImplementation;
        private readonly Class _genericType;

        [Fact]
        public void ClassImplementsGenericInterface()
        {
            // Setup, Act
            var implementedGenericInterface = _genericInterfaceImplementation.ImplementedInterfaces.FirstOrDefault();
            var genericArgumentsOfImplementedInterface = implementedGenericInterface?.GenericTypeArguments;

            // Assert
            Assert.True(_genericInterfaceImplementation.ImplementsInterface(_genericInterface));
            Assert.Single(genericArgumentsOfImplementedInterface ??
                          throw new NullException("No generic arguments from inherited " +
                                                  "generic interface implementation."));
            Assert.Contains(_genericType, genericArgumentsOfImplementedInterface);
        }
    }

    public interface IGenericInterface<TGenericType>
    {
    }

    public class GenericType
    {
    }

    public class GenericInterfaceImplementation : IGenericInterface<GenericType>
    {
    }
}