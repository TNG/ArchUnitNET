using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public abstract class AddPropertyMemberCondition<TNextElement>
        : AddMemberCondition<TNextElement, PropertyMember>,
            IAddPropertyMemberCondition<TNextElement, PropertyMember>
    {
        internal AddPropertyMemberCondition(IArchRuleCreator<PropertyMember> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TNextElement HaveGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveGetter());
        public TNextElement HavePrivateGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HavePrivateGetter());
        public TNextElement HavePublicGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HavePublicGetter());
        public TNextElement HaveProtectedGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveProtectedGetter());
        public TNextElement HaveInternalGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveInternalGetter());
        public TNextElement HaveProtectedInternalGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveProtectedInternalGetter());
        public TNextElement HavePrivateProtectedGetter() => CreateNextElement(PropertyMemberConditionsDefinition.HavePrivateProtectedGetter());

        public TNextElement HaveSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveSetter());
        public TNextElement HavePrivateSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HavePrivateSetter());
        public TNextElement HavePublicSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HavePublicSetter());
        public TNextElement HaveProtectedSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveProtectedSetter());
        public TNextElement HaveInternalSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveInternalSetter());
        public TNextElement HaveProtectedInternalSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveProtectedInternalSetter());
        public TNextElement HavePrivateProtectedSetter() => CreateNextElement(PropertyMemberConditionsDefinition.HavePrivateProtectedSetter());
        public TNextElement HaveInitOnlySetter() => CreateNextElement(PropertyMemberConditionsDefinition.HaveInitSetter());

        public TNextElement BeVirtual() => CreateNextElement(PropertyMemberConditionsDefinition.BeVirtual());

        //Negations

        public TNextElement NotHaveGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveGetter());
        public TNextElement NotHavePrivateGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHavePrivateGetter());
        public TNextElement NotHavePublicGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHavePublicGetter());
        public TNextElement NotHaveProtectedGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveProtectedGetter());
        public TNextElement NotHaveInternalGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveInternalGetter());
        public TNextElement NotHaveProtectedInternalGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveProtectedInternalGetter());
        public TNextElement NotHavePrivateProtectedGetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHavePrivateProtectedGetter());

        public TNextElement NotHaveSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveSetter());
        public TNextElement NotHavePrivateSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHavePrivateSetter());
        public TNextElement NotHavePublicSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHavePublicSetter());
        public TNextElement NotHaveProtectedSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveProtectedSetter());
        public TNextElement NotHaveInternalSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveInternalSetter());
        public TNextElement NotHaveProtectedInternalSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveProtectedInternalSetter());
        public TNextElement NotHavePrivateProtectedSetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHavePrivateProtectedSetter());
        public TNextElement NotHaveInitOnlySetter() => CreateNextElement(PropertyMemberConditionsDefinition.NotHaveInitSetter());

        public TNextElement NotBeVirtual() => CreateNextElement(PropertyMemberConditionsDefinition.NotBeVirtual());
        // csharpier-ignore-end
    }
}
