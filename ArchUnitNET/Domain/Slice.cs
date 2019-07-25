/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
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