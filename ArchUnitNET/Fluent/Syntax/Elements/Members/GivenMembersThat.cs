using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersThat<TGivenRuleTypeConjunction, TRuleType> :
        GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>,
        IMemberPredicates<TGivenRuleTypeConjunction>
        where TRuleType : IMember
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction AreDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.AreDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveBodyTypeMemberDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.HaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveMethodCallDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.HaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveFieldTypeDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.HaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction AreNotDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.AreNotDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreNotDeclaredIn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreNotDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreNotDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.AreNotDeclaredIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveBodyTypeMemberDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveMethodCallDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.DoNotHaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveFieldTypeDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.DoNotHaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}