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
