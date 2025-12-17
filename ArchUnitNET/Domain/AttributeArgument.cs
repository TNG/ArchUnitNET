using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class AttributeArgument
    {
        public AttributeArgument(object value, ITypeInstance<IType> typeInstance)
        {
            Value = value;
            Type = typeInstance;
        }

        public readonly object Value;

        public ITypeInstance<IType> Type { get; }

        private bool Equals(AttributeArgument other)
        {
            return Equals(Type, other.Type) && Equals(Value, other.Value);
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

            return obj.GetType() == GetType() && Equals((AttributeArgument)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type != null ? Type.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
