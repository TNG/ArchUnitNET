using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using static ArchUnitNET.Domain.Visibility;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TRuleTypeShouldConjunction>
        where TReferenceType : ICanBeAnalyzed
        where TRuleType : ICanBeAnalyzed
    {
        protected readonly Func<Architecture, IEnumerable<TReferenceType>> _referenceObjectProvider;
        protected readonly Func<TRuleType, TReferenceType, bool> _relationCondition;

        // ReSharper disable once MemberCanBeProtected.Global
        public ObjectsShouldThat(ArchRuleCreator<TRuleType> ruleCreator,
            Func<Architecture, IEnumerable<TReferenceType>> referenceObjectProvider,
            Func<TRuleType, TReferenceType, bool> relationCondition) : base(ruleCreator)
        {
            _referenceObjectProvider = referenceObjectProvider;
            _relationCondition = relationCondition;
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.DependsOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Name.Equals(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.NameStartsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.NameEndsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.NameContains(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivate()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility == Private);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePublic()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility == Public);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtected()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility == Protected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreInternal()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility == Internal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtectedInternal()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility == ProtectedInternal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivateProtected()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility == PrivateProtected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction DoNotDependOn(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => !obj.DependsOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveName(string name)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => !obj.Name.Equals(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => !obj.NameStartsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => !obj.NameEndsWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => !obj.NameContains(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivate()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility != Private);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPublic()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility != Public);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtected()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility != Protected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotInternal()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility != Internal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtectedInternal()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility != ProtectedInternal);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivateProtected()
        {
            _ruleCreator.AddComplexCondition(_referenceObjectProvider, _relationCondition,
                obj => obj.Visibility != PrivateProtected);
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}