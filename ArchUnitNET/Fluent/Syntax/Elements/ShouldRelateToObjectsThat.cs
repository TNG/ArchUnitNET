﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        SyntaxElement<TRuleType>,
        IObjectPredicates<TRuleTypeShouldConjunction, TReferenceType>
        where TReferenceType : ICanBeAnalyzed
        where TRuleType : ICanBeAnalyzed
    {
        protected ShouldRelateToObjectsThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }


        public TRuleTypeShouldConjunction Are(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.Are(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Are(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.Are(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.Are(firstObject, moreObjects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Are(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.Are(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Are(IObjectProvider<ICanBeAnalyzed> objects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.Are(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.CallAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.CallAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.CallAny(method, moreMethods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(IEnumerable<MethodMember> methods)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.CallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(IObjectProvider<MethodMember> methods)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.CallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DependOnAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DependOnAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction FollowCustomPredicate(IPredicate<TReferenceType> predicate)
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction FollowCustomPredicate(Func<TReferenceType, bool> predicate,
            string description)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.FollowCustomPredicate(predicate, description));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveNameStartingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveNameEndingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.HaveFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivate()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.ArePrivate());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePublic()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.ArePublic());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreProtectedInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ArePrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.ArePrivateProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.AreNot(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNot(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.AreNot(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.AreNot(firstObject, moreObjects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNot(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.AreNot(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNot(IObjectProvider<ICanBeAnalyzed> objects)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.AreNot(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotCallAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotCallAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotCallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotCallAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotCallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotCallAny(method, moreMethods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotCallAny(IEnumerable<MethodMember> methods)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotCallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotCallAny(IObjectProvider<MethodMember> methods)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotCallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotDependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveName(string name)
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFullNameMatching(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameStartingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameEndingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFullNameContaining(string pattern)
        {
            _ruleCreator.ContinueComplexCondition(
                ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivate()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotPrivate());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPublic()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotPublic());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotProtectedInternal()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotProtectedInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotPrivateProtected()
        {
            _ruleCreator.ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotPrivateProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}