using TestAssembly.Domain.Marker;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace TestAssembly.Domain.Entities
{
    public class Entity1 : IEntity
    {
        public string TestProperty { get; private set; }
    }
}
