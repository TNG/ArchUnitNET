using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    /// <summary>
    ///     What a slice pattern captured for one type: one part per capture group, plus the text
    ///     the pattern puts between them. Two identifiers belong to the same slice exactly when
    ///     their <see cref="Parts"/> match in order -- the separators only shape the name.
    /// </summary>
    public class SliceIdentifier : StringIdentifier, IHasDescription
    {
        /// <summary>
        ///     Groups identifiers by slice membership; see <see cref="CompareTo"/>.
        /// </summary>
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();

        /// <summary>
        ///     Whether the type that produced this identifier failed to match the pattern and is
        ///     therefore left out of the rule.
        /// </summary>
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

        /// <summary>
        ///     The slice's name: the captured parts joined by the pattern text that sits between
        ///     their capture groups. "App.(*).(*)" over "App.Orders.Http" gives "Orders.Http",
        ///     "App.(*)..(*)" over "App.Orders.Web.Http" gives "Orders..Http" -- the ".." says
        ///     segments were skipped, so the name is never mistaken for a namespace that exists.
        /// </summary>
        public string Description => Identifier;

        /// <summary>
        ///     The namespace prefix the pattern matched before the first capture group, or
        ///     <c>null</c> when the rule was defined with <c>Matching</c> rather than
        ///     <c>MatchingWithPackages</c>. Only used to nest the slice in diagrams.
        /// </summary>
        [CanBeNull]
        public readonly string NameSpace;

        /// <summary>
        ///     Creates an identifier with a single captured part.
        /// </summary>
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

        /// <summary>
        ///     Creates the identifier for a type that did not match the pattern. All such types
        ///     share one slice, which the rule then leaves out.
        /// </summary>
        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier(new[] { "Ignored" }, null, true);
        }

        /// <summary>
        ///     Whether the two identifiers belong to the same slice: both ignored, or both not
        ///     ignored with the same <see cref="Parts"/> in the same order.
        /// </summary>
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
