using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public class ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>
        : SyntaxElement<TRuleType>,
            IObjectPredicates<TRuleTypeShouldConjunction, TReferenceType>
        where TReferenceType : ICanBeAnalyzed
        where TRuleType : ICanBeAnalyzed
    {
        protected ShouldRelateToObjectsThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TRuleTypeShouldConjunction Are(params ICanBeAnalyzed[] objects) => Are(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction Are(IEnumerable<ICanBeAnalyzed> objects) => Are(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction Are(IObjectProvider<ICanBeAnalyzed> objects) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.Are(objects));

        public TRuleTypeShouldConjunction CallAny(params MethodMember[] methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction CallAny(IEnumerable<MethodMember> methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction CallAny(IObjectProvider<MethodMember> methods) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.CallAny(methods));

        public TRuleTypeShouldConjunction DependOnAny() => DependOnAny(new ObjectProvider<IType>());
        public TRuleTypeShouldConjunction DependOnAny(params IType[] types) => DependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DependOnAny(params Type[] types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DependOnAny(IObjectProvider<IType> types) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DependOnAny(types));
        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<IType> types) => DependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DependOnAny(IEnumerable<Type> types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TRuleTypeShouldConjunction FollowCustomPredicate(IPredicate<TReferenceType> predicate) => ContinueComplexCondition(predicate);
        public TRuleTypeShouldConjunction FollowCustomPredicate(Func<TReferenceType, bool> predicate, string description) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.FollowCustomPredicate(predicate, description));

        public TRuleTypeShouldConjunction OnlyDependOn() => OnlyDependOn(new ObjectProvider<IType>());
        public TRuleTypeShouldConjunction OnlyDependOn(params IType[] types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction OnlyDependOn(params Type[] types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction OnlyDependOn(IObjectProvider<IType> types) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.OnlyDependOn(types));
        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<IType> types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction OnlyDependOn(IEnumerable<Type> types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));

        public TRuleTypeShouldConjunction HaveAnyAttributes() => HaveAnyAttributes(new ObjectProvider<Attribute>());
        public TRuleTypeShouldConjunction HaveAnyAttributes(params Attribute[] attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(params Type[] attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributes(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction HaveAnyAttributes(IEnumerable<Type> attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TRuleTypeShouldConjunction OnlyHaveAttributes() => OnlyHaveAttributes(new ObjectProvider<Attribute>());
        public TRuleTypeShouldConjunction OnlyHaveAttributes(params Attribute[] attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(params Type[] attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.OnlyHaveAttributes(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction OnlyHaveAttributes(IEnumerable<Type> attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TRuleTypeShouldConjunction HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributesWithArguments(argumentValues));

        public TRuleTypeShouldConjunction HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TRuleTypeShouldConjunction HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TRuleTypeShouldConjunction HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAnyAttributesWithNamedArguments(attributeArguments));
        public TRuleTypeShouldConjunction HaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => HaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TRuleTypeShouldConjunction HaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction HaveName(string name) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveName(name));
        public TRuleTypeShouldConjunction HaveNameMatching(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveNameMatching(pattern));
        public TRuleTypeShouldConjunction HaveNameStartingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveNameStartingWith(pattern));
        public TRuleTypeShouldConjunction HaveNameEndingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveNameEndingWith(pattern));
        public TRuleTypeShouldConjunction HaveNameContaining(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveNameContaining(pattern));

        public TRuleTypeShouldConjunction HaveFullName(string fullName) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveFullName(fullName));
        public TRuleTypeShouldConjunction HaveFullNameMatching(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveFullNameMatching(pattern));
        public TRuleTypeShouldConjunction HaveFullNameStartingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveFullNameStartingWith(pattern));
        public TRuleTypeShouldConjunction HaveFullNameEndingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveFullNameEndingWith(pattern));
        public TRuleTypeShouldConjunction HaveFullNameContaining(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveFullNameContaining(pattern));

        public TRuleTypeShouldConjunction HaveAssemblyQualifiedName(string assemblyQualifiedName) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAssemblyQualifiedName(assemblyQualifiedName));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameMatching(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAssemblyQualifiedNameMatching(pattern));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameStartingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAssemblyQualifiedNameStartingWith(pattern));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameEndingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAssemblyQualifiedNameEndingWith(pattern));
        public TRuleTypeShouldConjunction HaveAssemblyQualifiedNameContaining(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.HaveAssemblyQualifiedNameContaining(pattern));

        public TRuleTypeShouldConjunction ArePrivate() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.ArePrivate());
        public TRuleTypeShouldConjunction ArePublic() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.ArePublic());
        public TRuleTypeShouldConjunction AreProtected() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreProtected());
        public TRuleTypeShouldConjunction AreInternal() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreInternal());
        public TRuleTypeShouldConjunction AreProtectedInternal() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreProtectedInternal());
        public TRuleTypeShouldConjunction ArePrivateProtected() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.ArePrivateProtected());

        // Negations

        public TRuleTypeShouldConjunction AreNot(params ICanBeAnalyzed[] objects) => AreNot(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction AreNot(IEnumerable<ICanBeAnalyzed> objects) => AreNot(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TRuleTypeShouldConjunction AreNot(IObjectProvider<ICanBeAnalyzed> objects) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNot(objects));

        public TRuleTypeShouldConjunction DoNotCallAny(params MethodMember[] methods) => DoNotCallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction DoNotCallAny(IEnumerable<MethodMember> methods) => DoNotCallAny(new ObjectProvider<MethodMember>(methods));
        public TRuleTypeShouldConjunction DoNotCallAny(IObjectProvider<MethodMember> methods) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotCallAny(methods));

        public TRuleTypeShouldConjunction DoNotDependOnAny() => DoNotDependOnAny(new ObjectProvider<IType>());
        public TRuleTypeShouldConjunction DoNotDependOnAny(params IType[] types) => DoNotDependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DoNotDependOnAny(params Type[] types) => DoNotDependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DoNotDependOnAny(IObjectProvider<IType> types) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotDependOnAny(types));
        public TRuleTypeShouldConjunction DoNotDependOnAny(IEnumerable<IType> types) => DoNotDependOnAny(new ObjectProvider<IType>(types));
        public TRuleTypeShouldConjunction DoNotDependOnAny(IEnumerable<Type> types) => DoNotDependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes() => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>());
        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(params Attribute[] attributes) => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(params Type[] attributes) => DoNotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributes(attributes));
        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes) => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TRuleTypeShouldConjunction DoNotHaveAnyAttributes(IEnumerable<Type> attributes) => DoNotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributesWithArguments(argumentValues));

        public TRuleTypeShouldConjunction DoNotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TRuleTypeShouldConjunction DoNotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TRuleTypeShouldConjunction DoNotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAnyAttributesWithNamedArguments(attributeArguments));
        public TRuleTypeShouldConjunction DoNotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => DoNotHaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction DoNotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TRuleTypeShouldConjunction DoNotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => DoNotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TRuleTypeShouldConjunction DoNotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TRuleTypeShouldConjunction DoNotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => DoNotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TRuleTypeShouldConjunction DoNotHaveName(string name) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveName(name));
        public TRuleTypeShouldConjunction DoNotHaveNameMatching(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameMatching(pattern));
        public TRuleTypeShouldConjunction DoNotHaveNameStartingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameStartingWith(pattern));
        public TRuleTypeShouldConjunction DoNotHaveNameEndingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameEndingWith(pattern));
        public TRuleTypeShouldConjunction DoNotHaveNameContaining(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveNameContaining(pattern));

        public TRuleTypeShouldConjunction DoNotHaveFullName(string fullName) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullName(fullName));
        public TRuleTypeShouldConjunction DoNotHaveFullNameMatching(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullNameMatching(pattern));
        public TRuleTypeShouldConjunction DoNotHaveFullNameStartingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullNameStartingWith(pattern));
        public TRuleTypeShouldConjunction DoNotHaveFullNameEndingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullNameEndingWith(pattern));
        public TRuleTypeShouldConjunction DoNotHaveFullNameContaining(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveFullNameContaining(pattern));

        public TRuleTypeShouldConjunction DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAssemblyQualifiedName(assemblyQualifiedName));
        public TRuleTypeShouldConjunction DoNotHaveAssemblyQualifiedNameMatching(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAssemblyQualifiedNameMatching(pattern));
        public TRuleTypeShouldConjunction DoNotHaveAssemblyQualifiedNameStartingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAssemblyQualifiedNameStartingWith(pattern));
        public TRuleTypeShouldConjunction DoNotHaveAssemblyQualifiedNameEndingWith(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAssemblyQualifiedNameEndingWith(pattern));
        public TRuleTypeShouldConjunction DoNotHaveAssemblyQualifiedNameContaining(string pattern) => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.DoNotHaveAssemblyQualifiedNameContaining(pattern));

        public TRuleTypeShouldConjunction AreNotPrivate() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotPrivate());
        public TRuleTypeShouldConjunction AreNotPublic() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotPublic());
        public TRuleTypeShouldConjunction AreNotProtected() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotProtected());
        public TRuleTypeShouldConjunction AreNotInternal() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotInternal());
        public TRuleTypeShouldConjunction AreNotProtectedInternal() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotProtectedInternal());
        public TRuleTypeShouldConjunction AreNotPrivateProtected() => ContinueComplexCondition(ObjectPredicatesDefinition<TReferenceType>.AreNotPrivateProtected());
        // csharpier-ignore-end

        private TRuleTypeShouldConjunction ContinueComplexCondition(
            IPredicate<TReferenceType> predicate
        )
        {
            _ruleCreator.ContinueComplexCondition(predicate);
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}
