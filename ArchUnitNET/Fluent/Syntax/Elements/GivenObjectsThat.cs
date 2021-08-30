//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
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
    public abstract class GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IObjectPredicates<TGivenRuleTypeConjunction, TRuleType> where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TGivenRuleTypeConjunction Are(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.Are(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction Are(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.Are(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
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

        public TGivenRuleTypeConjunction Are(IObjectProvider<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.Are(objects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction CallAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.CallAny(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction CallAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.CallAny(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction CallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.CallAny(method, moreMethods));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction CallAny(IEnumerable<MethodMember> methods)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.CallAny(methods));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction CallAny(IObjectProvider<MethodMember> methods)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.CallAny(methods));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DependOnAny(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DependOnAny(patterns, useRegularExpressions));
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

        public TGivenRuleTypeConjunction FollowCustomPredicate(IPredicate<TRuleType> predicate)
        {
            _ruleCreator.AddPredicate(predicate);
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction FollowCustomPredicate(Func<TRuleType, bool> predicate, string description)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.FollowCustomPredicate(predicate, description));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(patterns, useRegularExpressions));
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

        public TGivenRuleTypeConjunction HaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(firstAttribute, moreAttributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(firstAttribute, moreAttributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction OnlyHaveAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithArguments(argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithArguments(firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(string attribute, IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute,argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(string attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute,firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute,argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Attribute attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute,firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute,argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Type attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute,firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(string attribute, IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute,attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(string attribute, (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute,firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute,attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Attribute attribute, (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute,firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute,attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Type attribute, (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute,firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveName(string name)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveNameMatching(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameMatching(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullName(fullname));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFullNameMatching(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameMatching(pattern, useRegularExpressions));
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


        public TGivenRuleTypeConjunction AreNot(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.AreNot(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNot(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.AreNot(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

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

        public TGivenRuleTypeConjunction AreNot(IObjectProvider<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNot(objects));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotCallAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotCallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotCallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(method, moreMethods));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotCallAny(IEnumerable<MethodMember> methods)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(methods));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotCallAny(IObjectProvider<MethodMember> methods)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(methods));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(patterns, useRegularExpressions));
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

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(patterns, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            _ruleCreator.AddPredicate(
                ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(attributes));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithArguments(argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithArguments(object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithArguments(firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(string attribute, IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute,argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(string attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute,firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(string attribute, IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute,attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(string attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute,firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute,argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Attribute attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute,firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute,argumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Type attribute, object firstArgumentValue,
            params object[] moreArgumentValues)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute,firstArgumentValue,moreArgumentValues));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithNamedArguments(attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithNamedArguments(firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute,attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Attribute attribute,
            (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute,firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute,attributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Type attribute, (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute,firstAttributeArgument,moreAttributeArguments));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveName(string name)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveNameMatching(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameMatching(pattern, useRegularExpressions));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullName(string fullname)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullName(fullname));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFullNameMatching(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameMatching(pattern, useRegularExpressions));
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