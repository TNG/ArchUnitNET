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
            _ruleCreator.AddSimpleCondition(member => member.Visibility != NotAccessible, "have getter",
                "has no getter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != NotAccessible, "have setter",
                "has no setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Private, "have private setter",
                "does not have private setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePublicSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Public, "have public setter",
                "does not have public setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Protected, "have protected setter",
                "does not have protected setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == Internal, "have internal setter",
                "does not have internal setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == ProtectedInternal,
                "have protected internal setter", "does not have protected internal setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == PrivateProtected,
                "have private protected setter", "does not have private protected setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddSimpleCondition(member => member.IsVirtual, "be virtual", "is not virtual");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public PropertyMembersShouldConjunction NotHaveGetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.Visibility == NotAccessible, "not have getter",
                "does have a getter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility == NotAccessible, "not have setter",
                "does have a setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Private, "not have private setter",
                "does have a private setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePublicSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Public, "not have public setter",
                "does have a public setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Protected,
                "not have protected setter", "does have a protected setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != Internal, "not have internal setter",
                "does have an internal setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != ProtectedInternal,
                "not have protected internal setter", "does have a protected internal setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddSimpleCondition(member => member.SetterVisibility != PrivateProtected,
                "not have private protected setter", "does have a private protected setter");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddSimpleCondition(member => member.IsVirtual, "not be virtual", "is virtual");
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }
    }
}