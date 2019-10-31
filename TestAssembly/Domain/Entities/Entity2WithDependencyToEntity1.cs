//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using TestAssembly.Domain.Marker;

// ReSharper disable UnusedMember.Global

namespace TestAssembly.Domain.Entities
{
    public class Entity2WithDependencyToEntity1 : IEntity
    {
        public Entity1 HasEntity1;
        public List<Entity1> ManyEntity1;
    }
}