using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public interface IHasAttributes
    {
        IEnumerable<Attribute> Attributes { get; }
        List<AttributeInstance> AttributeInstances { get; }
    }
}
