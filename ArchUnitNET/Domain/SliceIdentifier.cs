using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Identifies a slice by its parts: for a slice pattern, the values its capture groups
    /// captured, in group order. Two identifiers are the same slice exactly when their parts are
    /// equal and both are ignored or both are not.
    /// </summary>
    public class SliceIdentifier : IHasDescription
    {
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();
        public readonly bool Ignored;

        private SliceIdentifier(
            IReadOnlyList<string> parts,
            bool ignored,
            int? countOfAsteriskInPattern = null
        )
        {
            Parts = parts;
            Ignored = ignored;
            CountOfAsteriskInPattern = countOfAsteriskInPattern;
        }

        /// <summary>
        /// What identifies the slice: for a slice pattern, the values its capture groups
        /// captured, in group order. "App.(**)" over "App.Orders.Web" gives
        /// <c>[Orders.Web]</c>.
        /// </summary>
        public IReadOnlyList<string> Parts { get; }

        /// <summary>
        /// The slice's name, built from its parts: "Slice " followed by the parts joined with
        /// " - ", e.g. "Slice Orders - Http". It depends on the parts only, so equal slices always
        /// have the same name.
        /// </summary>
        public string Description => "Slice " + string.Join(" - ", Parts);

        public readonly int? CountOfAsteriskInPattern;

        /// <summary>
        /// Creates an identifier for a slice with a single part, the same slice as
        /// <c>Of(new[] { part })</c>.
        /// </summary>
        /// <param name="part">
        /// The slice's only part: a custom <c>SliceAssignment</c> that returns identifiers with the
        /// same part for two types puts them into the same slice.
        /// </param>
        /// <param name="countOfAsteriskInPattern">
        /// The number of "(*)" groups in the pattern, if the slice comes from one.
        /// </param>
        public static SliceIdentifier Of(string part, int? countOfAsteriskInPattern = null)
        {
            return new SliceIdentifier(CheckParts(new[] { part }), false, countOfAsteriskInPattern);
        }

        /// <summary>
        /// Creates an identifier for the slice with the given parts. Identifiers with equal parts,
        /// in the same order, belong to the same slice.
        /// </summary>
        /// <param name="parts">The parts, at least one; none of them may be <c>null</c>.</param>
        /// <exception cref="ArgumentException">There are no parts, or one of them is null.</exception>
        public static SliceIdentifier Of(IEnumerable<string> parts)
        {
            return new SliceIdentifier(CheckParts(parts), false);
        }

        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier(new[] { "Ignored" }, true);
        }

        private static IReadOnlyList<string> CheckParts(IEnumerable<string> parts)
        {
            if (parts == null)
            {
                throw new ArgumentNullException(nameof(parts));
            }

            var list = parts.ToList();
            if (list.Count == 0)
            {
                throw new ArgumentException("A slice needs at least one part.", nameof(parts));
            }

            if (list.Any(part => part == null))
            {
                throw new ArgumentException("A slice part can't be null.", nameof(parts));
            }

            return list.AsReadOnly();
        }

        /// <summary>
        /// Whether the two identifiers belong to the same slice: both ignored, or both not
        /// ignored with the same parts.
        /// </summary>
        public bool CompareTo(SliceIdentifier other)
        {
            if (other == null)
            {
                return false;
            }

            return Ignored && other.Ignored
                || !Ignored && !other.Ignored && Parts.SequenceEqual(other.Parts);
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
                var hashCode = Ignored.GetHashCode();
                foreach (var part in Parts)
                {
                    hashCode = (hashCode * 397) ^ part.GetHashCode();
                }

                return hashCode;
            }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
