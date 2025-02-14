using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class MethodMemberInstance : ITypeInstance<IType>
    {
        public MethodMemberInstance(
            MethodMember member,
            IEnumerable<GenericArgument> declaringTypeGenericArguments,
            IEnumerable<GenericArgument> memberGenericArguments
        )
        {
            Member = member;
            GenericArguments = declaringTypeGenericArguments;
            MemberGenericArguments = memberGenericArguments;
        }

        public MethodMember Member { get; }
        public IEnumerable<GenericArgument> MemberGenericArguments { get; }
        public IType Type => Member.DeclaringType;
        public IEnumerable<GenericArgument> GenericArguments { get; }
        public bool IsArray => false;
        public IEnumerable<int> ArrayDimensions => Enumerable.Empty<int>();

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

            return Equals(Member, other.Member)
                && GenericArguments.SequenceEqual(other.GenericArguments)
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
                var hashCode = Member != null ? Member.GetHashCode() : 0;
                hashCode = GenericArguments.Aggregate(
                    hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0)
                );
                hashCode = MemberGenericArguments.Aggregate(
                    hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0)
                );
                return hashCode;
            }
        }
    }
}
