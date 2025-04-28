using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShould<TRuleTypeShouldConjunction, TRuleType>
        : SyntaxElement<TRuleType>,
            IComplexObjectConditions<TRuleTypeShouldConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        protected ObjectsShould(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TRuleTypeShouldConjunction Exist()
        {
            _ruleCreator.RequirePositiveResults = false;
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.Exist());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use Be(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction Be(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.Be(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use Be(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction Be(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.Be(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Be(
            ICanBeAnalyzed firstObject,
            params ICanBeAnalyzed[] moreObjects
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.Be(firstObject, moreObjects)
            );
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

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use CallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction CallAny(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.CallAny(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use CallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction CallAny(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.CallAny(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction CallAny(
            MethodMember method,
            params MethodMember[] moreMethods
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.CallAny(method, moreMethods)
            );
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

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DependOnAny(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction DependOnAny(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.DependOnAny(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use DependOnAny(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction DependOnAny(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.DependOnAny(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.DependOnAny(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.DependOnAny(firstType, moreTypes)
            );
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

        public TRuleTypeShouldConjunction FollowCustomCondition(ICondition<TRuleType> condition)
        {
            _ruleCreator.AddCondition(condition);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction FollowCustomCondition(
            Func<TRuleType, ConditionResult> condition,
            string description
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.FollowCustomCondition(condition, description)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction FollowCustomCondition(
            Func<TRuleType, bool> condition,
            string description,
            string failDescription
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.FollowCustomCondition(
                    condition,
                    description,
                    failDescription
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyDependOn(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction OnlyDependOn(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyDependOn(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyDependOn(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction OnlyDependOn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyDependOn(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyDependOn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyDependOn(firstType, moreTypes)
            );
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

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction HaveAnyAttributes(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use HaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction HaveAnyAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(
            Attribute firstAttribute,
            params Attribute[] moreAttributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(
                    firstAttribute,
                    moreAttributes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(
            Type firstAttribute,
            params Type[] moreAttributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(
                    firstAttribute,
                    moreAttributes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyHaveAttributes(Attributes().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction OnlyHaveAttributes(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use OnlyHaveAttributes(Attributes().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction OnlyHaveAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(
            Attribute firstAttribute,
            params Attribute[] moreAttributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(
                    firstAttribute,
                    moreAttributes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(
            Type firstAttribute,
            params Type[] moreAttributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(
                    firstAttribute,
                    moreAttributes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithArguments(argumentValues)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithArguments(
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction HaveAttributeWithArguments(
            string attribute,
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(
                    attribute,
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction HaveAttributeWithArguments(
            string attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(
                    attribute,
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithArguments(
            Attribute attribute,
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(
                    attribute,
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(
                    attribute,
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithArguments(
            Type attribute,
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(
                    attribute,
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(
                    attribute,
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(
            string attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(
                    attribute,
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(
            string attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(
                    attribute,
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(
            Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(
                    attribute,
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(
                    attribute,
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(
            Type attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(
                    attribute,
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(
                    attribute,
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either HaveName() without the useRegularExpressions parameter or HaveNameMatching() should be used"
        )]
        public TRuleTypeShouldConjunction HaveName(string pattern, bool useRegularExpressions)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveName(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveNameMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either HaveFullName() without the useRegularExpressions parameter or HaveFullNameMatching() should be used"
        )]
        public TRuleTypeShouldConjunction HaveFullName(string pattern, bool useRegularExpressions)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveFullName(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullName(string fullName)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullName(fullName));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveFullNameMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveNameStartingWith(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveNameEndingWith(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveNameContaining(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFullNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.HaveFullNameContaining(pattern)
            );
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

        public ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > DependOnAnyTypesThat()
        {
            _ruleCreator.BeginComplexCondition(
                ArchRuleDefinition.Types(true),
                ObjectConditionsDefinition<TRuleType>.DependOnAnyTypesThat()
            );
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(
                _ruleCreator
            );
        }

        public ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > OnlyDependOnTypesThat()
        {
            _ruleCreator.BeginComplexCondition(
                ArchRuleDefinition.Types(true),
                ObjectConditionsDefinition<TRuleType>.OnlyDependOnTypesThat()
            );
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(
                _ruleCreator
            );
        }

        public ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > HaveAnyAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(
                Attributes(true),
                ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesThat()
            );
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(
                _ruleCreator
            );
        }

        public ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > OnlyHaveAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(
                Attributes(true),
                ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributesThat()
            );
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(
                _ruleCreator
            );
        }

        //Negations


        public TRuleTypeShouldConjunction NotExist()
        {
            _ruleCreator.RequirePositiveResults = false;
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotExist());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBe(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotBe(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBe(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotBe(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotBe(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBe(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(
            ICanBeAnalyzed firstObject,
            params ICanBeAnalyzed[] moreObjects
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBe(firstObject, moreObjects)
            );
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

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotCallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotCallAny(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotCallAny(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotCallAny(MethodMembers().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotCallAny(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotCallAny(patterns, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotCallAny(
            MethodMember method,
            params MethodMember[] moreMethods
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotCallAny(method, moreMethods)
            );
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

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotDependOnAny(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotDependOnAny(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotDependOnAny(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotDependOnAny(Types().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotDependOnAny(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotDependOnAny(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotDependOnAny(firstType, moreTypes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotDependOnAny(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotDependOnAny(firstType, moreTypes)
            );
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

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotHaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(
            string pattern,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use NotHaveAnyAttributes(Attributes().That().HaveFullName()) instead"
        )]
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(
                    patterns,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(
            Attribute firstAttribute,
            params Attribute[] moreAttributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(
                    firstAttribute,
                    moreAttributes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(
            Type firstAttribute,
            params Type[] moreAttributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(
                    firstAttribute,
                    moreAttributes
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(
            IObjectProvider<Attribute> attributes
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<Attribute> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<Type> attributes)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithArguments(
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithArguments(
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithArguments(
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithArguments(
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(
            string attribute,
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(
                    attribute,
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(
            string attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(
                    attribute,
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(
            Attribute attribute,
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(
                    attribute,
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(
            Attribute attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(
                    attribute,
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(
            Type attribute,
            IEnumerable<object> argumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(
                    attribute,
                    argumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(
            Type attribute,
            object firstArgumentValue,
            params object[] moreArgumentValues
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(
                    attribute,
                    firstArgumentValue,
                    moreArgumentValues
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithNamedArguments(
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithNamedArguments(
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithNamedArguments(
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithNamedArguments(
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(
            string attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(
                    attribute,
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(
            string attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(
                    attribute,
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(
            Attribute attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(
                    attribute,
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(
            Attribute attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(
                    attribute,
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(
            Type attribute,
            IEnumerable<(string, object)> attributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(
                    attribute,
                    attributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(
            Type attribute,
            (string, object) firstAttributeArgument,
            params (string, object)[] moreAttributeArguments
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(
                    attribute,
                    firstAttributeArgument,
                    moreAttributeArguments
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either NotHaveName() without the useRegularExpressions parameter or NotHaveNameMatching() should be used"
        )]
        public TRuleTypeShouldConjunction NotHaveName(string pattern, bool useRegularExpressions)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveName(pattern, useRegularExpressions)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveName(string name)
        {
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveNameMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        [Obsolete(
            "Either NotHaveFullName() without the useRegularExpressions parameter or NotHaveFullNameMatching() should be used"
        )]
        public TRuleTypeShouldConjunction NotHaveFullName(
            string pattern,
            bool useRegularExpressions
        )
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveFullName(
                    pattern,
                    useRegularExpressions
                )
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullName(string fullName)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveFullName(fullName)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullNameMatching(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveFullNameMatching(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveNameStartingWith(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveNameEndingWith(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveNameContaining(pattern)
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotHaveFullNameContaining(string pattern)
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotHaveFullNameContaining(pattern)
            );
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
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBeProtectedInternal()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBePrivateProtected()
        {
            _ruleCreator.AddCondition(
                ObjectConditionsDefinition<TRuleType>.NotBePrivateProtected()
            );
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        //Relation Condition Negations

        public ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > NotDependOnAnyTypesThat()
        {
            _ruleCreator.BeginComplexCondition(
                ArchRuleDefinition.Types(true),
                ObjectConditionsDefinition<TRuleType>.NotDependOnAnyTypesThat()
            );
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(
                _ruleCreator
            );
        }

        public ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > NotHaveAnyAttributesThat()
        {
            _ruleCreator.BeginComplexCondition(
                Attributes(true),
                ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesThat()
            );
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(
                _ruleCreator
            );
        }
    }
}
