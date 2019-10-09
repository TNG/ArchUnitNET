using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IPropertyMemberConditions : IMemberConditions<PropertyMembersShouldConjunction, PropertyMember>
    {
        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction BePrivate();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction BePublic();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction BeProtected();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction BeInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction BeProtectedInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction BePrivateProtected();

        PropertyMembersShouldConjunction HaveGetter();
        PropertyMembersShouldConjunction HaveSetter();
        PropertyMembersShouldConjunction HavePrivateSetter();
        PropertyMembersShouldConjunction HavePublicSetter();
        PropertyMembersShouldConjunction HaveProtectedSetter();
        PropertyMembersShouldConjunction HaveInternalSetter();
        PropertyMembersShouldConjunction HaveProtectedInternalSetter();
        PropertyMembersShouldConjunction HavePrivateProtectedSetter();
        PropertyMembersShouldConjunction BeVirtual();


        //Negations


        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction NotBePrivate();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction NotBePublic();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction NotBeProtected();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction NotBeInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction NotBeProtectedInternal();

        /// <summary>
        ///     Refers to the getter of the property member.
        /// </summary>
        /// <returns></returns>
        new PropertyMembersShouldConjunction NotBePrivateProtected();

        PropertyMembersShouldConjunction NotHaveGetter();
        PropertyMembersShouldConjunction NotHaveSetter();
        PropertyMembersShouldConjunction NotHavePrivateSetter();
        PropertyMembersShouldConjunction NotHavePublicSetter();
        PropertyMembersShouldConjunction NotHaveProtectedSetter();
        PropertyMembersShouldConjunction NotHaveInternalSetter();
        PropertyMembersShouldConjunction NotHaveProtectedInternalSetter();
        PropertyMembersShouldConjunction NotHavePrivateProtectedSetter();
        PropertyMembersShouldConjunction NotBeVirtual();
    }
}