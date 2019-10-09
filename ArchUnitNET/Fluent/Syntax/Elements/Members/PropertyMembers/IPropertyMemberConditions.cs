namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IPropertyMemberConditions<out TReturnType> : IMemberConditions<TReturnType>
    {
        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType BePrivate();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType BePublic();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType BeProtected();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType BeInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType BeProtectedInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType BePrivateProtected();

        TReturnType HaveGetter();
        TReturnType HaveSetter();
        TReturnType HavePrivateSetter();
        TReturnType HavePublicSetter();
        TReturnType HaveProtectedSetter();
        TReturnType HaveInternalSetter();
        TReturnType HaveProtectedInternalSetter();
        TReturnType HavePrivateProtectedSetter();
        TReturnType BeVirtual();


        //Negations


        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType NotBePrivate();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType NotBePublic();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType NotBeProtected();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType NotBeInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType NotBeProtectedInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TReturnType NotBePrivateProtected();

        TReturnType NotHaveGetter();
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