using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class Namespace : Slice, IHasName
    {
        public Namespace(string name, IEnumerable<IType> types)
            : base(FrozenRuleSliceIdentifier.Of(name), types) { }

        public string Name => Description;
        public string FullName => Description;
    }
}
