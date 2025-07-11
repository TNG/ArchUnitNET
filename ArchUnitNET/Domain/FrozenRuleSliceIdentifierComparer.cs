using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class FrozenRuleSliceIdentifierComparer : IEqualityComparer<FrozenRuleSliceIdentifier>
    {
        public bool Equals(FrozenRuleSliceIdentifier x, FrozenRuleSliceIdentifier y)
        {
            return x != null && x.CompareTo(y);
        }

        public int GetHashCode(FrozenRuleSliceIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
}
