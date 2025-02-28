using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class Assembly : IHasName, IHasAttributes
    {
        public Assembly(
            string name,
            string fullName,
            bool isOnlyReferenced,
            List<string> referencedAssemblyNames
        )
        {
            Name = name;
            FullName = fullName;
            IsOnlyReferenced = isOnlyReferenced;
            ReferencedAssemblyNames = referencedAssemblyNames;
        }

        public bool IsOnlyReferenced { get; }

        public string Name { get; }

        public List<string> ReferencedAssemblyNames { get; }
        public string FullName { get; }

        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances { get; } = new List<AttributeInstance>();

        public bool Equals(Assembly other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Name, other.Name)
                && Equals(FullName, other.FullName)
                && Equals(IsOnlyReferenced, other.IsOnlyReferenced);
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

            return Equals((Assembly)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsOnlyReferenced.GetHashCode();
                return hashCode;
            }
        }
    }
}
