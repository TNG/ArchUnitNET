using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShould : MembersShould<PropertyMembersShouldConjunction, PropertyMember>,
        IComplexPropertyMemberConditions
    {
        public PropertyMembersShould(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public PropertyMembersShouldConjunction HaveGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePrivateSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePublicSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePublicSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveProtectedInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePrivateProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.BeVirtual());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public PropertyMembersShouldConjunction NotHaveGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePrivateSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePublicSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePublicSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveProtectedInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePrivateProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotBeVirtual());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }
    }
}