using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShould : MembersShould<PropertyMembersShouldConjunction, PropertyMember>,
        IPropertyMemberConditions
    {
        public PropertyMembersShould(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public PropertyMembersShouldConjunction HaveGetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HaveGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HaveSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HavePrivateSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePublicSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HavePublicSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HaveProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HaveInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HaveProtectedInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.HavePrivateProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.BeVirtual());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public PropertyMembersShouldConjunction NotHaveGetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHaveGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHaveSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHavePrivateSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePublicSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHavePublicSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHaveProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHaveInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHaveProtectedInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotHavePrivateProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(PropertyMembersConditionDefinition.NotBeVirtual());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }
    }
}