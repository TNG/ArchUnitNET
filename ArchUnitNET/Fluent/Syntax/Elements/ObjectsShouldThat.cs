using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TRuleTypeShouldConjunction>
        where TReferenceType : ICanBeAnalyzed
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.Are(firstObject, moreObjects));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DependOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DependOn(types));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DependOn(types));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.HaveName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.HaveFullName(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.HaveNameStartingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.HaveNameEndingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.HaveNameContaining(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivate()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.ArePrivate());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePublic()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.ArePublic());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreProtectedInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.ArePrivateProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.AreNot(firstObject, moreObjects));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(types));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(types));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DoNotHaveName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.DoNotHaveFullName(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveNameStartingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveNameEndingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveNameContaining(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivate()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreNotPrivate());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPublic()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreNotPublic());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreNotProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreNotInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreNotProtectedInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectsFilterDefinition<TReferenceType>.AreNotPrivateProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}