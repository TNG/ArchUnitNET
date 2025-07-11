using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class FrozenRuleIdentifierComparer : EqualityComparer<FrozenRuleIdentifier>
    {
        public override bool Equals(FrozenRuleIdentifier x, FrozenRuleIdentifier y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Identifier == y.Identifier;
        }

        public override int GetHashCode(FrozenRuleIdentifier obj)
        {
            return obj.Identifier.GetHashCode();
        }
    }
}
