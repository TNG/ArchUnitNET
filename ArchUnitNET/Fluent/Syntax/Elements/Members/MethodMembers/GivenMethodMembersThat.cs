using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersThat : GivenMembersThat<GivenMethodMembersConjunction, MethodMember>,
        IMethodMembersThat<GivenMethodMembersConjunction>
    {
        public GivenMethodMembersThat(ArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenMethodMembersConjunction AreConstructors()
        {
            _ruleCreator.AddObjectFilter(member => member.IsConstructor(), "are constructors");
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreVirtual()
        {
            _ruleCreator.AddObjectFilter(member => member.IsVirtual, "are virtual");
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        //Negations


        public GivenMethodMembersConjunction AreNoConstructors()
        {
            _ruleCreator.AddObjectFilter(member => !member.IsConstructor(), "are no constructors");
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddObjectFilter(member => !member.IsVirtual, "are not virtual");
            return new GivenMethodMembersConjunction(_ruleCreator);
        }
    }
}