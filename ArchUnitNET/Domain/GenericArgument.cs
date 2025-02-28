using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class GenericArgument : ITypeInstance<IType>
    {
        public GenericArgument(ITypeInstance<IType> typeInstance)
        {
            Type = typeInstance.Type;
            GenericArguments = typeInstance.GenericArguments;
        }

        public IType Type { get; }
        public IEnumerable<GenericArgument> GenericArguments { get; }
        public bool IsArray { get; }
        public IEnumerable<int> ArrayDimensions { get; }

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

            return obj.GetType() == GetType() && Equals((GenericArgument)obj);
        }

        private bool Equals(GenericArgument other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Type, other.Type)
                && GenericArguments.SequenceEqual(other.GenericArguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type != null ? Type.GetHashCode() : 0;
                hashCode = GenericArguments.Aggregate(
                    hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0)
                );
                return hashCode;
            }
        }

        public override string ToString()
        {
            return Type.FullName;
        }
    }
}
