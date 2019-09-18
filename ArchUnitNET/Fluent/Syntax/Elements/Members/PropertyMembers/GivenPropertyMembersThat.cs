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
            _ruleCreator.AddObjectFilter(member => member.Visibility != NotAccessible, "have getter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.SetterVisibility != NotAccessible, "have setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Private, "have private setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Public, "have public setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Protected, "have protected setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == Internal, "have internal setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == ProtectedInternal,
                "have protected internal setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == PrivateProtected,
                "have private protected setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreVirtual()
        {
            _ruleCreator.AddObjectFilter(member => member.IsVirtual, "are virtual");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }


        //Negations


        public GivenPropertyMembersConjunction HaveNoGetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility == NotAccessible, "have no getter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction HaveNoSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.SetterVisibility == NotAccessible, "have no setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Private, "do not have private setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Public, "do not have public setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Protected, "do not have protected setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != Internal, "do not have internal setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != ProtectedInternal,
                "do not have protected internal setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddObjectFilter(member => member.Visibility != PrivateProtected,
                "do not have private protected setter");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }

        public GivenPropertyMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddObjectFilter(member => !member.IsVirtual, "are not virtual");
            return new GivenPropertyMembersConjunction(_ruleCreator);
        }
    }
}