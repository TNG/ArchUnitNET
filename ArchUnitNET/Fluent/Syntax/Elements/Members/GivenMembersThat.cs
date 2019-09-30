using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersThat<TGivenRuleTypeConjunction, TRuleType> :
        GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>,
        IMembersThat<TGivenRuleTypeConjunction>
        where TRuleType : IMember
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveBodyTypeMemberDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveBodyTypeMemberDependencies(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveMethodCallDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveMethodCallDependencies(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveFieldTypeDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveFieldTypeDependencies(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveBodyTypeMemberDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.DoNotHaveBodyTypeMemberDependencies(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveMethodCallDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveMethodCallDependencies(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveFieldTypeDependencies());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveFieldTypeDependencies(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}