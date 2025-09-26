using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
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

        // csharpier-ignore-start
        public TRuleTypeShouldConjunction Be(params ICanBeAnalyzed[] objects) => Be(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction Be(IEnumerable<ICanBeAnalyzed> objects) => Be(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction Be(IObjectProvider<ICanBeAnalyzed> objects) => AddCondition(ObjectConditionsDefinition<TRuleType>.Be(objects));

        public TRuleTypeShouldConjunction CallAny(params MethodMember[] methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction CallAny(IEnumerable<MethodMember> methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction CallAny(IObjectProvider<MethodMember> methods) => AddCondition(ObjectConditionsDefinition<TRuleType>.CallAny(methods));

        public TRuleTypeShouldConjunction DependOnAny() => DependOnAny(new ObjectProvider<IType>());
        public TRuleTypeShouldConjunction DependOnAny(params IType[] types) => DependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DependOnAny(params Type[] types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DependOnAny(IObjectProvider<IType> types) => AddCondition(ObjectConditionsDefinition<TRuleType>.DependOnAny(types));
        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<IType> types) => DependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<Type> types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TRuleTypeShouldConjunction FollowCustomCondition(ICondition<TRuleType> condition) => AddCondition(condition);
        public TRuleTypeShouldConjunction FollowCustomCondition(Func<TRuleType, ConditionResult> condition, string description) => AddCondition(ObjectConditionsDefinition<TRuleType>.FollowCustomCondition(condition, description));
        public TRuleTypeShouldConjunction FollowCustomCondition(Func<TRuleType, bool> condition, string description, string failDescription) => AddCondition(ObjectConditionsDefinition<TRuleType>.FollowCustomCondition(condition, description, failDescription));

        public TRuleTypeShouldConjunction OnlyDependOn() => OnlyDependOn(new ObjectProvider<IType>());
        public TRuleTypeShouldConjunction OnlyDependOn(params IType[] types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction OnlyDependOn(params Type[] types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types) => AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(types));
        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));

        public TRuleTypeShouldConjunction HaveAnyAttributes() => HaveAnyAttributes(new ObjectProvider<Attribute>());
        public TRuleTypeShouldConjunction HaveAnyAttributes(params Attribute[] attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(params Type[] attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Type> attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TRuleTypeShouldConjunction OnlyHaveAttributes() => OnlyHaveAttributes(new ObjectProvider<Attribute>());
        public TRuleTypeShouldConjunction OnlyHaveAttributes(params Attribute[] attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(params Type[] attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes) => AddCondition(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Type> attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TRuleTypeShouldConjunction HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithArguments(argumentValues));

        public TRuleTypeShouldConjunction HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TRuleTypeShouldConjunction HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TRuleTypeShouldConjunction HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(attributeArguments));
        public TRuleTypeShouldConjunction HaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => HaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction HaveName(string name) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveName(name));
        public TRuleTypeShouldConjunction HaveNameMatching(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameMatching(pattern));
        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameStartingWith(pattern));
        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameEndingWith(pattern));
        public TRuleTypeShouldConjunction HaveNameContaining(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveNameContaining(pattern));

        public TRuleTypeShouldConjunction HaveFullName(string fullName) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullName(fullName));
        public TRuleTypeShouldConjunction HaveFullNameMatching(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullNameMatching(pattern));
        public TRuleTypeShouldConjunction HaveFullNameStartingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullNameStartingWith(pattern));
        public TRuleTypeShouldConjunction HaveFullNameEndingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullNameEndingWith(pattern));
        public TRuleTypeShouldConjunction HaveFullNameContaining(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveFullNameContaining(pattern));

        public TRuleTypeShouldConjunction HaveAssemblyQualifiedName(string assemblyQualifiedName) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedName(assemblyQualifiedName));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameMatching(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameMatching(pattern));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameStartingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameStartingWith(pattern));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameEndingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameEndingWith(pattern));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameContaining(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameContaining(pattern));

        public TRuleTypeShouldConjunction BePrivate() => AddCondition(ObjectConditionsDefinition<TRuleType>.BePrivate());
        public TRuleTypeShouldConjunction BePublic() => AddCondition(ObjectConditionsDefinition<TRuleType>.BePublic());
        public TRuleTypeShouldConjunction BeProtected() => AddCondition(ObjectConditionsDefinition<TRuleType>.BeProtected());
        public TRuleTypeShouldConjunction BeInternal() => AddCondition(ObjectConditionsDefinition<TRuleType>.BeInternal());
        public TRuleTypeShouldConjunction BeProtectedInternal() => AddCondition(ObjectConditionsDefinition<TRuleType>.BeProtectedInternal());
        public TRuleTypeShouldConjunction BePrivateProtected() => AddCondition(ObjectConditionsDefinition<TRuleType>.BePrivateProtected());

        // Relation Conditions

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> DependOnAnyTypesThat() => BeginComplexTypeCondition(ObjectConditionsDefinition<TRuleType>.DependOnAnyTypesThat());
        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> OnlyDependOnTypesThat() => BeginComplexTypeCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOnTypesThat());

        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> HaveAnyAttributesThat() => BeginComplexAttributeCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesThat());
        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> OnlyHaveAttributesThat() => BeginComplexAttributeCondition(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributesThat());

        // Negations

        public TRuleTypeShouldConjunction NotExist()
        {
            _ruleCreator.RequirePositiveResults = false;
            _ruleCreator.AddCondition(ObjectConditionsDefinition<TRuleType>.NotExist());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction NotBe(params ICanBeAnalyzed[] objects) => NotBe(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction NotBe(IEnumerable<ICanBeAnalyzed> objects) => NotBe(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction NotBe(IObjectProvider<ICanBeAnalyzed> objects) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBe(objects));

        public TRuleTypeShouldConjunction NotCallAny(params MethodMember[] methods) => NotCallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction NotCallAny(IEnumerable<MethodMember> methods) => NotCallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction NotCallAny(IObjectProvider<MethodMember> methods) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotCallAny(methods));

        public TRuleTypeShouldConjunction NotDependOnAny() => NotDependOnAny(new ObjectProvider<IType>());
        public TRuleTypeShouldConjunction NotDependOnAny(params IType[] types) => NotDependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction NotDependOnAny(params Type[] types) => NotDependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction NotDependOnAny(IObjectProvider<IType> types) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(types));
        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<IType> types) => NotDependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction NotDependOnAny(IEnumerable<Type> types) => NotDependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TRuleTypeShouldConjunction NotHaveAnyAttributes() => NotHaveAnyAttributes(new ObjectProvider<Attribute>());
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(params Attribute[] attributes) => NotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(params Type[] attributes) => NotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IObjectProvider<Attribute> attributes) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes));
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<Attribute> attributes) => NotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction NotHaveAnyAttributes(IEnumerable<Type> attributes) => NotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithArguments(argumentValues));

        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TRuleTypeShouldConjunction NotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithNamedArguments(attributeArguments));
        public TRuleTypeShouldConjunction NotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => NotHaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => NotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TRuleTypeShouldConjunction NotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => NotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction NotHaveName(string name) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveName(name));
        public TRuleTypeShouldConjunction NotHaveNameMatching(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameMatching(pattern));
        public TRuleTypeShouldConjunction NotHaveNameStartingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameStartingWith(pattern));
        public TRuleTypeShouldConjunction NotHaveNameEndingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameEndingWith(pattern));
        public TRuleTypeShouldConjunction NotHaveNameContaining(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveNameContaining(pattern));

        public TRuleTypeShouldConjunction NotHaveFullName(string fullName) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullName(fullName));
        public TRuleTypeShouldConjunction NotHaveFullNameMatching(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameMatching(pattern));
        public TRuleTypeShouldConjunction NotHaveFullNameStartingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameStartingWith(pattern));
        public TRuleTypeShouldConjunction NotHaveFullNameEndingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameEndingWith(pattern));
        public TRuleTypeShouldConjunction NotHaveFullNameContaining(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameContaining(pattern));

        public TRuleTypeShouldConjunction NotHaveAssemblyQualifiedName(string assemblyQualifiedName) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedName(assemblyQualifiedName));
        public TRuleTypeShouldConjunction NotHaveAssemblyQualifiedNameMatching(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameMatching(pattern));
        public TRuleTypeShouldConjunction NotHaveAssemblyQualifiedNameStartingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameStartingWith(pattern));
        public TRuleTypeShouldConjunction NotHaveAssemblyQualifiedNameEndingWith(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameEndingWith(pattern));
        public TRuleTypeShouldConjunction NotHaveAssemblyQualifiedNameContaining(string pattern) => AddCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameContaining(pattern));

        public TRuleTypeShouldConjunction NotBePrivate() => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBePrivate());
        public TRuleTypeShouldConjunction NotBePublic() => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBePublic());
        public TRuleTypeShouldConjunction NotBeProtected() => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBeProtected());
        public TRuleTypeShouldConjunction NotBeInternal() => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBeInternal());
        public TRuleTypeShouldConjunction NotBeProtectedInternal() => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBeProtectedInternal());
        public TRuleTypeShouldConjunction NotBePrivateProtected() => AddCondition(ObjectConditionsDefinition<TRuleType>.NotBePrivateProtected());

        // Relation Condition Negations

        public ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType> NotDependOnAnyTypesThat() => BeginComplexTypeCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAnyTypesThat());
        public ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType> NotHaveAnyAttributesThat() => BeginComplexAttributeCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesThat());
        // csharpier-ignore-end

        private TRuleTypeShouldConjunction AddCondition(ICondition<TRuleType> condition)
        {
            _ruleCreator.AddCondition(condition);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        private ShouldRelateToTypesThat<
            TRuleTypeShouldConjunction,
            IType,
            TRuleType
        > BeginComplexTypeCondition(RelationCondition<TRuleType, IType> relationCondition)
        {
            _ruleCreator.BeginComplexCondition(ArchRuleDefinition.Types(true), relationCondition);
            return new ShouldRelateToTypesThat<TRuleTypeShouldConjunction, IType, TRuleType>(
                _ruleCreator
            );
        }

        private ShouldRelateToAttributesThat<
            TRuleTypeShouldConjunction,
            TRuleType
        > BeginComplexAttributeCondition(RelationCondition<TRuleType, Attribute> relationCondition)
        {
            _ruleCreator.BeginComplexCondition(Attributes(true), relationCondition);
            return new ShouldRelateToAttributesThat<TRuleTypeShouldConjunction, TRuleType>(
                _ruleCreator
            );
        }
    }
}
