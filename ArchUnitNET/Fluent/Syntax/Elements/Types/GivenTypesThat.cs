using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types

{
    public class GivenTypesThat<TGivenRuleTypeConjunction, TRuleType> :
        GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>, ITypesThat<TGivenRuleTypeConjunction>
        where TRuleType : IType
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenTypesThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction Are(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.Are(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction Are(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.Are(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableToTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                TypesFilterDefinition<TRuleType>.AreAssignableToTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreAssignableTo(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreAssignableTo(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(ObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ImplementInterfaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                TypesFilterDefinition<TRuleType>.ImplementInterfaceWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInNamespaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                TypesFilterDefinition<TRuleType>.ResideInNamespaceWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HavePropertyMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HaveFieldMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HaveMethodMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.HaveMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNested()
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNested());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction AreNot(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNot(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNot(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNot(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableToTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                TypesFilterDefinition<TRuleType>.AreNotAssignableToTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotAssignableTo(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotAssignableTo(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(ObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotImplementInterfaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                TypesFilterDefinition<TRuleType>.DoNotImplementInterfaceWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInNamespaceWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                TypesFilterDefinition<TRuleType>.DoNotResideInNamespaceWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHavePropertyMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHaveFieldMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHaveMethodMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.DoNotHaveMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotNested()
        {
            _ruleCreator.AddObjectFilter(TypesFilterDefinition<TRuleType>.AreNotNested());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}