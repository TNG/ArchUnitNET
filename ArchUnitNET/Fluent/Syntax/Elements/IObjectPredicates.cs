using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public interface IObjectPredicates<out TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        // csharpier-ignore-start

        TReturnType FollowCustomPredicate(IPredicate<TRuleType> predicate);
        TReturnType FollowCustomPredicate(Func<TRuleType, bool> predicate, string description);

        TReturnType Are(params ICanBeAnalyzed[] objects);
        TReturnType Are(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Are(IObjectProvider<ICanBeAnalyzed> objects);

        TReturnType CallAny(params MethodMember[] methods);
        TReturnType CallAny(IEnumerable<MethodMember> methods);
        TReturnType CallAny(IObjectProvider<MethodMember> methods);

        TReturnType DependOnAny();
        TReturnType DependOnAny(params IType[] types);
        TReturnType DependOnAny(params Type[] types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType DependOnAny(IObjectProvider<IType> types);

        TReturnType OnlyDependOn();
        TReturnType OnlyDependOn(params IType[] types);
        TReturnType OnlyDependOn(params Type[] types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);

        TReturnType HaveAnyAttributes();
        TReturnType HaveAnyAttributes(params Attribute[] attributes);
        TReturnType HaveAnyAttributes(params Type[] attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType HaveAnyAttributes(IObjectProvider<Attribute> attributes);

        TReturnType OnlyHaveAttributes();
        TReturnType OnlyHaveAttributes(params Attribute[] attributes);
        TReturnType OnlyHaveAttributes(params Type[] attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Type> attributes);
        TReturnType OnlyHaveAttributes(IObjectProvider<Attribute> attributes);

        TReturnType HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType HaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues);

        TReturnType HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(Attribute attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(Type attribute, object firstArgumentValue, params object[] moreArgumentValues);

        TReturnType HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TReturnType HaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments);

        TReturnType HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Attribute attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType HaveAttributeWithNamedArguments(Type attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments);

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

        TReturnType ArePrivate();
        TReturnType ArePublic();
        TReturnType AreProtected();
        TReturnType AreInternal();
        TReturnType AreProtectedInternal();
        TReturnType ArePrivateProtected();

        //Negations

        TReturnType AreNot(params ICanBeAnalyzed[] objects);
        TReturnType AreNot(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType AreNot(IObjectProvider<ICanBeAnalyzed> objects);

        TReturnType DoNotCallAny(params MethodMember[] methods);
        TReturnType DoNotCallAny(IEnumerable<MethodMember> methods);
        TReturnType DoNotCallAny(IObjectProvider<MethodMember> methods);

        TReturnType DoNotDependOnAny();
        TReturnType DoNotDependOnAny(params IType[] types);
        TReturnType DoNotDependOnAny(params Type[] types);
        TReturnType DoNotDependOnAny(IObjectProvider<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<Type> types);

        TReturnType DoNotHaveAnyAttributes();
        TReturnType DoNotHaveAnyAttributes(params Attribute[] attributes);
        TReturnType DoNotHaveAnyAttributes(params Type[] attributes);
        TReturnType DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<Type> attributes);

        TReturnType DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType DoNotHaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues);

        TReturnType DoNotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues);
        TReturnType DoNotHaveAttributeWithArguments(Attribute attribute, object firstArgumentValue, params object[] moreArgumentValues);
        TReturnType DoNotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);
        TReturnType DoNotHaveAttributeWithArguments(Type attribute, object firstArgumentValue, params object[] moreArgumentValues);

        TReturnType DoNotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments);
        TReturnType DoNotHaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments);

        TReturnType DoNotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType DoNotHaveAttributeWithNamedArguments(Attribute attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments);
        TReturnType DoNotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments);
        TReturnType DoNotHaveAttributeWithNamedArguments(Type attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments);

        TReturnType DoNotHaveName(string name);
        TReturnType DoNotHaveNameMatching(string pattern);
        TReturnType DoNotHaveNameStartingWith(string pattern);
        TReturnType DoNotHaveNameEndingWith(string pattern);
        TReturnType DoNotHaveNameContaining(string pattern);

        TReturnType DoNotHaveFullName(string fullName);
        TReturnType DoNotHaveFullNameMatching(string pattern);
        TReturnType DoNotHaveFullNameStartingWith(string pattern);
        TReturnType DoNotHaveFullNameEndingWith(string pattern);
        TReturnType DoNotHaveFullNameContaining(string pattern);

        TReturnType DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName);
        TReturnType DoNotHaveAssemblyQualifiedNameMatching(string pattern);
        TReturnType DoNotHaveAssemblyQualifiedNameStartingWith(string pattern);
        TReturnType DoNotHaveAssemblyQualifiedNameEndingWith(string pattern);
        TReturnType DoNotHaveAssemblyQualifiedNameContaining(string pattern);

        TReturnType AreNotPrivate();
        TReturnType AreNotPublic();
        TReturnType AreNotProtected();
        TReturnType AreNotInternal();
        TReturnType AreNotProtectedInternal();
        TReturnType AreNotPrivateProtected();

        // csharpier-ignore-end
    }
}
