//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectConditions<out TReturnType, out TRuleType> where TRuleType : ICanBeAnalyzed
    {
        TReturnType Exist();
        TReturnType Be(string pattern, bool useRegularExpressions = false);
        TReturnType Be(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType Be(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Be(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType CallAny(string pattern, bool useRegularExpressions = false);
        TReturnType CallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType CallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType CallAny(IEnumerable<MethodMember> methods);
        TReturnType CallAny(IObjectProvider<MethodMember> methods);
        TReturnType DependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType FollowCustomCondition(ICondition<TRuleType> condition);
        TReturnType FollowCustomCondition(Func<TRuleType, ConditionResult> condition, string description);
        TReturnType FollowCustomCondition(Func<TRuleType, bool> condition, string description, string failDescription);
        TReturnType OnlyDependOn(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IType firstType, params IType[] moreTypes);
        TReturnType OnlyDependOn(Type firstType, params Type[] moreTypes);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);
        TReturnType HaveAnyAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType HaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType HaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType OnlyHaveAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType OnlyHaveAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyHaveAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType OnlyHaveAttributes(IObjectProvider<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Type> attributes);
        TReturnType HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType HaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType HaveAttributeWithArguments(string attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(string attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(Attribute attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(Type attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType HaveAnyAttributesWithNamedArguments(IEnumerable<(string,object)> attributeArguments);
        TReturnType HaveAnyAttributesWithNamedArguments((string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType HaveAttributeWithNamedArguments(string attribute, IEnumerable<(string,object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(string attribute, (string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string,object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Attribute attribute, (string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string,object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Type attribute, (string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType HaveName(string pattern, bool useRegularExpressions = false);
        TReturnType HaveFullName(string pattern, bool useRegularExpressions = false);
        TReturnType HaveNameStartingWith(string pattern);
        TReturnType HaveNameEndingWith(string pattern);
        TReturnType HaveNameContaining(string pattern);
        TReturnType HaveFullNameContaining(string pattern);
        TReturnType BePrivate();
        TReturnType BePublic();
        TReturnType BeProtected();
        TReturnType BeInternal();
        TReturnType BeProtectedInternal();
        TReturnType BePrivateProtected();


        //Negations


        TReturnType NotExist();
        TReturnType NotBe(string pattern, bool useRegularExpressions = false);
        TReturnType NotBe(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType NotBe(IObjectProvider<ICanBeAnalyzed> objects);
        TReturnType NotCallAny(string pattern, bool useRegularExpressions = false);
        TReturnType NotCallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotCallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType NotCallAny(IEnumerable<MethodMember> methods);
        TReturnType NotCallAny(IObjectProvider<MethodMember> methods);
        TReturnType NotDependOnAny(string pattern, bool useRegularExpressions = false);
        TReturnType NotDependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotDependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType NotDependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType NotDependOnAny(IObjectProvider<IType> types);
        TReturnType NotDependOnAny(IEnumerable<IType> types);
        TReturnType NotDependOnAny(IEnumerable<Type> types);
        TReturnType NotHaveAnyAttributes(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveAnyAttributes(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotHaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType NotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType NotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType NotHaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType NotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TReturnType NotHaveAttributeWithArguments(Attribute attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType NotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);
        TReturnType NotHaveAttributeWithArguments(Type attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType NotHaveAnyAttributesWithNamedArguments(IEnumerable<(string,object)> attributeArguments);
        TReturnType NotHaveAnyAttributesWithNamedArguments((string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string,object)> attributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Attribute attribute, (string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string,object)> attributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Type attribute, (string,object) firstAttributeArgument, params (string,object)[] moreAttributeArguments);
        TReturnType NotHaveName(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveFullName(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveNameStartingWith(string pattern);
        TReturnType NotHaveNameEndingWith(string pattern);
        TReturnType NotHaveNameContaining(string pattern);
        TReturnType NotHaveFullNameContaining(string pattern);
        TReturnType NotBePrivate();
        TReturnType NotBePublic();
        TReturnType NotBeProtected();
        TReturnType NotBeInternal();
        TReturnType NotBeProtectedInternal();
        TReturnType NotBePrivateProtected();
    }
}