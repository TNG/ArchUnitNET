using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        ITypePredicates<TRuleTypeShouldConjunction>
        where TReferenceType : IType
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public ShouldRelateToTypesThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Are(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.Are(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Are(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.Are(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ImplementInterface(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction ResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ResideInNamespace(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ResideInAssembly(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.HaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.HaveMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNested()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNot(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNot(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNot(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotImplementInterface(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotResideInNamespace(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotResideInAssembly(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHavePropertyMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotNested()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}