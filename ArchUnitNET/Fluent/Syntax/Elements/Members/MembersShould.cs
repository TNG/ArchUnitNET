using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        IMembersShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public MembersShould(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveBodyTypeMemberDependencies(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveMethodCallDependencies(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveFieldTypeDependencies(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Complex Conditions

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(MembersConditionDefinition<TRuleType>.HaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(MembersConditionDefinition<TRuleType>.OnlyHaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveBodyTypeMemberDependencies(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveMethodCallDependencies(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveFieldTypeDependencies(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Complex Condition Negations

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(MembersConditionDefinition<TRuleType>.NotHaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}