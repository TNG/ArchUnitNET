using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// Identifies a slice by its name. Two identifiers are the same slice exactly when their
    /// names are equal and both are ignored or both are not. Slice patterns build the name from
    /// what their capture groups matched; see <see cref="Fluent.Slices.SliceRuleInitializer"/>.
    /// </summary>
    public class SliceIdentifier : StringIdentifier, IHasDescription
    {
        /// <summary>
        /// Groups identifiers by slice membership; see <see cref="CompareTo"/>.
        /// </summary>
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();

        /// <summary>
        /// Whether the type that produced this identifier failed to match the pattern and is
        /// therefore left out of the rule.
        /// </summary>
        public readonly bool Ignored;

        private SliceIdentifier(string identifier, bool ignored, string nameSpace = null)
            : base(identifier)
        {
            Ignored = ignored;
            NameSpace = nameSpace;
        }

        /// <summary>
        /// The slice's name, which is also its identity. A slice pattern names a slice after
        /// what its capture groups matched, joined with ".": "App.(*).(*)" over
        /// "App.Orders.Http" gives "Orders.Http".
        /// </summary>
        public string Description => Identifier;

        /// <summary>
        /// The namespace prefix the pattern matched before the first capture group, or
        /// <c>null</c> when the rule was defined with <c>Matching</c> rather than
        /// <c>MatchingWithPackages</c>. Only used to nest the slice in diagrams; it takes no
        /// part in the slice's identity.
        /// </summary>
        [CanBeNull]
        public readonly string NameSpace;

        /// <summary>
        /// Creates an identifier for the slice of the given name.
        /// </summary>
        /// <param name="identifier">
        /// The slice's name and identity: a custom <c>SliceAssignment</c> that returns
        /// identifiers of the same name for two types puts them into the same slice.
        /// </param>
        /// <param name="nameSpace">The namespace prefix to nest the slice under, if any.</param>
        public static SliceIdentifier Of(string identifier, string nameSpace = null)
        {
            return new SliceIdentifier(identifier, false, nameSpace);
        }

        /// <summary>
        /// Creates the identifier for a type that did not match the pattern. All such types
        /// share one slice, which the rule then leaves out.
        /// </summary>
        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier("Ignored", true);
        }

        /// <summary>
        /// Whether the two identifiers belong to the same slice: both ignored, or both not
        /// ignored with the same name.
        /// </summary>
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
