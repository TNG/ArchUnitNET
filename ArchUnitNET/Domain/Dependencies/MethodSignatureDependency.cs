namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodSignatureDependency : MemberTypeInstanceDependency
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public MethodSignatureDependency(
            MethodMember method,
            ITypeInstance<IType> signatureTypeInstance
        )
            : base(method, signatureTypeInstance) { }
    }
}
