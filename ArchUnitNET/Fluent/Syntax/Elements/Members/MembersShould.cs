using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public class MembersShould<TRuleTypeShouldConjunction, TRuleType> :
        ObjectsShould<TRuleTypeShouldConjunction, TRuleType>,
        IMembersShould<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public MembersShould(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddSimpleCondition(member => member.HasBodyTypeMemberDependencies(),
                "have body type member dependencies");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddSimpleCondition(member => member.HasBodyTypeMemberDependencies(pattern),
                "have body type member dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies()
        {
            _ruleCreator.AddSimpleCondition(member => member.HasMethodCallDependencies(),
                "have method call dependencies");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddSimpleCondition(member => member.HasMethodCallDependencies(pattern),
                "have method call dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies()
        {
            _ruleCreator.AddSimpleCondition(member => member.HasFieldTypeDependencies(),
                "have field type dependencies");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddSimpleCondition(member => member.HasFieldTypeDependencies(pattern),
                "have field type dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies()
        {
            _ruleCreator.AddSimpleCondition(member => !member.HasBodyTypeMemberDependencies(),
                "not have body type member dependencies");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies(string pattern)
        {
            _ruleCreator.AddSimpleCondition(member => !member.HasBodyTypeMemberDependencies(pattern),
                "not have body type member dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies()
        {
            _ruleCreator.AddSimpleCondition(member => !member.HasMethodCallDependencies(),
                "not have method call dependencies");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveMethodCallDependencies(string pattern)
        {
            _ruleCreator.AddSimpleCondition(member => !member.HasMethodCallDependencies(pattern),
                "not have method call dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies()
        {
            _ruleCreator.AddSimpleCondition(member => !member.HasFieldTypeDependencies(),
                "not have field type dependencies");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFieldTypeDependencies(string pattern)
        {
            _ruleCreator.AddSimpleCondition(member => !member.HasFieldTypeDependencies(pattern),
                "not have field type dependencies \"" + pattern + "\"");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}