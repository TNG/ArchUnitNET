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

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNETTests.Dependencies.Attributes;
using ArchUnitNETTests.Dependencies.Members;
using TestAssembly;
// ReSharper disable InconsistentNaming

namespace ArchUnitNETTests.Fluent
{
    public static class StaticTestArchitectures
    {
        public static readonly Architecture AttributeDependencyTestArchitecture = 
            new ArchLoader().LoadAssemblies(typeof(ClassWithExampleAttribute).Assembly, typeof(Class1).Assembly)
                .Build();
        
        public static readonly Architecture ArchUnitNETTestArchitecture =
            new ArchLoader().LoadAssemblies(typeof(BaseClass).Assembly).Build();
    }
}