using System;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        ITypesThat<TRuleTypeShouldConjunction>
        where TReferenceType : IType
        where TRuleType : ICanBeAnalyzed
    {
        protected TypesShouldThat(IArchRuleCreator<TRuleType> ruleCreator,
            ObjectProvider<TReferenceType> referenceObjectProvider) :
            base(ruleCreator, referenceObjectProvider)
        {
        }

        public TypesShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator,
            ObjectProviderDefinition.Types as ObjectProvider<TReferenceType>)
        {
        }

        public TRuleTypeShouldConjunction Are(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.Are(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.ImplementInterface(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.ImplementInterface(intf));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.ResideInNamespace(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.HaveMethodMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.HaveMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNested()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.AreNested());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.AreNot(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotImplementInterface(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotImplementInterface(intf));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInNamespace(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotResideInNamespace(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotHavePropertyMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotHaveFieldMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotNested()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                TypesFilterDefinition<TReferenceType>.AreNotNested());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}