//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersThat<TGivenRuleTypeConjunction, TRuleType> :
        GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>,
        IMemberPredicates<TGivenRuleTypeConjunction, TRuleType>
        where TRuleType : IMember
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TGivenRuleTypeConjunction AreNotDeclaredIn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}