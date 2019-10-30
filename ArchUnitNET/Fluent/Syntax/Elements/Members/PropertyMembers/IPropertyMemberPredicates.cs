namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IPropertyMemberPredicates<out TRuleTypeConjunction> : IMemberPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction HaveGetter();
        TRuleTypeConjunction HavePrivateGetter();
        TRuleTypeConjunction HavePublicGetter();
        TRuleTypeConjunction HaveProtectedGetter();
        TRuleTypeConjunction HaveInternalGetter();
        TRuleTypeConjunction HaveProtectedInternalGetter();
        TRuleTypeConjunction HavePrivateProtectedGetter();
        TRuleTypeConjunction HaveSetter();
        TRuleTypeConjunction HavePrivateSetter();
        TRuleTypeConjunction HavePublicSetter();
        TRuleTypeConjunction HaveProtectedSetter();
        TRuleTypeConjunction HaveInternalSetter();
        TRuleTypeConjunction HaveProtectedInternalSetter();
        TRuleTypeConjunction HavePrivateProtectedSetter();
        TRuleTypeConjunction AreVirtual();


        //Negations


        TRuleTypeConjunction HaveNoGetter();
        TRuleTypeConjunction DoNotHavePrivateGetter();
        TRuleTypeConjunction DoNotHavePublicGetter();
        TRuleTypeConjunction DoNotHaveProtectedGetter();
        TRuleTypeConjunction DoNotHaveInternalGetter();
        TRuleTypeConjunction DoNotHaveProtectedInternalGetter();
        TRuleTypeConjunction DoNotHavePrivateProtectedGetter();
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