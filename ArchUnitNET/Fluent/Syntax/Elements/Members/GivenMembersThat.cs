//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersThat<TGivenRuleTypeConjunction, TRuleType>
        : GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>,
            IMemberPredicates<TGivenRuleTypeConjunction, TRuleType>
        where TRuleType : IMember
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        public TGivenRuleTypeConjunction AreDeclaredIn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(pattern, useRegularExpressions)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        public TGivenRuleTypeConjunction AreDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(patterns, useRegularExpressions)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes)
            );
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

        public TGivenRuleTypeConjunction AreStatic()
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreStatic());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreReadOnly()
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreReadOnly());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreImmutable()
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreImmutable());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        //Negations

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        public TGivenRuleTypeConjunction AreNotDeclaredIn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        public TGivenRuleTypeConjunction AreNotDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(
                MemberPredicatesDefinition<TRuleType>.AreNotDeclaredIn(types)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotStatic()
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotStatic());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotReadOnly()
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotReadOnly());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotImmutable()
        {
            _ruleCreator.AddPredicate(MemberPredicatesDefinition<TRuleType>.AreNotImmutable());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}
