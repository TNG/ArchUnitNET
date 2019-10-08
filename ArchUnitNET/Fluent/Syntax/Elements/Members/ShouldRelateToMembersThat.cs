using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class ShouldRelateToMembersThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        IMembersThat<TRuleTypeShouldConjunction>
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
                MembersFilterDefinition<TReferenceType>.AreDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.AreDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.AreDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.AreDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.HaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.HaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.HaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNotDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.AreNotDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.AreNotDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.AreNotDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>.AreNotDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>
                .DoNotHaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>
                    .DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.ContinueComplexCondition(MembersFilterDefinition<TReferenceType>
                .DoNotHaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                MembersFilterDefinition<TReferenceType>.DoNotHaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}