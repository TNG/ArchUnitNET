namespace ArchUnitNET.Domain.Dependencies
{
    public class BodyTypeMemberDependency : MemberTypeInstanceDependency
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public BodyTypeMemberDependency(
            MethodMember method,
            ITypeInstance<IType> targetTypeInstance
        )
            : base(method, targetTypeInstance) { }
    }
}
