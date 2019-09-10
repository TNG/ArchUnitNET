using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
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
        public MembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<Architecture, IEnumerable<TReferenceType>> referenceObjectProvider,
            Func<TRuleType, TReferenceType, bool> relationCondition) :
            base(ruleCreator, referenceObjectProvider, relationCondition)
        {
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.HasBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.HasBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.HasMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.HasMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.HasFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.HasFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.HasBodyTypeMemberDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.HasBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.HasMethodCallDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.HasMethodCallDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.HasFieldTypeDependencies());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => !member.HasFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}