using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        ITypesShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IType
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public TypesShould(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Be(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.Be(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.Be(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableToTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypesConditionDefinition<TRuleType>.BeAssignableToTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.BeAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.BeAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(ObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.BeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.BeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.BeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterfaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypesConditionDefinition<TRuleType>.ImplementInterfaceWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInNamespaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypesConditionDefinition<TRuleType>.ResideInNamespaceWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.HavePropertyMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.HaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.HaveMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeNested()
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.BeNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotBe(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBe(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBe(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableToTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypesConditionDefinition<TRuleType>.NotBeAssignableToTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBeAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBeAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(ObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBeAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction NotImplementInterfaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypesConditionDefinition<TRuleType>.NotImplementInterfaceWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotResideInNamespaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                TypesConditionDefinition<TRuleType>.NotResideInNamespaceWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotHavePropertyMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotHaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMemberWithName(string name)
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotHaveMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeNested()
        {
            _ruleCreator.AddCondition(TypesConditionDefinition<TRuleType>.NotBeNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}