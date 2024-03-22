//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class ShouldRelateToPropertyMembersThat<TRuleTypeShouldConjunction, TRuleType>
        : ShouldRelateToMembersThat<TRuleTypeShouldConjunction, PropertyMember, TRuleType>,
            IPropertyMemberPredicates<TRuleTypeShouldConjunction, PropertyMember>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToPropertyMembersThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShouldConjunction HaveGetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMemberPredicateDefinition.HaveGetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HavePrivateGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePublicGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HavePublicGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveProtectedGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInternalGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveInternalGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedInternalGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveProtectedInternalGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateProtectedGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HavePrivateProtectedGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMemberPredicateDefinition.HaveSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HavePrivateSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HavePublicSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveProtectedSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveInternalSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveProtectedInternalSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HavePrivateProtectedSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInitOnlySetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.HaveInitSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMemberPredicateDefinition.AreVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TRuleTypeShouldConjunction HaveNoGetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMemberPredicateDefinition.HaveNoGetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHavePrivateGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePublicGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHavePublicGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveProtectedGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInternalGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveInternalGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedInternalGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveProtectedInternalGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateProtectedGetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHavePrivateProtectedGetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNoSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMemberPredicateDefinition.HaveNoSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHavePrivateSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHavePublicSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveProtectedSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveInternalSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveProtectedInternalSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHavePrivateProtectedSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInitOnlySetter()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.DoNotHaveInitSetter()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(
                PropertyMemberPredicateDefinition.AreNotVirtual()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
