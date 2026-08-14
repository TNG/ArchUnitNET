using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    /// <summary>
    ///     An equality comparer for <see cref="SliceIdentifier"/> that groups identifiers by slice
    ///     membership. Two identifiers are equal if they both represent ignored types, or if they
    ///     are both non-ignored and have the same parts in order.
    /// </summary>
    public class SliceIdentifierComparer : IEqualityComparer<SliceIdentifier>
    {
        /// <inheritdoc />
        public bool Equals(SliceIdentifier x, SliceIdentifier y)
        {
            return x != null && x.CompareTo(y);
        }

        /// <inheritdoc />
        public int GetHashCode(SliceIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
}
