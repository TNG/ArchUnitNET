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
        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use Are(Types().That().HaveFullName()) instead"
        )]
        TReturnType Are(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use Are(Types().That().HaveFullName()) instead"
        )]
        TReturnType Are(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType Are(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType Are(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Are(IObjectProvider<ICanBeAnalyzed> objects);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use CallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        TReturnType CallAny(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use CallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        TReturnType CallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType CallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType CallAny(IEnumerable<MethodMember> methods);
        TReturnType CallAny(IObjectProvider<MethodMember> methods);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DependOnAny(Types().That().HaveFullName()) instead"
        )]
        TReturnType DependOnAny(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DependOnAny(Types().That().HaveFullName()) instead"
        )]
        TReturnType DependOnAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType FollowCustomPredicate(IPredicate<TRuleType> predicate);
        TReturnType FollowCustomPredicate(Func<TRuleType, bool> predicate, string description);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyDependOn(Types().That().HaveFullName()) instead"
        )]
        TReturnType OnlyDependOn(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyDependOn(Types().That().HaveFullName()) instead"
        )]
        TReturnType OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(Type firstType, params Type[] moreTypes);
        TReturnType OnlyDependOn(IType firstType, params IType[] moreTypes);
        TReturnType OnlyDependOn(IObjectProvider<IType> types);
        TReturnType OnlyDependOn(IEnumerable<IType> types);
        TReturnType OnlyDependOn(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType HaveAnyAttributes(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType HaveAnyAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType HaveAnyAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType HaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType HaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType HaveAnyAttributes(IEnumerable<Type> attributes);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyHaveAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType OnlyHaveAttributes(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyHaveAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType OnlyHaveAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType OnlyHaveAttributes(Attribute firstAttribute, params Attribute[] moreAttributes);
        TReturnType OnlyHaveAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType OnlyHaveAttributes(IObjectProvider<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Attribute> attributes);
        TReturnType OnlyHaveAttributes(IEnumerable<Type> attributes);
        TReturnType HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType HaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType HaveAttributeWithArguments(
            string attribute,
            IEnumerable<object> argumentValues
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType HaveAttributeWithArguments(
            string attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType HaveAttributeWithArguments(
            Attribute attribute,
            IEnumerable<object> argumentValues
        );
        TReturnType HaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues);
        TReturnType HaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType HaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType HaveAttributeWithNamedArguments(
            string attribute,
            IEnumerable<(string, object)> attributeArguments
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType HaveAttributeWithNamedArguments(
            string attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );
        TReturnType HaveAttributeWithNamedArguments(
            Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType HaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );
        TReturnType HaveAttributeWithNamedArguments(
            Type attribute,
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType HaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );

        [Obsolete(
            "Either HaveName() without the useRegularExpressions parameter or HaveNameMatching() should be used"
        )]
        TReturnType HaveName(string pattern, bool useRegularExpressions);
        TReturnType HaveName(string name);
        TReturnType HaveNameMatching(string pattern);

        [Obsolete(
            "Either HaveFullName() without the useRegularExpressions parameter or HaveFullNameMatching() should be used"
        )]
        TReturnType HaveFullName(string pattern, bool useRegularExpressions);
        TReturnType HaveFullName(string fullName);
        TReturnType HaveFullNameMatching(string pattern);
        TReturnType HaveNameStartingWith(string pattern);
        TReturnType HaveNameEndingWith(string pattern);
        TReturnType HaveNameContaining(string pattern);
        TReturnType HaveFullNameContaining(string pattern);
        TReturnType ArePrivate();
        TReturnType ArePublic();
        TReturnType AreProtected();
        TReturnType AreInternal();
        TReturnType AreProtectedInternal();
        TReturnType ArePrivateProtected();

        //Negations

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNot(Types().That().HaveFullName()) instead"
        )]
        TReturnType AreNot(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNot(Types().That().HaveFullName()) instead"
        )]
        TReturnType AreNot(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType AreNot(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType AreNot(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType AreNot(IObjectProvider<ICanBeAnalyzed> objects);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotCallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        TReturnType DoNotCallAny(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotCallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        TReturnType DoNotCallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType DoNotCallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType DoNotCallAny(IEnumerable<MethodMember> methods);
        TReturnType DoNotCallAny(IObjectProvider<MethodMember> methods);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotDependOnAny(Types().That().HaveFullName()) instead"
        )]
        TReturnType DoNotDependOnAny(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotDependOnAny(Types().That().HaveFullName()) instead"
        )]
        TReturnType DoNotDependOnAny(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType DoNotDependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DoNotDependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DoNotDependOnAny(IObjectProvider<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<IType> types);
        TReturnType DoNotDependOnAny(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotHaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType DoNotHaveAnyAttributes(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DoNotHaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType DoNotHaveAnyAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType DoNotHaveAnyAttributes(
            Attribute firstAttribute,
            params Attribute[] moreAttributes
        );
        TReturnType DoNotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType DoNotHaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType DoNotHaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType DoNotHaveAttributeWithArguments(
            string attribute,
            IEnumerable<object> argumentValues
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType DoNotHaveAttributeWithArguments(
            string attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType DoNotHaveAttributeWithArguments(
            Attribute attribute,
            IEnumerable<object> argumentValues
        );
        TReturnType DoNotHaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType DoNotHaveAttributeWithArguments(
            Type attribute,
            IEnumerable<object> argumentValues
        );
        TReturnType DoNotHaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType DoNotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType DoNotHaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType DoNotHaveAttributeWithNamedArguments(
            string attribute,
            IEnumerable<(string, object)> attributeArguments
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType DoNotHaveAttributeWithNamedArguments(
            string attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );
        TReturnType DoNotHaveAttributeWithNamedArguments(
            Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType DoNotHaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );
        TReturnType DoNotHaveAttributeWithNamedArguments(
            Type attribute,
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType DoNotHaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );

        [Obsolete(
            "Either DoNotHaveName() without the useRegularExpressions parameter or DoNotHaveNameMatching() should be used"
        )]
        TReturnType DoNotHaveName(string pattern, bool useRegularExpressions);
        TReturnType DoNotHaveName(string name);
        TReturnType DoNotHaveNameMatching(string pattern);

        [Obsolete(
            "Either DoNotHaveFullName() without the useRegularExpressions parameter or DoNotHaveFullNameMatching() should be used"
        )]
        TReturnType DoNotHaveFullName(string pattern, bool useRegularExpressions);
        TReturnType DoNotHaveFullName(string fullName);
        TReturnType DoNotHaveFullNameMatching(string pattern);
        TReturnType DoNotHaveNameStartingWith(string pattern);
        TReturnType DoNotHaveNameEndingWith(string pattern);
        TReturnType DoNotHaveNameContaining(string pattern);
        TReturnType DoNotHaveFullNameContaining(string pattern);
        TReturnType AreNotPrivate();
        TReturnType AreNotPublic();
        TReturnType AreNotProtected();
        TReturnType AreNotInternal();
        TReturnType AreNotProtectedInternal();
        TReturnType AreNotPrivateProtected();
    }
}
