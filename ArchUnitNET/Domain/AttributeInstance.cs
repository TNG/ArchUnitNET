using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain
{
    public class AttributeInstance : TypeInstance<Attribute>
    {
        // ReSharper disable SuggestBaseTypeForParameter
        public AttributeInstance(
            Attribute attribute,
            IEnumerable<AttributeArgument> attributeArguments
        )
            : base(attribute)
        {
            AttributeArguments = attributeArguments;
        }

        public AttributeInstance(Attribute attribute)
            : base(attribute)
        {
            AttributeArguments = Enumerable.Empty<AttributeArgument>();
        }

        public IEnumerable<AttributeArgument> AttributeArguments { get; }

        public bool HasAttributeArguments => AttributeArguments.Any();
    }
}
