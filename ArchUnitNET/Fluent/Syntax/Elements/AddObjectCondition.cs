using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;
using JetBrains.Annotations;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class AddObjectCondition<TNextElement, TRuleType>
        : PartialConditionConjunction<TNextElement, TRuleType>,
            IAddObjectCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected AddObjectCondition(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override string Description { get; } = "";

        // csharpier-ignore-start

        public TNextElement Exist() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.Exist());

        public TNextElement Be(params ICanBeAnalyzed[] objects) => Be(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement Be(IEnumerable<ICanBeAnalyzed> objects) => Be(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement Be(IObjectProvider<ICanBeAnalyzed> objects) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.Be(objects));

        public TNextElement CallAny(params MethodMember[] methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement CallAny(IEnumerable<MethodMember> methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement CallAny(IObjectProvider<MethodMember> methods) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.CallAny(methods));

        public TNextElement DependOnAny() => DependOnAny(new ObjectProvider<IType>());
        public TNextElement DependOnAny(params IType[] types) => DependOnAny(new ObjectProvider<IType>(types));
        public TNextElement DependOnAny(params Type[] types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TNextElement DependOnAny(IObjectProvider<IType> types) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.DependOnAny(types));
        public TNextElement DependOnAny(IEnumerable<IType> types) => DependOnAny(new ObjectProvider<IType>(types));
        public TNextElement DependOnAny(IEnumerable<Type> types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TNextElement FollowCustomCondition(ICondition<TRuleType> condition) => CreateNextElement(condition.AsOrderedCondition());
        public TNextElement FollowCustomCondition(Func<TRuleType, ConditionResult> condition, string description) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.FollowCustomCondition(condition, description));
        public TNextElement FollowCustomCondition(Func<TRuleType, bool> condition, string description, string failDescription) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.FollowCustomCondition(condition, description, failDescription));

        public TNextElement OnlyDependOn() => OnlyDependOn(new ObjectProvider<IType>());
        public TNextElement OnlyDependOn(params IType[] types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TNextElement OnlyDependOn(params Type[] types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));
        public TNextElement OnlyDependOn(IObjectProvider<IType> types) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.OnlyDependOn(types));
        public TNextElement OnlyDependOn(IEnumerable<IType> types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TNextElement OnlyDependOn(IEnumerable<Type> types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));

        public TNextElement HaveAnyAttributes() => HaveAnyAttributes(new ObjectProvider<Attribute>());
        public TNextElement HaveAnyAttributes(params Attribute[] attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement HaveAnyAttributes(params Type[] attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TNextElement HaveAnyAttributes(IObjectProvider<Attribute> attributes) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributes(attributes));
        public TNextElement HaveAnyAttributes(IEnumerable<Attribute> attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement HaveAnyAttributes(IEnumerable<Type> attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TNextElement OnlyHaveAttributes() => OnlyHaveAttributes(new ObjectProvider<Attribute>());
        public TNextElement OnlyHaveAttributes(params Attribute[] attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement OnlyHaveAttributes(params Type[] attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TNextElement OnlyHaveAttributes(IObjectProvider<Attribute> attributes) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributes(attributes));
        public TNextElement OnlyHaveAttributes(IEnumerable<Attribute> attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement OnlyHaveAttributes(IEnumerable<Type> attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TNextElement HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithArguments(argumentValues));

        public TNextElement HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TNextElement HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TNextElement HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(attributeArguments));
        public TNextElement HaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => HaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TNextElement HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TNextElement HaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TNextElement HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TNextElement HaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TNextElement HaveName(string name) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveName(name));
        public TNextElement HaveNameMatching(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveNameMatching(pattern));
        public TNextElement HaveNameStartingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveNameStartingWith(pattern));
        public TNextElement HaveNameEndingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveNameEndingWith(pattern));
        public TNextElement HaveNameContaining(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveNameContaining(pattern));

        public TNextElement HaveFullName(string fullName) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveFullName(fullName));
        public TNextElement HaveFullNameMatching(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveFullNameMatching(pattern));
        public TNextElement HaveFullNameStartingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveFullNameStartingWith(pattern));
        public TNextElement HaveFullNameEndingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveFullNameEndingWith(pattern));
        public TNextElement HaveFullNameContaining(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveFullNameContaining(pattern));

        public TNextElement HaveAssemblyQualifiedName(string assemblyQualifiedName) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedName(assemblyQualifiedName));
        public TNextElement HaveAssemblyQualifiedNameMatching(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameMatching(pattern));
        public TNextElement HaveAssemblyQualifiedNameStartingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameStartingWith(pattern));
        public TNextElement HaveAssemblyQualifiedNameEndingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameEndingWith(pattern));
        public TNextElement HaveAssemblyQualifiedNameContaining(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.HaveAssemblyQualifiedNameContaining(pattern));

        public TNextElement BePrivate() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.BePrivate());
        public TNextElement BePublic() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.BePublic());
        public TNextElement BeProtected() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.BeProtected());
        public TNextElement BeInternal() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.BeInternal());
        public TNextElement BeProtectedInternal() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.BeProtectedInternal());
        public TNextElement BePrivateProtected() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.BePrivateProtected());

        // Relation Conditions

        public ShouldRelateToTypesThat<TNextElement, TRuleType> DependOnAnyTypesThat() => BeginComplexTypeCondition(ObjectConditionsDefinition<TRuleType>.DependOnAnyTypesThat());
        public ShouldRelateToTypesThat<TNextElement, TRuleType> OnlyDependOnTypesThat() => BeginComplexTypeCondition(ObjectConditionsDefinition<TRuleType>.OnlyDependOnTypesThat());

        public ShouldRelateToAttributesThat<TNextElement, TRuleType> HaveAnyAttributesThat() => BeginComplexAttributeCondition(ObjectConditionsDefinition<TRuleType>.HaveAnyAttributesThat());
        public ShouldRelateToAttributesThat<TNextElement, TRuleType> OnlyHaveAttributesThat() => BeginComplexAttributeCondition(ObjectConditionsDefinition<TRuleType>.OnlyHaveAttributesThat());

        // Negations

        public TNextElement NotExist() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotExist());

        public TNextElement NotBe(params ICanBeAnalyzed[] objects) => NotBe(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement NotBe(IEnumerable<ICanBeAnalyzed> objects) => NotBe(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement NotBe(IObjectProvider<ICanBeAnalyzed> objects) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBe(objects));

        public TNextElement NotCallAny(params MethodMember[] methods) => NotCallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement NotCallAny(IEnumerable<MethodMember> methods) => NotCallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement NotCallAny(IObjectProvider<MethodMember> methods) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotCallAny(methods));

        public TNextElement NotDependOnAny() => NotDependOnAny(new ObjectProvider<IType>());
        public TNextElement NotDependOnAny(params IType[] types) => NotDependOnAny(new ObjectProvider<IType>(types));
        public TNextElement NotDependOnAny(params Type[] types) => NotDependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TNextElement NotDependOnAny(IObjectProvider<IType> types) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotDependOnAny(types));
        public TNextElement NotDependOnAny(IEnumerable<IType> types) => NotDependOnAny(new ObjectProvider<IType>(types));
        public TNextElement NotDependOnAny(IEnumerable<Type> types) => NotDependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TNextElement NotHaveAnyAttributes() => NotHaveAnyAttributes(new ObjectProvider<Attribute>());
        public TNextElement NotHaveAnyAttributes(params Attribute[] attributes) => NotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement NotHaveAnyAttributes(params Type[] attributes) => NotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TNextElement NotHaveAnyAttributes(IObjectProvider<Attribute> attributes) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributes(attributes));
        public TNextElement NotHaveAnyAttributes(IEnumerable<Attribute> attributes) => NotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement NotHaveAnyAttributes(IEnumerable<Type> attributes) => NotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TNextElement NotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithArguments(argumentValues));

        public TNextElement NotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TNextElement NotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TNextElement NotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesWithNamedArguments(attributeArguments));
        public TNextElement NotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => NotHaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TNextElement NotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TNextElement NotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => NotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TNextElement NotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TNextElement NotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => NotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TNextElement NotHaveName(string name) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveName(name));
        public TNextElement NotHaveNameMatching(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveNameMatching(pattern));
        public TNextElement NotHaveNameStartingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveNameStartingWith(pattern));
        public TNextElement NotHaveNameEndingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveNameEndingWith(pattern));
        public TNextElement NotHaveNameContaining(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveNameContaining(pattern));

        public TNextElement NotHaveFullName(string fullName) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveFullName(fullName));
        public TNextElement NotHaveFullNameMatching(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameMatching(pattern));
        public TNextElement NotHaveFullNameStartingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameStartingWith(pattern));
        public TNextElement NotHaveFullNameEndingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameEndingWith(pattern));
        public TNextElement NotHaveFullNameContaining(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveFullNameContaining(pattern));

        public TNextElement NotHaveAssemblyQualifiedName(string assemblyQualifiedName) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedName(assemblyQualifiedName));
        public TNextElement NotHaveAssemblyQualifiedNameMatching(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameMatching(pattern));
        public TNextElement NotHaveAssemblyQualifiedNameStartingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameStartingWith(pattern));
        public TNextElement NotHaveAssemblyQualifiedNameEndingWith(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameEndingWith(pattern));
        public TNextElement NotHaveAssemblyQualifiedNameContaining(string pattern) => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotHaveAssemblyQualifiedNameContaining(pattern));

        public TNextElement NotBePrivate() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBePrivate());
        public TNextElement NotBePublic() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBePublic());
        public TNextElement NotBeProtected() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBeProtected());
        public TNextElement NotBeInternal() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBeInternal());
        public TNextElement NotBeProtectedInternal() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBeProtectedInternal());
        public TNextElement NotBePrivateProtected() => CreateNextElement(ObjectConditionsDefinition<TRuleType>.NotBePrivateProtected());
        // Relation Condition Negations

        public ShouldRelateToTypesThat<TNextElement, TRuleType> NotDependOnAnyTypesThat() => BeginComplexTypeCondition(ObjectConditionsDefinition<TRuleType>.NotDependOnAnyTypesThat());
        public ShouldRelateToAttributesThat<TNextElement, TRuleType> NotHaveAnyAttributesThat() => BeginComplexAttributeCondition(ObjectConditionsDefinition<TRuleType>.NotHaveAnyAttributesThat());
        // csharpier-ignore-end

        protected ShouldRelateToTypesThat<TNextElement, TRuleType> BeginComplexTypeCondition(
            RelationCondition<TRuleType, IType> relationCondition
        ) =>
            new ShouldRelateToTypesThat<TNextElement, TRuleType>(
                PartialArchRuleConjunction,
                ArchRuleDefinition.Types(true).WithDescription("types that"),
                this,
                relationCondition
            );

        protected ShouldRelateToInterfacesThat<
            TNextElement,
            TRuleType
        > BeginComplexInterfaceCondition(
            RelationCondition<TRuleType, Interface> relationCondition
        ) =>
            new ShouldRelateToInterfacesThat<TNextElement, TRuleType>(
                PartialArchRuleConjunction,
                ArchRuleDefinition.Interfaces(true).WithDescription("interfaces that"),
                this,
                relationCondition
            );

        protected ShouldRelateToAttributesThat<
            TNextElement,
            TRuleType
        > BeginComplexAttributeCondition(
            RelationCondition<TRuleType, Attribute> relationCondition
        ) =>
            new ShouldRelateToAttributesThat<TNextElement, TRuleType>(
                PartialArchRuleConjunction,
                ArchRuleDefinition.Attributes(true).WithDescription("attributes that"),
                this,
                relationCondition
            );

        protected ShouldRelateToMethodMembersThat<
            TNextElement,
            TRuleType
        > BeginComplexMethodMemberCondition(
            RelationCondition<TRuleType, MethodMember> relationCondition
        ) =>
            new ShouldRelateToMethodMembersThat<TNextElement, TRuleType>(
                PartialArchRuleConjunction,
                ArchRuleDefinition.MethodMembers().WithDescription("method members that"),
                this,
                relationCondition
            );
    }
}
