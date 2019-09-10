namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IPropertyMembersThat<TRuleTypeConjunction> : IMembersThat<TRuleTypeConjunction>
    {
        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction ArePrivate();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction ArePublic();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreProtected();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreProtectedInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction ArePrivateProtected();

        TRuleTypeConjunction HaveGetter();
        TRuleTypeConjunction HaveSetter();
        TRuleTypeConjunction HavePrivateSetter();
        TRuleTypeConjunction HavePublicSetter();
        TRuleTypeConjunction HaveProtectedSetter();
        TRuleTypeConjunction HaveInternalSetter();
        TRuleTypeConjunction HaveProtectedInternalSetter();
        TRuleTypeConjunction HavePrivateProtectedSetter();
        TRuleTypeConjunction AreVirtual();


        //Negations


        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreNotPrivate();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreNotPublic();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreNotProtected();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreNotInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreNotProtectedInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new TRuleTypeConjunction AreNotPrivateProtected();

        TRuleTypeConjunction HaveNoGetter();
        TRuleTypeConjunction HaveNoSetter();
        TRuleTypeConjunction DoNotHavePrivateSetter();
        TRuleTypeConjunction DoNotHavePublicSetter();
        TRuleTypeConjunction DoNotHaveProtectedSetter();
        TRuleTypeConjunction DoNotHaveInternalSetter();
        TRuleTypeConjunction DoNotHaveProtectedInternalSetter();
        TRuleTypeConjunction DoNotHavePrivateProtectedSetter();
        TRuleTypeConjunction AreNotVirtual();
    }
}