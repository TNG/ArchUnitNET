namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IPropertyMemberConditions<out TReturnType> : IMemberConditions<TReturnType>
    {
        TReturnType HaveGetter();
        TReturnType HavePrivateGetter();
        TReturnType HavePublicGetter();
        TReturnType HaveProtectedGetter();
        TReturnType HaveInternalGetter();
        TReturnType HaveProtectedInternalGetter();
        TReturnType HavePrivateProtectedGetter();
        TReturnType HaveSetter();
        TReturnType HavePrivateSetter();
        TReturnType HavePublicSetter();
        TReturnType HaveProtectedSetter();
        TReturnType HaveInternalSetter();
        TReturnType HaveProtectedInternalSetter();
        TReturnType HavePrivateProtectedSetter();
        TReturnType BeVirtual();


        //Negations


        TReturnType NotHaveGetter();
        TReturnType NotHavePrivateGetter();
        TReturnType NotHavePublicGetter();
        TReturnType NotHaveProtectedGetter();
        TReturnType NotHaveInternalGetter();
        TReturnType NotHaveProtectedInternalGetter();
        TReturnType NotHavePrivateProtectedGetter();
        TReturnType NotHaveSetter();
        TReturnType NotHavePrivateSetter();
        TReturnType NotHavePublicSetter();
        TReturnType NotHaveProtectedSetter();
        TReturnType NotHaveInternalSetter();
        TReturnType NotHaveProtectedInternalSetter();
        TReturnType NotHavePrivateProtectedSetter();
        TReturnType NotBeVirtual();
    }
}