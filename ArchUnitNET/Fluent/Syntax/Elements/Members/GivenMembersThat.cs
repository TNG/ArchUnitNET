using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
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
            _ruleCreator.AddObjectFilter(member => member.HasBodyTypeMemberDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => member.HasBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(member => member.HasMethodCallDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => member.HasMethodCallDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(member => member.HasFieldTypeDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => member.HasFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(member => !member.HasBodyTypeMemberDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => !member.HasBodyTypeMemberDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(member => !member.HasMethodCallDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => !member.HasMethodCallDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(member => !member.HasFieldTypeDependencies());
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => !member.HasFieldTypeDependencies(pattern));
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}