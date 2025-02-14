using TestAssembly.Domain.Marker;

// ReSharper disable UnusedMember.Global

namespace TestAssembly.Domain.Entities
{
    public class EntityWithPublicSetters : IEntity
    {
        public string TestProperty { get; set; }
    }
}
