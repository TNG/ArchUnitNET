using System.Collections.Generic;
using System.Text;

namespace ArchUnitNET.Domain
{
    public class EnumerableIdentifier : StringIdentifier
    {
        public EnumerableIdentifier(IEnumerable<StringIdentifier> enumerable)
            : base(CreateIdentifierString(enumerable)) { }

        private static string CreateIdentifierString(IEnumerable<StringIdentifier> enumerable)
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
