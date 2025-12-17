namespace ArchUnitNET.Domain.Dependencies
{
    public class FieldTypeDependency : MemberTypeInstanceDependency
    {
        public FieldTypeDependency(FieldMember field)
            : base(field, field.Type)
        {
            TargetField = field;
        }

        public FieldMember TargetField { get; }

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

            return obj.GetType() == GetType() && Equals((FieldTypeDependency)obj);
        }

        private bool Equals(FieldTypeDependency other)
        {
            return Equals(OriginMember, other.OriginMember)
                && Equals(TargetField, other.TargetField);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OriginMember != null ? OriginMember.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (TargetField != null ? TargetField.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
