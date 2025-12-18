using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IAddObjectCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        // csharpier-ignore-start

        TNextElement Exist();

        TNextElement Be(params ICanBeAnalyzed[] objects);
        TNextElement Be(IEnumerable<ICanBeAnalyzed> objects);
        TNextElement Be(IObjectProvider<ICanBeAnalyzed> objects);

        TNextElement CallAny(params MethodMember[] methods);
        TNextElement CallAny(IEnumerable<MethodMember> methods);
        TNextElement CallAny(IObjectProvider<MethodMember> methods);

        TNextElement DependOnAny();
        TNextElement DependOnAny(params IType[] types);
        TNextElement DependOnAny(params Type[] types);
        TNextElement DependOnAny(IObjectProvider<IType> types);
        TNextElement DependOnAny(IEnumerable<IType> types);
        TNextElement DependOnAny(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> DependOnAnyTypesThat();

        TNextElement FollowCustomCondition(ICondition<TRuleType> condition);
        TNextElement FollowCustomCondition(Func<TRuleType, ConditionResult> condition, string description);
        TNextElement FollowCustomCondition(Func<TRuleType, bool> condition, string description, string failDescription);

        TNextElement OnlyDependOn();
        TNextElement OnlyDependOn(params IType[] types);
        TNextElement OnlyDependOn(params Type[] types);
        TNextElement OnlyDependOn(IObjectProvider<IType> types);
        TNextElement OnlyDependOn(IEnumerable<IType> types);
        TNextElement OnlyDependOn(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> OnlyDependOnTypesThat();

        TNextElement HaveAnyAttributes();
        TNextElement HaveAnyAttributes(params Attribute[] attributes);
        TNextElement HaveAnyAttributes(params Type[] attributes);
        TNextElement HaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TNextElement HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TNextElement HaveAnyAttributes(IEnumerable<Type> attributes);
        ShouldRelateToAttributesThat<TNextElement, TRuleType> HaveAnyAttributesThat();

        TNextElement OnlyHaveAttributes();
        TNextElement OnlyHaveAttributes(params Attribute[] attributes);
        TNextElement OnlyHaveAttributes(params Type[] attributes);
        TNextElement OnlyHaveAttributes(IObjectProvider<Attribute> attributes);
        TNextElement OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TNextElement OnlyHaveAttributes(IEnumerable<Type> attributes);
        ShouldRelateToAttributesThat<TNextElement, TRuleType> OnlyHaveAttributesThat();

        TNextElement HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);

        TNextElement HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TNextElement HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);

        TNextElement HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TNextElement HaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments);

        TNextElement HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TNextElement HaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments);
        TNextElement HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TNextElement HaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments);

        TNextElement HaveName(string name);
        TNextElement HaveNameMatching(string pattern);
        TNextElement HaveNameStartingWith(string pattern);
        TNextElement HaveNameEndingWith(string pattern);
        TNextElement HaveNameContaining(string pattern);

        TNextElement HaveFullName(string fullName);
        TNextElement HaveFullNameMatching(string pattern);
        TNextElement HaveFullNameStartingWith(string pattern);
        TNextElement HaveFullNameEndingWith(string pattern);
        TNextElement HaveFullNameContaining(string pattern);

        TNextElement HaveAssemblyQualifiedName(string assemblyQualifiedName);
        TNextElement HaveAssemblyQualifiedNameMatching(string pattern);
        TNextElement HaveAssemblyQualifiedNameStartingWith(string pattern);
        TNextElement HaveAssemblyQualifiedNameEndingWith(string pattern);
        TNextElement HaveAssemblyQualifiedNameContaining(string pattern);

        TNextElement BePrivate();
        TNextElement BePublic();
        TNextElement BeProtected();
        TNextElement BeInternal();
        TNextElement BeProtectedInternal();
        TNextElement BePrivateProtected();

        //Negations

        TNextElement NotExist();

        TNextElement NotBe(params ICanBeAnalyzed[] objects);
        TNextElement NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TNextElement NotBe(IObjectProvider<ICanBeAnalyzed> objects);

        TNextElement NotCallAny(params MethodMember[] methods);
        TNextElement NotCallAny(IEnumerable<MethodMember> methods);
        TNextElement NotCallAny(IObjectProvider<MethodMember> methods);

        TNextElement NotDependOnAny();
        TNextElement NotDependOnAny(params IType[] types);
        TNextElement NotDependOnAny(params Type[] types);
        TNextElement NotDependOnAny(IObjectProvider<IType> types);
        TNextElement NotDependOnAny(IEnumerable<IType> types);
        TNextElement NotDependOnAny(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> NotDependOnAnyTypesThat();

        TNextElement NotHaveAnyAttributes();
        TNextElement NotHaveAnyAttributes(params Attribute[] attributes);
        TNextElement NotHaveAnyAttributes(params Type[] attributes);
        TNextElement NotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TNextElement NotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TNextElement NotHaveAnyAttributes(IEnumerable<Type> attributes);
        ShouldRelateToAttributesThat<TNextElement, TRuleType> NotHaveAnyAttributesThat();

        TNextElement NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);

        TNextElement NotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TNextElement NotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);

        TNextElement NotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TNextElement NotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments);

        TNextElement NotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TNextElement NotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments);
        TNextElement NotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TNextElement NotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments);

        TNextElement NotHaveName(string name);
        TNextElement NotHaveNameMatching(string pattern);
        TNextElement NotHaveNameStartingWith(string pattern);
        TNextElement NotHaveNameEndingWith(string pattern);
        TNextElement NotHaveNameContaining(string pattern);

        TNextElement NotHaveFullName(string fullName);
        TNextElement NotHaveFullNameMatching(string pattern);
        TNextElement NotHaveFullNameStartingWith(string pattern);
        TNextElement NotHaveFullNameEndingWith(string pattern);
        TNextElement NotHaveFullNameContaining(string pattern);

        TNextElement NotHaveAssemblyQualifiedName(string assemblyQualifiedName);
        TNextElement NotHaveAssemblyQualifiedNameMatching(string pattern);
        TNextElement NotHaveAssemblyQualifiedNameStartingWith(string pattern);
        TNextElement NotHaveAssemblyQualifiedNameEndingWith(string pattern);
        TNextElement NotHaveAssemblyQualifiedNameContaining(string pattern);

        TNextElement NotBePrivate();
        TNextElement NotBePublic();
        TNextElement NotBeProtected();
        TNextElement NotBeInternal();
        TNextElement NotBeProtectedInternal();
        TNextElement NotBePrivateProtected();

        // csharpier-ignore-end
    }
}
