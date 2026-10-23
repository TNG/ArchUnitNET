using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class SliceIdentifier : StringIdentifier, IHasDescription
    {
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();
        public readonly bool Ignored;

        private SliceIdentifier(
            string identifier,
            bool ignored,
            int? countOfAsteriskInPattern = null,
            string matchedPrefix = null,
            string diagramPath = null
        )
            : base(identifier)
        {
            Ignored = ignored;
            CountOfAsteriskInPattern = countOfAsteriskInPattern;
            MatchedPrefix = matchedPrefix;
            DiagramPath = diagramPath;
        }

        public string Description => Identifier;

        /// <summary>
        /// The part of the type's namespace the pattern matched before the first capture group,
        /// e.g. "App." for "App.(*)" over "App.Orders", or an empty string if the pattern starts
        /// with a group; <c>null</c> if the identifier does not come from a slice pattern. Only
        /// used to nest the slice in diagrams; it takes no part in the slice's identity.
        /// </summary>
        [CanBeNull]
        internal readonly string MatchedPrefix;

        /// <summary>
        /// The path a diagram nests the slice along below <see cref="MatchedPrefix"/>: the part of
        /// the namespace the pattern captured, e.g. "Orders.Web" for "App.(**)" over
        /// "App.Orders.Web". <c>null</c> if the identifier does not come from a slice pattern. It
        /// takes no part in the slice's identity.
        /// </summary>
        [CanBeNull]
        internal readonly string DiagramPath;

        public readonly int? CountOfAsteriskInPattern;

        public static SliceIdentifier Of(string identifier, int? countOfAsteriskInPattern = null)
        {
            return new SliceIdentifier(identifier, false, countOfAsteriskInPattern);
        }

        /// <summary>
        /// Creates the identifier a slice pattern assigns to a type, together with where the
        /// pattern matched, which diagrams use to nest the slice.
        /// </summary>
        internal static SliceIdentifier OfMatch(
            string identifier,
            int? countOfAsteriskInPattern,
            string matchedPrefix,
            string diagramPath
        )
        {
            return new SliceIdentifier(
                identifier,
                false,
                countOfAsteriskInPattern,
                matchedPrefix,
                diagramPath
            );
        }

        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier("Ignored", true);
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

            return Ignored && other.Ignored
                || !Ignored && !other.Ignored && Identifier == other.Identifier;
        }

        private bool Equals(SliceIdentifier other)
        {
            return Identifier == other.Identifier && Ignored == other.Ignored;
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
                var hashCode = 397 ^ Identifier.GetHashCode();
                hashCode = (hashCode * 397) ^ Ignored.GetHashCode();
                return hashCode;
            }
        }
    }
}
