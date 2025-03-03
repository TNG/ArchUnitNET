using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class StringIdentifierComparer : EqualityComparer<StringIdentifier>
    {
        public override bool Equals(StringIdentifier x, StringIdentifier y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Identifier == y.Identifier;
        }

        public override int GetHashCode(StringIdentifier obj)
        {
            return obj.Identifier.GetHashCode();
        }
    }
}
