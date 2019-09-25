using System;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ActivatorHandler;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ObjectsShouldThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TRuleTypeShouldConjunction>
        where TReferenceType : ICanBeAnalyzed
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once InconsistentNaming
        protected readonly ObjectProvider<TReferenceType> _referenceObjectProvider;


        protected ObjectsShouldThat(IArchRuleCreator<TRuleType> ruleCreator,
            ObjectProvider<TReferenceType> referenceObjectProvider) : base(ruleCreator)
        {
            _referenceObjectProvider = referenceObjectProvider;
        }

        public TRuleTypeShouldConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.Are(firstObject, moreObjects));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DependOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.HaveName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.HaveFullName(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.HaveNameStartingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.HaveNameEndingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.HaveNameContaining(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivate()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.ArePrivate());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePublic()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.ArePublic());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreProtectedInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.ArePrivateProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNot(firstObject, moreObjects));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveName(name));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveFullName(fullname));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveNameStartingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveNameEndingWith(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotHaveNameContaining(pattern));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivate()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNotPrivate());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPublic()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNotPublic());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNotProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNotInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNotProtectedInternal());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.AreNotPrivateProtected());
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(_referenceObjectProvider,
                ObjectsFilterDefinition<TReferenceType>.DoNotDependOn(firstType, moreTypes));
            return CreateSyntaxElement<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}