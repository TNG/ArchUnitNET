using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShould : MembersShould<PropertyMembersShouldConjunction, PropertyMember>,
        IPropertyMembersShould
    {
        public PropertyMembersShould(ArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public PropertyMembersShouldConjunction HaveGetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.Visibility != NotAccessible);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != NotAccessible);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Private);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePublicSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Public);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Protected);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Internal);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == ProtectedInternal);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == PrivateProtected);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddSimpleCondition(member => member.IsVirtual);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public PropertyMembersShouldConjunction NotHaveGetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.Visibility == NotAccessible);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == NotAccessible);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Private);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePublicSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Public);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Protected);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Internal);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != ProtectedInternal);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != PrivateProtected);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddSimpleCondition(member => member.IsVirtual);
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }
    }
}