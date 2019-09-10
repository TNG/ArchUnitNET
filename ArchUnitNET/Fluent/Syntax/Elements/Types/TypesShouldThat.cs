using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class TypesShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        ITypesThat<TRuleTypeShouldConjunction>
        where TReferenceType : IType
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public TypesShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<Architecture, IEnumerable<TReferenceType>> referenceObjectProvider,
            Func<TRuleType, TReferenceType, bool> relationCondition) :
            base(ruleCreator, referenceObjectProvider, relationCondition)
        {
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => type.ImplementsInterface(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInNamespace(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => type.ResidesInNamespace(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => type.HasPropertyMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => type.HasFieldMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => type.HasMethodMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => type.HasMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNested()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, type => type.IsNested);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction DoNotImplementInterface(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => !type.ImplementsInterface(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInNamespace(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => !type.ResidesInNamespace(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => !type.HasPropertyMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => !type.HasFieldMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => !type.HasMethodMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                type => !type.HasMemberWithName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotNested()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition, type => !type.IsNested);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}