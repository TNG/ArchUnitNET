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
    public class ShouldRelateToMembersThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>
        : ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
            IMemberPredicates<TRuleTypeShouldConjunction, TReferenceType>
        where TReferenceType : IMember
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public ShouldRelateToMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShouldConjunction AreDeclaredIn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreStatic()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreStatic()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreReadOnly()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreReadOnly()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreImmutable()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreImmutable()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TRuleTypeShouldConjunction AreNotDeclaredIn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(types)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotStatic()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotStatic()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotReadOnly()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotReadOnly()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotImmutable()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotImmutable()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
