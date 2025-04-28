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
        TReturnType Exist();

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use Be(Types().That().HaveFullName()) instead"
        )]
        TReturnType Be(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use Be(Types().That().HaveFullName()) instead"
        )]
        TReturnType Be(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType Be(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType Be(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType Be(IObjectProvider<ICanBeAnalyzed> objects);

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
        TReturnType DependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType DependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType DependOnAny(IObjectProvider<IType> types);
        TReturnType DependOnAny(IEnumerable<IType> types);
        TReturnType DependOnAny(IEnumerable<Type> types);
        TReturnType FollowCustomCondition(ICondition<TRuleType> condition);
        TReturnType FollowCustomCondition(
            Func<TRuleType, ConditionResult> condition,
            string description
        );
        TReturnType FollowCustomCondition(
            Func<TRuleType, bool> condition,
            string description,
            string failDescription
        );

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyDependOn(Types().That().HaveFullName()) instead"
        )]
        TReturnType OnlyDependOn(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyDependOn(Types().That().HaveFullName()) instead"
        )]
        TReturnType OnlyDependOn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType OnlyDependOn(IType firstType, params IType[] moreTypes);
        TReturnType OnlyDependOn(Type firstType, params Type[] moreTypes);
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
        TReturnType BePrivate();
        TReturnType BePublic();
        TReturnType BeProtected();
        TReturnType BeInternal();
        TReturnType BeProtectedInternal();
        TReturnType BePrivateProtected();

        //Negations

        TReturnType NotExist();

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBe(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotBe(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBe(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotBe(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBe(ICanBeAnalyzed firstObject, params ICanBeAnalyzed[] moreObjects);
        TReturnType NotBe(IEnumerable<ICanBeAnalyzed> objects);
        TReturnType NotBe(IObjectProvider<ICanBeAnalyzed> objects);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotCallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        TReturnType NotCallAny(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotCallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        TReturnType NotCallAny(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotCallAny(MethodMember method, params MethodMember[] moreMethods);
        TReturnType NotCallAny(IEnumerable<MethodMember> methods);
        TReturnType NotCallAny(IObjectProvider<MethodMember> methods);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotDependOnAny(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotDependOnAny(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotDependOnAny(Types().That().HaveFullName()) instead"
        )]
        TReturnType NotDependOnAny(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType NotDependOnAny(IType firstType, params IType[] moreTypes);
        TReturnType NotDependOnAny(Type firstType, params Type[] moreTypes);
        TReturnType NotDependOnAny(IObjectProvider<IType> types);
        TReturnType NotDependOnAny(IEnumerable<IType> types);
        TReturnType NotDependOnAny(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotHaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType NotHaveAnyAttributes(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotHaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        TReturnType NotHaveAnyAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType NotHaveAnyAttributes(
            Attribute firstAttribute,
            params Attribute[] moreAttributes
        );
        TReturnType NotHaveAnyAttributes(Type firstAttribute, params Type[] moreAttributes);
        TReturnType NotHaveAnyAttributes(IObjectProvider<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Attribute> attributes);
        TReturnType NotHaveAnyAttributes(IEnumerable<Type> attributes);
        TReturnType NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues);
        TReturnType NotHaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType NotHaveAttributeWithArguments(
            Attribute attribute,
            IEnumerable<object> argumentValues
        );
        TReturnType NotHaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType NotHaveAttributeWithArguments(
            Type attribute,
            IEnumerable<object> argumentValues
        );
        TReturnType NotHaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        );
        TReturnType NotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType NotHaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );
        TReturnType NotHaveAttributeWithNamedArguments(
            Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType NotHaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );
        TReturnType NotHaveAttributeWithNamedArguments(
            Type attribute,
            IEnumerable<(string, object)> attributeArguments
        );
        TReturnType NotHaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        );

        [Obsolete(
            "Either NotHaveName() without the useRegularExpressions parameter or NotHaveNameMatching() should be used"
        )]
        TReturnType NotHaveName(string pattern, bool useRegularExpressions);
        TReturnType NotHaveName(string name);
        TReturnType NotHaveNameMatching(string pattern);

        [Obsolete(
            "Either NotHaveFullName() without the useRegularExpressions parameter or NotHaveFullNameMatching() should be used"
        )]
        TReturnType NotHaveFullName(string pattern, bool useRegularExpressions);
        TReturnType NotHaveFullName(string fullName);
        TReturnType NotHaveFullNameMatching(string pattern);
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
