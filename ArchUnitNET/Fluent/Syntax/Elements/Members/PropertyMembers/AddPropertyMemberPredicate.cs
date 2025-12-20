using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public abstract class AddPropertyMemberPredicate<TNextElement>
        : AddMemberPredicate<TNextElement, PropertyMember>,
            IAddPropertyMemberPredicate<TNextElement, PropertyMember>
    {
        internal AddPropertyMemberPredicate(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<PropertyMember> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        // csharpier-ignore-start
        public TNextElement HaveGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveGetter());
        public TNextElement HavePrivateGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HavePrivateGetter());
        public TNextElement HavePublicGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HavePublicGetter());
        public TNextElement HaveProtectedGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveProtectedGetter());
        public TNextElement HaveInternalGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveInternalGetter());
        public TNextElement HaveProtectedInternalGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveProtectedInternalGetter());
        public TNextElement HavePrivateProtectedGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HavePrivateProtectedGetter());

        public TNextElement HaveSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveSetter());
        public TNextElement HavePrivateSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HavePrivateSetter());
        public TNextElement HavePublicSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HavePublicSetter());
        public TNextElement HaveProtectedSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveProtectedSetter());
        public TNextElement HaveInternalSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveInternalSetter());
        public TNextElement HaveProtectedInternalSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveProtectedInternalSetter());
        public TNextElement HavePrivateProtectedSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HavePrivateProtectedSetter());
        public TNextElement HaveInitOnlySetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveInitSetter());

        public TNextElement AreVirtual() => CreateNextElement(PropertyMemberPredicatesDefinition.AreVirtual());

        //Negations

        public TNextElement HaveNoGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveNoGetter());
        public TNextElement DoNotHavePrivateGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHavePrivateGetter());
        public TNextElement DoNotHavePublicGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHavePublicGetter());
        public TNextElement DoNotHaveProtectedGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveProtectedGetter());
        public TNextElement DoNotHaveInternalGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveInternalGetter());
        public TNextElement DoNotHaveProtectedInternalGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveProtectedInternalGetter());
        public TNextElement DoNotHavePrivateProtectedGetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHavePrivateProtectedGetter());

        public TNextElement HaveNoSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.HaveNoSetter());
        public TNextElement DoNotHavePrivateSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHavePrivateSetter());
        public TNextElement DoNotHavePublicSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHavePublicSetter());
        public TNextElement DoNotHaveProtectedSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveProtectedSetter());
        public TNextElement DoNotHaveInternalSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveInternalSetter());
        public TNextElement DoNotHaveProtectedInternalSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveProtectedInternalSetter());
        public TNextElement DoNotHavePrivateProtectedSetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHavePrivateProtectedSetter());
        public TNextElement DoNotHaveInitOnlySetter() => CreateNextElement(PropertyMemberPredicatesDefinition.DoNotHaveInitSetter());

        public TNextElement AreNotVirtual() => CreateNextElement(PropertyMemberPredicatesDefinition.AreNotVirtual());

        // csharpier-ignore-end
    }
}
