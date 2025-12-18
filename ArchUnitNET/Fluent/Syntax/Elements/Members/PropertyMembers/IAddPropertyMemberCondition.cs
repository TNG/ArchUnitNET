using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IAddPropertyMemberCondition<TNextElement, TRuleType>
        : IAddMemberCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement HaveGetter();
        TNextElement HavePrivateGetter();
        TNextElement HavePublicGetter();
        TNextElement HaveProtectedGetter();
        TNextElement HaveInternalGetter();
        TNextElement HaveProtectedInternalGetter();
        TNextElement HavePrivateProtectedGetter();
        TNextElement HaveSetter();
        TNextElement HavePrivateSetter();
        TNextElement HavePublicSetter();
        TNextElement HaveProtectedSetter();
        TNextElement HaveInternalSetter();
        TNextElement HaveProtectedInternalSetter();
        TNextElement HavePrivateProtectedSetter();
        TNextElement HaveInitOnlySetter();
        TNextElement BeVirtual();

        //Negations

        TNextElement NotHaveGetter();
        TNextElement NotHavePrivateGetter();
        TNextElement NotHavePublicGetter();
        TNextElement NotHaveProtectedGetter();
        TNextElement NotHaveInternalGetter();
        TNextElement NotHaveProtectedInternalGetter();
        TNextElement NotHavePrivateProtectedGetter();
        TNextElement NotHaveSetter();
        TNextElement NotHavePrivateSetter();
        TNextElement NotHavePublicSetter();
        TNextElement NotHaveProtectedSetter();
        TNextElement NotHaveInternalSetter();
        TNextElement NotHaveProtectedInternalSetter();
        TNextElement NotHavePrivateProtectedSetter();
        TNextElement NotHaveInitOnlySetter();
        TNextElement NotBeVirtual();
    }
}
