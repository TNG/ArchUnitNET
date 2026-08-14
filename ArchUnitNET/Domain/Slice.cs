using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    /// <summary>
    ///     A group of types whose namespaces produced the same <see cref="SliceIdentifier"/> under
    ///     a slice pattern. Slices are the unit that cycle-freedom and independence rules are
    ///     checked on.
    /// </summary>
    public class Slice : IHasDescription, IHasDependencies
    {
        /// <summary>
        ///     What the pattern captured for this slice. Slices are equal exactly when their
        ///     identifiers are.
        /// </summary>
        public readonly SliceIdentifier Identifier;

        /// <summary>
        ///     The types that fell into this slice.
        /// </summary>
        public readonly IEnumerable<IType> Types;

        public Slice(SliceIdentifier identifier, IEnumerable<IType> types)
        {
            Identifier = identifier;
            Types = types;
        }

        /// <summary>The classes in this slice.</summary>
        public IEnumerable<Class> Classes => Types.OfType<Class>();

        /// <summary>The interfaces in this slice.</summary>
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();

        /// <summary>Every dependency going out of this slice's types, including internal ones.</summary>
        public List<ITypeDependency> Dependencies =>
            Types.SelectMany(type => type.Dependencies).ToList();

        /// <summary>Every dependency pointing at this slice's types.</summary>
        public List<ITypeDependency> BackwardsDependencies =>
            Types.SelectMany(type => type.BackwardsDependencies).ToList();

        /// <summary>
        ///     This slice's name. See <see cref="SliceIdentifier.Description"/> for how it is built.
        /// </summary>
        public string Description => Identifier.Description;

        /// <summary>
        ///     The namespace this slice sits under, or <c>null</c> if the rule was defined with
        ///     <c>Matching</c> rather than <c>MatchingWithPackages</c>. Only used for diagrams.
        /// </summary>
        [CanBeNull]
        public string NameSpace => Identifier.NameSpace;

        protected bool Equals(Slice other)
        {
            return Equals(Identifier, other.Identifier);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Slice)obj);
        }

        public override int GetHashCode()
        {
            return Identifier != null ? Identifier.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        ///     Whether this slice carries a namespace, which is the case when the rule was defined
        ///     with <c>MatchingWithPackages</c>.
        /// </summary>
        public bool ContainsNamespace()
        {
            return NameSpace != null;
        }
    }
}
