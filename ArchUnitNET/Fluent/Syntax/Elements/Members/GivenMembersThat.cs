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
            _ruleCreator.AddObjectFilter(member => member.HasBodyTypeMemberDependencies(),
                "have body type member dependencies");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => member.HasBodyTypeMemberDependencies(pattern),
                "have body type member dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(member => member.HasMethodCallDependencies(), "have method call dependencies");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => member.HasMethodCallDependencies(pattern),
                "have method call dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(member => member.HasFieldTypeDependencies(), "have field type dependencies");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => member.HasFieldTypeDependencies(pattern),
                "have field type dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddObjectFilter(member => !member.HasBodyTypeMemberDependencies(),
                "do not have body type member dependencies");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => !member.HasBodyTypeMemberDependencies(pattern),
                "do not have body type member dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies()
        {
            _ruleCreator.AddObjectFilter(member => !member.HasMethodCallDependencies(),
                "do not have method call dependencies");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => !member.HasMethodCallDependencies(pattern),
                "do not have method call dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddObjectFilter(member => !member.HasFieldTypeDependencies(),
                "do not have field type dependencies");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddObjectFilter(member => !member.HasFieldTypeDependencies(pattern),
                "do not have field type dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}