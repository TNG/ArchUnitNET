using System.Collections.Generic;

namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodCallDependency : MemberTypeInstanceDependency, IMemberMemberDependency
    {
        public MethodCallDependency(IMember originMember, MethodMemberInstance calledMethodInstance)
            : base(originMember, calledMethodInstance)
        {
            TargetMember = calledMethodInstance.Member;
            TargetMemberGenericArguments = calledMethodInstance.MemberGenericArguments;
        }

        public IMember TargetMember { get; }
        public IEnumerable<GenericArgument> TargetMemberGenericArguments { get; }
    }
}
