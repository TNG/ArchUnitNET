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
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreConstructors());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreVirtual()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreVirtual());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        //Negations


        public GivenMethodMembersConjunction AreNoConstructors()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNoConstructors());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotVirtual());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }
    }
}