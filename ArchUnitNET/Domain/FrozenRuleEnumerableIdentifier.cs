using System.Collections.Generic;
using System.Text;

namespace ArchUnitNET.Domain
{
    public class FrozenRuleEnumerableIdentifier : FrozenRuleIdentifier
    {
        public FrozenRuleEnumerableIdentifier(IEnumerable<FrozenRuleIdentifier> enumerable)
            : base(CreateIdentifierString(enumerable)) { }

        private static string CreateIdentifierString(IEnumerable<FrozenRuleIdentifier> enumerable)
        {
            var sb = new StringBuilder("<>_first:");
            foreach (var identifier in enumerable)
            {
                sb.Append(identifier.Identifier);
                sb.Append("+<>_next:");
            }

            sb.Append("+<>_end");
            return sb.ToString();
        }
    }
}
