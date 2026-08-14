using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class SliceIdentifier : StringIdentifier, IHasDescription
    {
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();
        public readonly bool Ignored;

        /// <summary>
        ///     The captured name parts, one per capture group in the matching pattern. Two slices
        ///     are the same exactly when these match in order.
        /// </summary>
        public readonly IReadOnlyList<string> Parts;

        private SliceIdentifier(
            IReadOnlyList<string> parts,
            IReadOnlyList<string> separators,
            bool ignored,
            string nameSpace = null
        )
            : base(FormatIdentifier(parts, separators, ignored))
        {
            Parts = parts;
            Ignored = ignored;
            NameSpace = nameSpace;
        }

        public string Description => Identifier;

        [CanBeNull]
        public readonly string NameSpace;

        public static SliceIdentifier Of(string identifier, string nameSpace = null)
        {
            return new SliceIdentifier(new[] { identifier }, null, false, nameSpace);
        }

        /// <param name="parts">One captured value per capture group in the pattern.</param>
        /// <param name="separators">
        ///     The text to put between adjacent parts, one shorter than <paramref name="parts"/>.
        ///     Pass <c>null</c> to join everything with ".".
        /// </param>
        /// <param name="nameSpace">The namespace prefix to nest the slice under, if any.</param>
        public static SliceIdentifier Of(
            IReadOnlyList<string> parts,
            IReadOnlyList<string> separators = null,
            string nameSpace = null
        )
        {
            return new SliceIdentifier(parts, separators, false, nameSpace);
        }

        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier(new[] { "Ignored" }, null, true);
        }

        /// <summary>
        ///     Is true when the two SliceIdentifiers belong to the same slice
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CompareTo(SliceIdentifier other)
        {
            if (other == null)
            {
                return false;
            }

            if (Ignored && other.Ignored)
            {
                return true;
            }

            if (Ignored || other.Ignored)
            {
                return false;
            }

            return Parts.SequenceEqual(other.Parts);
        }

        private bool Equals(SliceIdentifier other)
        {
            return Ignored == other.Ignored && Parts.SequenceEqual(other.Parts);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((SliceIdentifier)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ Ignored.GetHashCode();
                foreach (var part in Parts)
                {
                    hashCode = (hashCode * 397) ^ part.GetHashCode();
                }

                return hashCode;
            }
        }

        private static string FormatIdentifier(
            IReadOnlyList<string> parts,
            IReadOnlyList<string> separators,
            bool ignored
        )
        {
            if (ignored)
            {
                return "Ignored";
            }

            if (separators == null)
            {
                return string.Join(".", parts);
            }

            // A slice has exactly one name: PlantUML component aliases may not contain whitespace,
            // and the exporter splits the name on "." to nest package blocks, so anything else
            // would leave messages, the freeze store and diagrams disagreeing.
            var result = new StringBuilder(parts[0]);
            for (var i = 1; i < parts.Count; i++)
            {
                result.Append(separators[i - 1]).Append(parts[i]);
            }

            return result.ToString();
        }
    }
}
