using System.Collections.Generic;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodCallDependency : MemberTypeInstanceDependency, IMemberMemberDependency
    {
        public MethodCallDependency(IMember originMember, MethodMemberInstance calledMethodInstance)
            : base(originMember, calledMethodInstance)
        {
            TargetMember = calledMethodInstance.Member;
            TargetMemberGenericArguments = calledMethodInstance.GenericArguments;
        }

        public IMember TargetMember { get; }
        public IEnumerable<GenericArgument> TargetMemberGenericArguments { get; }
    }
}
