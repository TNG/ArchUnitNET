/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using TestAssembly.Domain.Marker;

// ReSharper disable UnusedMember.Global

namespace TestAssembly.Domain.Entities
{
    public class EntityWithPublicSetters : IEntity
    {
        public string TestProperty { get; set; }
    }
}