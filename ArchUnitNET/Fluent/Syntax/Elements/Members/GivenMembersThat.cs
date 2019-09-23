using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class GivenMembersThat<TGivenRuleTypeConjunction, TRuleType> :
        GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>,
        IMembersThat<TGivenRuleTypeConjunction>
        where TRuleType : IMember
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenMembersThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveMethodCallDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveFieldTypeDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.HaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveBodyTypeMemberDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                MembersFilterDefinition<TRuleType>.DoNotHaveBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveMethodCallDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveMethodCallDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveFieldTypeDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(MembersFilterDefinition<TRuleType>.DoNotHaveFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}