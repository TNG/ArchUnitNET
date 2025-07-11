using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class FrozenRuleSliceIdentifier : FrozenRuleIdentifier, IHasDescription
    {
        public static readonly FrozenRuleSliceIdentifierComparer Comparer = new FrozenRuleSliceIdentifierComparer();
        public readonly bool Ignored;

        private FrozenRuleSliceIdentifier(
            string identifier,
            bool ignored,
            int? countOfAsteriskInPattern = null,
            string nameSpace = null
        )
            : base(identifier)
        {
            Ignored = ignored;
            CountOfAsteriskInPattern = countOfAsteriskInPattern;
            NameSpace = nameSpace;
        }

        public string Description => Identifier;

        [CanBeNull]
        public readonly string NameSpace;
        public readonly int? CountOfAsteriskInPattern;

        public static FrozenRuleSliceIdentifier Of(
            string identifier,
            int? countOfAsteriskInPattern = null,
            string nameSpace = null
        )
        {
            return new FrozenRuleSliceIdentifier(identifier, false, countOfAsteriskInPattern, nameSpace);
        }

        public static FrozenRuleSliceIdentifier Ignore()
        {
            return new FrozenRuleSliceIdentifier("Ignored", true);
        }

        /// <summary>
        ///     Is true when the two SliceIdentifiers belong to the same slice
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CompareTo(FrozenRuleSliceIdentifier other)
        {
            if (other == null)
            {
                return false;
            }

            return Ignored && other.Ignored
                || !Ignored && !other.Ignored && Identifier == other.Identifier;
        }

        private bool Equals(FrozenRuleSliceIdentifier other)
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

            return obj.GetType() == GetType() && Equals((FrozenRuleSliceIdentifier)obj);
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
