using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectsThat<TGivenRuleTypeConjunction> where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.Are(firstObject, moreObjects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction Are(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.Are(objects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAnyTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                ObjectsFilterDefinition<TRuleType>.DependOnAnyTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOnTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                ObjectsFilterDefinition<TRuleType>.OnlyDependOnTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveName(string name)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveFullName(fullname));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveNameStartingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveNameEndingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.HaveNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivate()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.ArePrivate());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePublic()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.ArePublic());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtected()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreInternal()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreProtectedInternal()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreProtectedInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ArePrivateProtected()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.ArePrivateProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TGivenRuleTypeConjunction AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNot(firstObject, moreObjects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNot(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNot(objects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAnyTypesWithFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(
                ObjectsFilterDefinition<TRuleType>.DoNotDependOnAnyTypesWithFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotDependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotDependOnAny(firstType, moreTypes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotDependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotDependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotDependOnAny(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveName(string name)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveFullName(fullname));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveFullNameMatching(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveNameStartingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveNameEndingWith(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.DoNotHaveNameContaining(pattern));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivate()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNotPrivate());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPublic()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNotPublic());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtected()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNotProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotInternal()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNotInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotProtectedInternal()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNotProtectedInternal());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotPrivateProtected()
        {
            _ruleCreator.AddObjectFilter(ObjectsFilterDefinition<TRuleType>.AreNotPrivateProtected());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}