using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersThat : GivenMembersThat<GivenPropertyMembersConjunction, PropertyMember>,
        IPropertyMemberPredicates<GivenPropertyMembersConjunction>
    {
        public GivenPropertyMembersThat(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenPropertyMembersConjunction HaveGetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveGetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HavePrivateSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePublicSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HavePublicSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveInternalSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveProtectedInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HavePrivateProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreVirtual()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.AreVirtual());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }


        //Negations


        public GivenPropertyMembersConjunction HaveNoGetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveNoGetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveNoSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.HaveNoSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.DoNotHavePrivateSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.DoNotHavePublicSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.DoNotHaveProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.DoNotHaveInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.DoNotHaveProtectedInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.DoNotHavePrivateProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddPredicate(PropertyMemberPredicateDefinition.AreNotVirtual());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }
    }
}