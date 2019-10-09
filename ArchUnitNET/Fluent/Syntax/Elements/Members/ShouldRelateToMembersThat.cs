using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class ShouldRelateToMembersThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        IMemberPredicates<TRuleTypeShouldConjunction>
        where TReferenceType : IMember
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public ShouldRelateToMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction AreDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredInTypesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredInTypesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>.AreDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>
                    .HaveBodyTypeMemberDependenciesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependenciesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveMethodCallDependenciesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependenciesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.HaveFieldTypeDependenciesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredInTypesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredInTypesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>.AreNotDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>
                .DoNotHaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>
                    .DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>
                    .DoNotHaveBodyTypeMemberDependenciesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MemberPredicatesDefinition<TReferenceType>
                .DoNotHaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>
                    .DoNotHaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependenciesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>
                    .DoNotHaveMethodCallDependenciesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.DoNotHaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>.DoNotHaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependenciesWithFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MemberPredicatesDefinition<TReferenceType>
                    .DoNotHaveFieldTypeDependenciesWithFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}