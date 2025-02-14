using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class SliceIdentifierComparer : IEqualityComparer<SliceIdentifier>
    {
        public bool Equals(SliceIdentifier x, SliceIdentifier y)
        {
            return x != null && x.CompareTo(y);
        }

        public int GetHashCode(SliceIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
}
