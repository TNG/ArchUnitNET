using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersThat : GivenMembersThat<GivenPropertyMembersConjunction, PropertyMember>,
        IPropertyMembersThat<GivenPropertyMembersConjunction>
    {
        public GivenPropertyMembersThat(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenPropertyMembersConjunction HaveGetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveGetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HavePrivateSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HavePublicSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveProtectedInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HavePrivateProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreVirtual()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.AreVirtual());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }


        //Negations


        public GivenPropertyMembersConjunction HaveNoGetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveNoGetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveNoSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.HaveNoSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.DoNotHavePrivateSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.DoNotHavePublicSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.DoNotHaveProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.DoNotHaveInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.DoNotHaveProtectedInternalSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.DoNotHavePrivateProtectedSetter());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddObjectFilter(PropertyMembersFilterDefinition.AreNotVirtual());
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }
    }
}