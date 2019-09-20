﻿using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        IMembersShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public MembersShould(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.HaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddCondition(MembersConditionDefinition<TRuleType>.NotHaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}