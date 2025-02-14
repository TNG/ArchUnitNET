namespace ArchUnitNET.Domain
{
    public class AttributeNamedArgument : AttributeArgument
    {
        public AttributeNamedArgument(string name, object value, ITypeInstance<IType> typeInstance)
            : base(value, typeInstance)
        {
            Name = name;
        }

        public readonly string Name;

        private bool Equals(AttributeNamedArgument other)
        {
            return base.Equals(other) && Equals(Name, other.Name);
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

            return obj.GetType() == GetType() && Equals((AttributeNamedArgument)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
