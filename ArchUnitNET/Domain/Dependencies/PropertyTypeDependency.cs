namespace ArchUnitNET.Domain.Dependencies
{
    public class PropertyTypeDependency : MemberTypeInstanceDependency
    {
        public PropertyTypeDependency(PropertyMember property)
            : base(property, property.Type)
        {
            TargetProperty = property;
        }

        public PropertyMember TargetProperty { get; }

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

            return obj.GetType() == GetType() && Equals((PropertyTypeDependency)obj);
        }

        private bool Equals(PropertyTypeDependency other)
        {
            return Equals(OriginMember, other.OriginMember)
                && Equals(TargetProperty, other.TargetProperty);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OriginMember != null ? OriginMember.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397) ^ (TargetProperty != null ? TargetProperty.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
