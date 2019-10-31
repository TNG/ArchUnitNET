//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShould<TRuleTypeShouldConjunction, TRuleType> : SyntaxElement<TRuleType>,
        IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        protected ObjectsShould(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Exist()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.Exist());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.Be(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.Be(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.Be(firstObject, moreObjects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.Be(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(IObjectProvider<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.Be(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.CallAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.CallAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.CallAny(method, moreMethods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(IEnumerable<MethodMember> methods)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.CallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(IObjectProvider<MethodMember> methods)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.CallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.DependOnAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.DependOnAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.DependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.DependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyDependOn(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyDependOn(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameStartingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameEndingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivate()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.BePrivate());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePublic()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.BePublic());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtected()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.BeProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeInternal()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.BeInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BeProtectedInternal()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.BeProtectedInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction BePrivateProtected()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.BePrivateProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Conditions

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnAnyTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ArchRuleDefinition.Types(),
                ObjectConditionsDefinition<TRuleType>.DependOnAnyTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ArchRuleDefinition.Types(),
                ObjectConditionsDefinition<TRuleType>.OnlyDependOnTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAnyAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(Attributes(),
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(Attributes(),
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TRuleTypeShouldConjunction NotExist()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotExist());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBe(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBe(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBe(firstObject, moreObjects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(IEnumerable<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBe(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(IObjectProvider<ICanBeAnalyzed> objects)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBe(objects));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotCallAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotCallAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotCallAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotCallAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotCallAny(MethodMember method, params MethodMember[] moreMethods)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotCallAny(method, moreMethods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotCallAny(IEnumerable<MethodMember> methods)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotCallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotCallAny(IObjectProvider<MethodMember> methods)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotCallAny(methods));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotDependOnAny(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotDependOnAny(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(Attribute firstAttribute,
            params Attribute[] moreAttributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(firstAttribute, moreAttributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullname)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullName(fullname));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameMatching(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameStartingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameEndingWith(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameContaining(pattern));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivate()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBePrivate());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePublic()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBePublic());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtected()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBeProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeInternal()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBeInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBeProtectedInternal()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBeProtectedInternal());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivateProtected()
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotBePrivateProtected());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Condition Negations

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnAnyTypesThat()
        {
            _ruleCreator.BeginComplexCondition(ArchRuleDefinition.Types(),
                ObjectConditionsDefinition<TRuleType>.NotDependOnAnyTypesThat());
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(_ruleCreator);
        }

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAnyAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(Attributes(),
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesThat());
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}