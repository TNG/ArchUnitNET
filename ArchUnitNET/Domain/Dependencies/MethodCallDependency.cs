using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodCallDependency : MemberTypeInstanceDependency, IMemberMemberDependency
    {
        public MethodCallDependency(IMember originMember, MethodMemberInstance calledMethodInstance)
            : base(originMember, calledMethodInstance.DeclaringTypeInstance)
        {
            TargetMember = calledMethodInstance.Member;
            TargetMemberGenericArguments = calledMethodInstance.MemberGenericArguments;
        }

        public IMember TargetMember { get; }
        public IEnumerable<GenericArgument> TargetMemberGenericArguments { get; }

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

            return obj.GetType() == GetType() && Equals((MethodCallDependency)obj);
        }

        private bool Equals(MethodCallDependency other)
        {
            return Equals(OriginMember, other.OriginMember)
                && Equals(TargetMember, other.TargetMember)
                && Equals(TargetMemberGenericArguments, other.TargetMemberGenericArguments);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OriginMember != null ? OriginMember.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397) ^ (TargetMember != null ? TargetMember.GetHashCode() : 0);
                hashCode =
                    (hashCode * 397)
                    ^ (
                        TargetMemberGenericArguments != null
                            ? TargetMemberGenericArguments.GetHashCode()
                            : 0
                    );
                return hashCode;
            }
        }
    }
}
