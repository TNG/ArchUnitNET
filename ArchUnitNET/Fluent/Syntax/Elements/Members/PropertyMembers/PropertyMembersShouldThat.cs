using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        MembersShouldThat<TRuleTypeShouldConjunction, PropertyMember, TRuleType>,
        IPropertyMembersThat<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public PropertyMembersShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction HaveGetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveGetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HavePrivateSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HavePublicSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveProtectedSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveInternalSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveProtectedInternalSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HavePrivateProtectedSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.AreVirtual());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction HaveNoGetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveNoGetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNoSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveNoSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHavePrivateSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHavePublicSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHaveProtectedSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHaveInternalSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHaveProtectedInternalSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHavePrivateProtectedSetter());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.AreNotVirtual());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}