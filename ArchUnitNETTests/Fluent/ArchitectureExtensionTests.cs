/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Linq;
using ArchUnitNET.ArchitectureExceptions;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using TestAssembly;
using TestAssembly.Domain.Marker;
using Xunit;

namespace ArchUnitNETTests.Fluent
{
    public class ArchitectureExtensionTests
    {
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        [Fact]
        public void TypeNotInArchitectureNotFound()
        {
            Assert.Throws<TypeDoesNotExistInArchitecture>(() => _architecture.GetTypeOfType(typeof(Guid)));
        }

        [Fact]
        public void FoundCorrectClassInArchitecture()
        {
            Assert.Equal(_architecture.Classes.SingleOrDefault(archClass => archClass.Name == nameof(Class1)),
                _architecture.GetClassOfType(typeof(Class1)));
        }
        
        [Fact]
        public void FoundCorrectInterfaceInArchitecture()
        {
            Assert.Equal(_architecture.Interfaces
                    .SingleOrDefault(archClass => archClass.Name == nameof(IEntity)),
                _architecture.GetInterfaceOfType(typeof(IEntity)));
        }
    }
}