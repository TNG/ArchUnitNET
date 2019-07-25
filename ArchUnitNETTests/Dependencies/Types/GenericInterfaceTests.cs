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

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Xunit;
using Xunit.Sdk;

// ReSharper disable UnusedTypeParameter

namespace ArchUnitNETTests.Dependencies.Types
{
    public class GenericInterfaceTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Interface _genericInterface;
        private readonly Class _genericInterfaceImplementation;
        private readonly Class _genericType;

        public GenericInterfaceTests()
        {
            _genericInterface = _architecture.GetInterfaceOfType(typeof(IGenericInterface<>));
            _genericInterfaceImplementation = _architecture.GetClassOfType(typeof(GenericInterfaceImplementation));
            _genericType = _architecture.GetClassOfType(typeof(GenericType));
        }

        [Fact]
        public void ClassImplementsGenericInterface()
        {
            // Setup, Act
            var implementedGenericInterface = _genericInterfaceImplementation.ImplementedInterfaces.FirstOrDefault();
            var genericArgumentsOfImplementedInterface = implementedGenericInterface?.GenericTypeArguments;

            // Assert
            Assert.True(_genericInterfaceImplementation.Implements(_genericInterface));
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