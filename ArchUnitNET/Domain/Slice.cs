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

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies.Types;
using Equ;

namespace ArchUnitNET.Domain
{
    public class Slice<TKey> : MemberwiseEquatable<Slice<TKey>>, IHasDependencies
    {
        private readonly List<IType> _types;

        public Slice(TKey sliceKey, List<IType> types)
        {
            _types = types;
            SliceKey = sliceKey;
        }

        public TKey SliceKey { get; }
        public IEnumerable<IType> Types => _types;
        public IEnumerable<Class> Classes => _types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => _types.OfType<Interface>();


        public List<ITypeDependency> Dependencies => _types.SelectMany(type => type.Dependencies).ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            _types.SelectMany(type => type.BackwardsDependencies).ToList();
    }
}