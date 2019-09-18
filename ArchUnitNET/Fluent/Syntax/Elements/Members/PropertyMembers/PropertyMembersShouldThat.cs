using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShouldThat<TRuleTypeShouldConjunction, TRuleType> :
        MembersShouldThat<TRuleTypeShouldConjunction, PropertyMember, TRuleType>,
        IPropertyMembersThat<TRuleTypeShouldConjunction>
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        public PropertyMembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            architecture => architecture.PropertyMembers)
        {
        }

        public TRuleTypeShouldConjunction HaveGetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.Visibility != NotAccessible, "have getter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != NotAccessible, "have setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == Private, "have private setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == Public, "have public setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == Protected, "have protected setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == Internal, "have internal setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == ProtectedInternal, "have protected internal setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == PrivateProtected, "have private protected setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, member => member.IsVirtual, "are virtual");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction HaveNoGetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.Visibility == NotAccessible, "have no getter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNoSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility == NotAccessible, "have no setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != Private, "do not have private setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != Public, "do not have public setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != Protected, "do not have protected setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != Internal, "do not have internal setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != ProtectedInternal, "do not have protected internal setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                member => member.SetterVisibility != PrivateProtected, "do not have private protected setter");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider, member => !member.IsVirtual,
                "are not virtual");
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}