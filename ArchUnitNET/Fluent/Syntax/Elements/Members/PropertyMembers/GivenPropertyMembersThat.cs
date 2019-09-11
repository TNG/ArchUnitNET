using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class GivenPropertyMembersThat : GivenMembersThat<GivenPropertyMembersConjunction, PropertyMember>,
        IPropertyMembersThat<GivenPropertyMembersConjunction>
    {
        public GivenPropertyMembersThat(ArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenPropertyMembersConjunction HaveGetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != NotAccessible);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.SetterVisibility != NotAccessible);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Private);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Public);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Protected);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Internal);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == ProtectedInternal);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == PrivateProtected);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreVirtual()
        {
            _ruleCreator.AddObjectFilter(member => member.IsVirtual);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }


        //Negations


        public GivenPropertyMembersConjunction HaveNoGetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == NotAccessible);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveNoSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.SetterVisibility == NotAccessible);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Private);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Public);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Protected);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Internal);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != ProtectedInternal);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != PrivateProtected);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddObjectFilter(member => !member.IsVirtual);
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }
    }
}