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
        private readonly Architecture _architecture = StaticTestArchitectures.ArchUnitCsTestArchitecture;

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