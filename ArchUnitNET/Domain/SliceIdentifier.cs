namespace ArchUnitNET.Domain
{
    public class SliceIdentifier : StringIdentifier, IHasDescription
    {
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();
        public readonly bool Ignored;

        private SliceIdentifier(
            string identifier,
            bool ignored,
            int? countOfAsteriskInPattern = null
        )
            : base(identifier)
        {
            Ignored = ignored;
            CountOfAsteriskInPattern = countOfAsteriskInPattern;
        }

        public string Description => Identifier;

        public readonly int? CountOfAsteriskInPattern;

        public static SliceIdentifier Of(string identifier, int? countOfAsteriskInPattern = null)
        {
            return new SliceIdentifier(identifier, false, countOfAsteriskInPattern);
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
