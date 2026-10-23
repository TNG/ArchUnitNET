using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    /// <summary>
    /// A namespace of the loaded architecture together with the types declared directly in it.
    /// Namespaces are equal exactly when their full names are.
    /// </summary>
    public class Namespace : IHasName, IHasDescription, IHasDependencies
    {
        private readonly string _name;
        private readonly List<IType> _types;

        public Namespace(string name, IEnumerable<IType> types)
        {
            _name = name;
            _types = types.ToList();
        }

        /// <summary>The full name of this namespace.</summary>
        public string Name => _name;

        /// <summary>The full name of this namespace.</summary>
        public string FullName => _name;

        /// <summary>The full name of this namespace.</summary>
        public string Description => _name;

        /// <summary>The types declared directly in this namespace.</summary>
        public IEnumerable<IType> Types => _types;

        /// <summary>The classes declared directly in this namespace.</summary>
        public IEnumerable<Class> Classes => Types.OfType<Class>();

        /// <summary>The interfaces declared directly in this namespace.</summary>
        public IEnumerable<Interface> Interfaces => Types.OfType<Interface>();

        /// <summary>Every dependency going out of this namespace's types, including internal ones.</summary>
        public List<ITypeDependency> Dependencies =>
            Types.SelectMany(type => type.Dependencies).ToList();

        /// <summary>Every dependency pointing at this namespace's types.</summary>
        public List<ITypeDependency> BackwardsDependencies =>
            Types.SelectMany(type => type.BackwardsDependencies).ToList();

        internal void AddType(IType type)
        {
            _types.Add(type);
        }

        private bool Equals(Namespace other)
        {
            return FullName == other.FullName;
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

            return obj.GetType() == GetType() && Equals((Namespace)obj);
        }

        public override int GetHashCode()
        {
            return FullName != null ? FullName.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
