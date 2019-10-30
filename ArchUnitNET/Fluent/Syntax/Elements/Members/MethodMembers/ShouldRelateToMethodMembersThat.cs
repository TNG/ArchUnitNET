//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class ShouldRelateToMethodMembersThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToMembersThat<TRuleTypeShouldConjunction, MethodMember, TRuleType>,
        IMethodMemberPredicates<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToMethodMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction AreConstructors()
        {
            _ruleCreator.ContinueComplexCondition(MethodMemberPredicatesDefinition.AreConstructors());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMemberPredicatesDefinition.AreVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreCalledBy(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNoConstructors()
        {
            _ruleCreator.ContinueComplexCondition(MethodMemberPredicatesDefinition.AreNoConstructors());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(MethodMemberPredicatesDefinition.AreNotVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.AreNotCalledBy(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}