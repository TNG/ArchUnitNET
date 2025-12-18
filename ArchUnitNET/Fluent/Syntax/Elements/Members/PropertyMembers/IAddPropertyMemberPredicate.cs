using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public interface IAddPropertyMemberPredicate<out TNextElement, TRuleType>
        : IAddMemberPredicate<TNextElement, TRuleType>
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
        TNextElement AreVirtual();

        //Negations

        TNextElement HaveNoGetter();
        TNextElement DoNotHavePrivateGetter();
        TNextElement DoNotHavePublicGetter();
        TNextElement DoNotHaveProtectedGetter();
        TNextElement DoNotHaveInternalGetter();
        TNextElement DoNotHaveProtectedInternalGetter();
        TNextElement DoNotHavePrivateProtectedGetter();
        TNextElement HaveNoSetter();
        TNextElement DoNotHavePrivateSetter();
        TNextElement DoNotHavePublicSetter();
        TNextElement DoNotHaveProtectedSetter();
        TNextElement DoNotHaveInternalSetter();
        TNextElement DoNotHaveProtectedInternalSetter();
        TNextElement DoNotHavePrivateProtectedSetter();
        TNextElement DoNotHaveInitOnlySetter();
        TNextElement AreNotVirtual();
    }
}
