using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectConditions<out TReturnType, out TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        // csharpier-ignore-start

        TReturnType Exist();

        TReturnType Be(params ICanBeAnalyzed[] objects);
        TReturnType Be(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Be(IObjectProvider<ICanBeAnalyzed> objects);

        TReturnType CallAny(params MethodMember[] methods);
        TReturnType CallAny(IEnumerable<MethodMember> methods);
        TReturnType CallAny(IObjectProvider<MethodMember> methods);

        TReturnType DependOnAny();
        TReturnType DependOnAny(params IType[] types);
        TReturnType DependOnAny(params Type[] types);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);

        TReturnType FollowCustomCondition(ICondition<TRuleType> condition);
        TReturnType FollowCustomCondition(Func<TRuleType, ConditionResult> condition, string description);
        TReturnType FollowCustomCondition(Func<TRuleType, bool> condition, string description, string failDescription);

        TReturnType OnlyDependOn();
        TReturnType OnlyDependOn(params IType[] types);
        TReturnType OnlyDependOn(params Type[] types);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);

        TReturnType HaveAnyAttributes();
        TReturnType HaveAnyAttributes(params Attribute[] attributes);
        TReturnType HaveAnyAttributes(params Type[] attributes);
        TReturnType HaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Type> attributes);

        TReturnType OnlyHaveAttributes();
        TReturnType OnlyHaveAttributes(params Attribute[] attributes);
        TReturnType OnlyHaveAttributes(params Type[] attributes);
        TReturnType OnlyHaveAttributes(IObjectProvider<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Type> attributes);

        TReturnType HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);

        TReturnType HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);

        TReturnType HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TReturnType HaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments);

        TReturnType HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments);

        TReturnType HaveName(string name);
        TReturnType HaveNameMatching(string pattern);
        TReturnType HaveNameStartingWith(string pattern);
        TReturnType HaveNameEndingWith(string pattern);
        TReturnType HaveNameContaining(string pattern);

        TReturnType HaveFullName(string fullName);
        TReturnType HaveFullNameMatching(string pattern);
        TReturnType HaveFullNameStartingWith(string pattern);
        TReturnType HaveFullNameEndingWith(string pattern);
        TReturnType HaveFullNameContaining(string pattern);

        TReturnType HaveAssemblyQualifiedName(string assemblyQualifiedName);
        TReturnType HaveAssemblyQualifiedNameMatching(string pattern);
        TReturnType HaveAssemblyQualifiedNameStartingWith(string pattern);
        TReturnType HaveAssemblyQualifiedNameEndingWith(string pattern);
        TReturnType HaveAssemblyQualifiedNameContaining(string pattern);

        TReturnType BePrivate();
        TReturnType BePublic();
        TReturnType BeProtected();
        TReturnType BeInternal();
        TReturnType BeProtectedInternal();
        TReturnType BePrivateProtected();

        //Negations

        TReturnType NotExist();

        TReturnType NotBe(params ICanBeAnalyzed[] objects);
        TReturnType NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType NotBe(IObjectProvider<ICanBeAnalyzed> objects);

        TReturnType NotCallAny(params MethodMember[] methods);
        TReturnType NotCallAny(IEnumerable<MethodMember> methods);
        TReturnType NotCallAny(IObjectProvider<MethodMember> methods);

        TReturnType NotDependOnAny();
        TReturnType NotDependOnAny(params IType[] types);
        TReturnType NotDependOnAny(params Type[] types);
        TReturnType NotDependOnAny(IObjectProvider<IType> types);
        TReturnType NotDependOnAny(IEnumerable<IType> types);
        TReturnType NotDependOnAny(IEnumerable<Type> types);

        TReturnType NotHaveAnyAttributes();
        TReturnType NotHaveAnyAttributes(params Attribute[] attributes);
        TReturnType NotHaveAnyAttributes(params Type[] attributes);
        TReturnType NotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Type> attributes);

        TReturnType NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);

        TReturnType NotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TReturnType NotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);

        TReturnType NotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TReturnType NotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments);

        TReturnType NotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType NotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments);

        TReturnType NotHaveName(string name);
        TReturnType NotHaveNameMatching(string pattern);
        TReturnType NotHaveNameStartingWith(string pattern);
        TReturnType NotHaveNameEndingWith(string pattern);
        TReturnType NotHaveNameContaining(string pattern);

        TReturnType NotHaveFullName(string fullName);
        TReturnType NotHaveFullNameMatching(string pattern);
        TReturnType NotHaveFullNameStartingWith(string pattern);
        TReturnType NotHaveFullNameEndingWith(string pattern);
        TReturnType NotHaveFullNameContaining(string pattern);

        TReturnType NotHaveAssemblyQualifiedName(string assemblyQualifiedName);
        TReturnType NotHaveAssemblyQualifiedNameMatching(string pattern);
        TReturnType NotHaveAssemblyQualifiedNameStartingWith(string pattern);
        TReturnType NotHaveAssemblyQualifiedNameEndingWith(string pattern);
        TReturnType NotHaveAssemblyQualifiedNameContaining(string pattern);

        TReturnType NotBePrivate();
        TReturnType NotBePublic();
        TReturnType NotBeProtected();
        TReturnType NotBeInternal();
        TReturnType NotBeProtectedInternal();
        TReturnType NotBePrivateProtected();

        // csharpier-ignore-end
    }
}
