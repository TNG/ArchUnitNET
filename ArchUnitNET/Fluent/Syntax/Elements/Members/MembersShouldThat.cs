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
        protected MembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            ObjectProvider<TReferenceType> referenceObjectProvider) :
            base(ruleCreator, referenceObjectProvider)
        {
        }

        public MembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.Members as ObjectProvider<TReferenceType>)
        {
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.HaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.HaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.HaveMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.HaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.HaveFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.HaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.DoNotHaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.DoNotHaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.DoNotHaveMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.DoNotHaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.DoNotHaveFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                MembersFilterDefinition<TReferenceType>.DoNotHaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}