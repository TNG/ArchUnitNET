using System;
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
        public PropertyMembersShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<TRuleType, PropertyMember, bool> relationCondition) : base(ruleCreator,
            architecture => architecture.PropertyMembers, relationCondition)
        {
        }

        public TRuleTypeShouldConjunction HaveGetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.Visibility != NotAccessible);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != NotAccessible);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == Private);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePublicSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == Public);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == Protected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == Internal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == ProtectedInternal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == PrivateProtected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreVirtual()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, member => member.IsVirtual);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction HaveNoGetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.Visibility == NotAccessible);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNoSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility == NotAccessible);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != Private);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePublicSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != Public);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != Protected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveInternalSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != Internal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != ProtectedInternal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                member => member.SetterVisibility != PrivateProtected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotVirtual()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, member => !member.IsVirtual);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}