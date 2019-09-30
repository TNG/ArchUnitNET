using System;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        ITypesThat<TRuleTypeShouldConjunction>
        where TReferenceType : IType
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public TypesShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Are(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.Are(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.ImplementInterface(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.ImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.ResideInNamespace(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.HaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.HaveMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNested()
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.AreNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.AreNot(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                TypesFilterDefinition<TReferenceType>.DoNotImplementInterface(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.DoNotImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInNamespace(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                TypesFilterDefinition<TReferenceType>.DoNotResideInNamespace(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypesFilterDefinition<TReferenceType>.DoNotHavePropertyMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypesFilterDefinition<TReferenceType>.DoNotHaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypesFilterDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypesFilterDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotNested()
        {
            _ruleCreator.ContinueComplexCondition(TypesFilterDefinition<TReferenceType>.AreNotNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}