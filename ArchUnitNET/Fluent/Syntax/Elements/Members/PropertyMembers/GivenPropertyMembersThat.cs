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
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveGetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HavePrivateSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HavePublicSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveProtectedInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HavePrivateProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreVirtual()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.AreVirtual());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }


        //Negations


        public GivenPropertyMembersConjunction HaveNoGetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveNoGetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveNoSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.HaveNoSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.DoNotHavePrivateSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.DoNotHavePublicSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.DoNotHaveProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.DoNotHaveInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.DoNotHaveProtectedInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.DoNotHavePrivateProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddObjectFilter(PropertyMemberPredicateDefinition.AreNotVirtual());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }
    }
}