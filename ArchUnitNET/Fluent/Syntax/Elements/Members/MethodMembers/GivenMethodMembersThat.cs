using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersThat : GivenMembersThat<GivenMethodMembersConjunction, MethodMember>,
        IMethodMemberPredicates<GivenMethodMembersConjunction>
    {
        public GivenMethodMembersThat(IArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenMethodMembersConjunction AreConstructors()
        {
            _ruleCreator.AddObjectFilter(MethodMembersFilterDefinition.AreConstructors());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreVirtual()
        {
            _ruleCreator.AddObjectFilter(MethodMembersFilterDefinition.AreVirtual());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        //Negations


        public GivenMethodMembersConjunction AreNoConstructors()
        {
            _ruleCreator.AddObjectFilter(MethodMembersFilterDefinition.AreNoConstructors());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddObjectFilter(MethodMembersFilterDefinition.AreNotVirtual());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }
    }
}