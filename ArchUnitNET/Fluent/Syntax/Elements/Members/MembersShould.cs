//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        IComplexMemberConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public MembersShould(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.BeDeclaredIn(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.BeDeclaredIn(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Conditions

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> BeDeclaredInTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ArchRuleDefinition.Types(),
                MemberConditionsDefinition<TRuleType>.BeDeclaredInTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotBeDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction NotBeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Condition Negations

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotBeDeclaredInTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ArchRuleDefinition.Types(),
                MemberConditionsDefinition<TRuleType>.NotBeDeclaredInTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }
    }
}