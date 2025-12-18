using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IAddObjectPredicate<out TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        // csharpier-ignore-start

        TNextElement FollowCustomPredicate(IPredicate<TRuleType> predicate);
        TNextElement FollowCustomPredicate(Func<TRuleType, bool> predicate, string description);

        TNextElement Are(params ICanBeAnalyzed[] objects);
        TNextElement Are(IEnumerable<ICanBeAnalyzed> objects);
        TNextElement Are(IObjectProvider<ICanBeAnalyzed> objects);

        TNextElement CallAny(params MethodMember[] methods);
        TNextElement CallAny(IEnumerable<MethodMember> methods);
        TNextElement CallAny(IObjectProvider<MethodMember> methods);

        TNextElement DependOnAny();
        TNextElement DependOnAny(params IType[] types);
        TNextElement DependOnAny(params Type[] types);
        TNextElement DependOnAny(IEnumerable<IType> types);
        TNextElement DependOnAny(IEnumerable<Type> types);
        TNextElement DependOnAny(IObjectProvider<IType> types);

        TNextElement OnlyDependOn();
        TNextElement OnlyDependOn(params IType[] types);
        TNextElement OnlyDependOn(params Type[] types);
        TNextElement OnlyDependOn(IEnumerable<IType> types);
        TNextElement OnlyDependOn(IEnumerable<Type> types);
        TNextElement OnlyDependOn(IObjectProvider<IType> types);

        TNextElement HaveAnyAttributes();
        TNextElement HaveAnyAttributes(params Attribute[] attributes);
        TNextElement HaveAnyAttributes(params Type[] attributes);
        TNextElement HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TNextElement HaveAnyAttributes(IEnumerable<Type> attributes);
        TNextElement HaveAnyAttributes(IObjectProvider<Attribute> attributes);

        TNextElement OnlyHaveAttributes();
        TNextElement OnlyHaveAttributes(params Attribute[] attributes);
        TNextElement OnlyHaveAttributes(params Type[] attributes);
        TNextElement OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TNextElement OnlyHaveAttributes(IEnumerable<Type> attributes);
        TNextElement OnlyHaveAttributes(IObjectProvider<Attribute> attributes);

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

        TNextElement ArePrivate();
        TNextElement ArePublic();
        TNextElement AreProtected();
        TNextElement AreInternal();
        TNextElement AreProtectedInternal();
        TNextElement ArePrivateProtected();

        //Negations

        TNextElement AreNot(params ICanBeAnalyzed[] objects);
        TNextElement AreNot(IEnumerable<ICanBeAnalyzed> objects);
        TNextElement AreNot(IObjectProvider<ICanBeAnalyzed> objects);

        TNextElement DoNotCallAny(params MethodMember[] methods);
        TNextElement DoNotCallAny(IEnumerable<MethodMember> methods);
        TNextElement DoNotCallAny(IObjectProvider<MethodMember> methods);

        TNextElement DoNotDependOnAny();
        TNextElement DoNotDependOnAny(params IType[] types);
        TNextElement DoNotDependOnAny(params Type[] types);
        TNextElement DoNotDependOnAny(IObjectProvider<IType> types);
        TNextElement DoNotDependOnAny(IEnumerable<IType> types);
        TNextElement DoNotDependOnAny(IEnumerable<Type> types);

        TNextElement DoNotHaveAnyAttributes();
        TNextElement DoNotHaveAnyAttributes(params Attribute[] attributes);
        TNextElement DoNotHaveAnyAttributes(params Type[] attributes);
        TNextElement DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TNextElement DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TNextElement DoNotHaveAnyAttributes(IEnumerable<Type> attributes);

        TNextElement DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);

        TNextElement DoNotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TNextElement DoNotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);

        TNextElement DoNotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TNextElement DoNotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments);

        TNextElement DoNotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TNextElement DoNotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments);
        TNextElement DoNotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TNextElement DoNotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments);

        TNextElement DoNotHaveName(string name);
        TNextElement DoNotHaveNameMatching(string pattern);
        TNextElement DoNotHaveNameStartingWith(string pattern);
        TNextElement DoNotHaveNameEndingWith(string pattern);
        TNextElement DoNotHaveNameContaining(string pattern);

        TNextElement DoNotHaveFullName(string fullName);
        TNextElement DoNotHaveFullNameMatching(string pattern);
        TNextElement DoNotHaveFullNameStartingWith(string pattern);
        TNextElement DoNotHaveFullNameEndingWith(string pattern);
        TNextElement DoNotHaveFullNameContaining(string pattern);

        TNextElement DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName);
        TNextElement DoNotHaveAssemblyQualifiedNameMatching(string pattern);
        TNextElement DoNotHaveAssemblyQualifiedNameStartingWith(string pattern);
        TNextElement DoNotHaveAssemblyQualifiedNameEndingWith(string pattern);
        TNextElement DoNotHaveAssemblyQualifiedNameContaining(string pattern);

        TNextElement AreNotPrivate();
        TNextElement AreNotPublic();
        TNextElement AreNotProtected();
        TNextElement AreNotInternal();
        TNextElement AreNotProtectedInternal();
        TNextElement AreNotPrivateProtected();

        // csharpier-ignore-end
    }
}
