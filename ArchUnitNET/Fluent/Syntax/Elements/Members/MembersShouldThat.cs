using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        IMembersThat<TRuleTypeShouldConjunction>
        where TReferenceType : IMember
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public MembersShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }


        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.HaveMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.HaveFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>
                .DoNotHaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>
                .DoNotHaveMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}