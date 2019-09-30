using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class ShouldRelateToPropertyMembersThat<TRuleTypeShouldConjunction, TRuleType> :
        ShouldRelateToMembersThat<TRuleTypeShouldConjunction, PropertyMember, TRuleType>,
        IPropertyMembersThat<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public ShouldRelateToPropertyMembersThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction HaveGetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveGetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HavePrivateSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HavePublicSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveProtectedSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveInternalSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveProtectedInternalSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HavePrivateProtectedSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.AreVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction HaveNoGetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveNoGetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNoSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.HaveNoSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHavePrivateSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHavePublicSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHaveProtectedSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHaveInternalSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHaveProtectedInternalSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.DoNotHavePrivateProtectedSetter());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(PropertyMembersFilterDefinition.AreNotVirtual());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}