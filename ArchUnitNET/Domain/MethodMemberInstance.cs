using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class MethodMemberInstance
    {
        public MethodMemberInstance(
            ITypeInstance<IType> declaringTypeInstance,
            MethodMember member,
            IEnumerable<GenericArgument> memberGenericArguments
        )
        {
            DeclaringTypeInstance = declaringTypeInstance;
            Member = member;
            MemberGenericArguments = memberGenericArguments;
        }

        public ITypeInstance<IType> DeclaringTypeInstance { get; }
        public MethodMember Member { get; }
        public IEnumerable<GenericArgument> MemberGenericArguments { get; }

        public bool Equals(MethodMemberInstance other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return DeclaringTypeInstance.Equals(other.DeclaringTypeInstance)
                && Equals(Member, other.Member)
                && MemberGenericArguments.SequenceEqual(other.MemberGenericArguments);
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

            return Equals((MethodMemberInstance)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DeclaringTypeInstance.GetHashCode();
                hashCode = Member != null ? (hashCode * 397) ^ Member.GetHashCode() : hashCode;
                hashCode = MemberGenericArguments.Aggregate(
                    hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0)
                );
                return hashCode;
            }
        }
    }
}
