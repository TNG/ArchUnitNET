using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        IComplexMemberConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public MembersShould(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction BeDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.BeDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.BeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.HaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>
                .HaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.HaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.HaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.HaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.HaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotBeDeclaredInTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                MemberConditionsDefinition<TRuleType>.NotBeDeclaredInTypesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotBeDeclaredIn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotHaveBodyTypeMemberDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>
                .NotHaveBodyTypeMemberDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies()
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotHaveMethodCallDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>
                .NotHaveMethodCallDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>.NotHaveFieldTypeDependencies());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(MemberConditionsDefinition<TRuleType>
                .NotHaveFieldTypeDependenciesWithFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}