using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class SliceIdentifier : StringIdentifier, IHasDescription
    {
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();
        public readonly bool Ignored;

        /// <summary>
        ///     The captured name parts, one per capture group in the matching pattern.
        /// </summary>
        public readonly IReadOnlyList<string> Parts;

        private SliceIdentifier(IReadOnlyList<string> parts, bool ignored, string nameSpace = null)
            : base(FormatIdentifier(parts, ignored))
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
            return new SliceIdentifier(new[] { identifier }, false, nameSpace);
        }

        public static SliceIdentifier Of(IReadOnlyList<string> parts, string nameSpace = null)
        {
            return new SliceIdentifier(parts, false, nameSpace);
        }

        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier(new[] { "Ignored" }, true);
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

        private static string FormatIdentifier(IReadOnlyList<string> parts, bool ignored)
        {
            // Joined with "." so that a slice has exactly one name: PlantUML component aliases may
            // not contain whitespace, and the exporter splits the name on "." to nest package
            // blocks. "(**)" already produces dotted names, so this also makes "App.(*).(*)" name
            // the same grouping the same way as "App.(**)".
            return ignored ? "Ignored" : string.Join(".", parts);
        }
    }
}
