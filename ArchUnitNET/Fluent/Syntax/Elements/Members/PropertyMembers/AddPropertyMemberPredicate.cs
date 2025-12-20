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
        public TNextElement HaveGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveGetter());
        public TNextElement HavePrivateGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HavePrivateGetter());
        public TNextElement HavePublicGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HavePublicGetter());
        public TNextElement HaveProtectedGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveProtectedGetter());
        public TNextElement HaveInternalGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveInternalGetter());
        public TNextElement HaveProtectedInternalGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveProtectedInternalGetter());
        public TNextElement HavePrivateProtectedGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HavePrivateProtectedGetter());

        public TNextElement HaveSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveSetter());
        public TNextElement HavePrivateSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HavePrivateSetter());
        public TNextElement HavePublicSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HavePublicSetter());
        public TNextElement HaveProtectedSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveProtectedSetter());
        public TNextElement HaveInternalSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveInternalSetter());
        public TNextElement HaveProtectedInternalSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveProtectedInternalSetter());
        public TNextElement HavePrivateProtectedSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HavePrivateProtectedSetter());
        public TNextElement HaveInitOnlySetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveInitSetter());

        public TNextElement AreVirtual() => CreateNextElement(PropertyMemberPredicateDefinition.AreVirtual());

        //Negations

        public TNextElement HaveNoGetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveNoGetter());
        public TNextElement DoNotHavePrivateGetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHavePrivateGetter());
        public TNextElement DoNotHavePublicGetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHavePublicGetter());
        public TNextElement DoNotHaveProtectedGetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveProtectedGetter());
        public TNextElement DoNotHaveInternalGetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveInternalGetter());
        public TNextElement DoNotHaveProtectedInternalGetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveProtectedInternalGetter());
        public TNextElement DoNotHavePrivateProtectedGetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHavePrivateProtectedGetter());

        public TNextElement HaveNoSetter() => CreateNextElement(PropertyMemberPredicateDefinition.HaveNoSetter());
        public TNextElement DoNotHavePrivateSetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHavePrivateSetter());
        public TNextElement DoNotHavePublicSetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHavePublicSetter());
        public TNextElement DoNotHaveProtectedSetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveProtectedSetter());
        public TNextElement DoNotHaveInternalSetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveInternalSetter());
        public TNextElement DoNotHaveProtectedInternalSetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveProtectedInternalSetter());
        public TNextElement DoNotHavePrivateProtectedSetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHavePrivateProtectedSetter());
        public TNextElement DoNotHaveInitOnlySetter() => CreateNextElement(PropertyMemberPredicateDefinition.DoNotHaveInitSetter());

        public TNextElement AreNotVirtual() => CreateNextElement(PropertyMemberPredicateDefinition.AreNotVirtual());

        // csharpier-ignore-end
    }
}
