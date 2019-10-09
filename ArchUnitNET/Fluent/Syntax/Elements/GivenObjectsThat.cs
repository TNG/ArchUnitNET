using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectPredicates<TGivenRuleTypeConjunction> where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.Are(firstObject, moreObjects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction Are(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.Are(objects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAnyTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DependOnAnyTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAnyTypesWithFullNameContaining(string pattern)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DependOnAnyTypesWithFullNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOnTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyDependOnTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveName(string name)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullName(fullname));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameStartingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameEndingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullNameContaining(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivate()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.ArePrivate());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePublic()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.ArePublic());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtected()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreInternal()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtectedInternal()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreProtectedInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivateProtected()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.ArePrivateProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNot(firstObject, moreObjects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNot(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNot(objects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAnyTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAnyTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAnyTypesWithFullNameContaining(string pattern)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAnyTypesWithFullNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveName(string name)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullName(fullname));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameStartingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameEndingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullNameContaining(string pattern)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivate()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotPrivate());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPublic()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotPublic());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtected()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotInternal()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtectedInternal()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotProtectedInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivateProtected()
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotPrivateProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}