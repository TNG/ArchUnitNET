using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class Slice : IHasDescription, IHasDependencies
    {
        public readonly SliceIdentifier Identifier;
        public readonly IEnumerable<IType> Types;

        public Slice(SliceIdentifier identifier, IEnumerable<IType> types)
        {
            Identifier = identifier;
            Types = types;
        }

        public IEnumerable<Class> Classes => Types.OfType<Class>();
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();

        public List<ITypeDependency> Dependencies =>
            Types.SelectMany(type => type.Dependencies).ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            Types.SelectMany(type => type.BackwardsDependencies).ToList();

        public string Description => Identifier.Description;

        public int? CountOfAsteriskInPattern => Identifier.CountOfAsteriskInPattern;

        /// <summary>
        /// The namespace a diagram nests this slice under when it nests slices in packages: the
        /// part of the namespace that the pattern matched before the first capture group, if
        /// it is the same for every type in the slice, which may be empty. <c>null</c> if it
        /// differs between the types, or if the slice does not come from a slice pattern; a
        /// diagram then draws the slice on its own. It takes no part in the slice's identity.
        /// </summary>
        [CanBeNull]
        internal string DiagramNamespace { get; set; }

        /// <summary>
        /// Whether diagrams nest this slice in packages even if
        /// <see cref="PlantUml.Export.GenerationOptions.NestSlicesInPackages"/> is not set, which
        /// is the case for slices of the obsolete <c>MatchingWithPackages</c>.
        /// </summary>
        internal bool NestByDefault { get; set; }

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
    }
}
