using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class AddObjectPredicate<TNextElement, TRelatedType, TRuleType>
        : SyntaxElement<TRelatedType>,
            IAddObjectPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
        where TRelatedType : ICanBeAnalyzed
    {
        internal AddObjectPredicate(IArchRuleCreator<TRelatedType> archRuleCreator)
            : base(archRuleCreator) { }

        // csharpier-ignore-start
        public TNextElement FollowCustomPredicate(IPredicate<TRuleType> predicate) => CreateNextElement(predicate);
        public TNextElement FollowCustomPredicate(Func<TRuleType, bool> predicate, string description) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.FollowCustomPredicate(predicate, description));

        public TNextElement Are(params ICanBeAnalyzed[] objects) => Are(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement Are(IEnumerable<ICanBeAnalyzed> objects) => Are(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement Are(IObjectProvider<ICanBeAnalyzed> objects) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.Are(objects));

        public TNextElement CallAny(params MethodMember[] methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement CallAny(IEnumerable<MethodMember> methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement CallAny(IObjectProvider<MethodMember> methods) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.CallAny(methods));

        public TNextElement DependOnAny() => DependOnAny(new ObjectProvider<IType>());
        public TNextElement DependOnAny(params IType[] types) => DependOnAny(new ObjectProvider<IType>(types));
        public TNextElement DependOnAny(params Type[] types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TNextElement DependOnAny(IObjectProvider<IType> types) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DependOnAny(types));
        public TNextElement DependOnAny(IEnumerable<IType> types) => DependOnAny(new ObjectProvider<IType>(types));
        public TNextElement DependOnAny(IEnumerable<Type> types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TNextElement OnlyDependOn() => OnlyDependOn(new ObjectProvider<IType>());
        public TNextElement OnlyDependOn(params IType[] types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TNextElement OnlyDependOn(params Type[] types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));
        public TNextElement OnlyDependOn(IObjectProvider<IType> types) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(types));
        public TNextElement OnlyDependOn(IEnumerable<IType> types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TNextElement OnlyDependOn(IEnumerable<Type> types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));

        public TNextElement HaveAnyAttributes() => HaveAnyAttributes(new ObjectProvider<Attribute>());
        public TNextElement HaveAnyAttributes(params Attribute[] attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement HaveAnyAttributes(params Type[] attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TNextElement HaveAnyAttributes(IObjectProvider<Attribute> attributes) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(attributes));
        public TNextElement HaveAnyAttributes(IEnumerable<Attribute> attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement HaveAnyAttributes(IEnumerable<Type> attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TNextElement OnlyHaveAttributes() => OnlyHaveAttributes(new ObjectProvider<Attribute>());
        public TNextElement OnlyHaveAttributes(params Attribute[] attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement OnlyHaveAttributes(params Type[] attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TNextElement OnlyHaveAttributes(IObjectProvider<Attribute> attributes) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(attributes));
        public TNextElement OnlyHaveAttributes(IEnumerable<Attribute> attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement OnlyHaveAttributes(IEnumerable<Type> attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TNextElement HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithArguments(argumentValues));
        public TNextElement HaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithArguments(new[] { firstArgumentValue }.Concat(moreArgumentValues)));

        public TNextElement HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TNextElement HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TNextElement HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(attributeArguments));
        public TNextElement HaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => HaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TNextElement HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TNextElement HaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TNextElement HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TNextElement HaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => HaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TNextElement HaveName(string name) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveName(name));
        public TNextElement HaveNameMatching(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveNameMatching(pattern));
        public TNextElement HaveNameStartingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveNameStartingWith(pattern));
        public TNextElement HaveNameEndingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveNameEndingWith(pattern));
        public TNextElement HaveNameContaining(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveNameContaining(pattern));

        public TNextElement HaveFullName(string fullName) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveFullName(fullName));
        public TNextElement HaveFullNameMatching(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveFullNameMatching(pattern));
        public TNextElement HaveFullNameStartingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveFullNameStartingWith(pattern));
        public TNextElement HaveFullNameEndingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveFullNameEndingWith(pattern));
        public TNextElement HaveFullNameContaining(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveFullNameContaining(pattern));

        public TNextElement HaveAssemblyQualifiedName(string assemblyQualifiedName) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedName(assemblyQualifiedName));
        public TNextElement HaveAssemblyQualifiedNameMatching(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameMatching(pattern));
        public TNextElement HaveAssemblyQualifiedNameStartingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameStartingWith(pattern));
        public TNextElement HaveAssemblyQualifiedNameEndingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameEndingWith(pattern));
        public TNextElement HaveAssemblyQualifiedNameContaining(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameContaining(pattern));

        public TNextElement ArePrivate() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.ArePrivate());
        public TNextElement ArePublic() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.ArePublic());
        public TNextElement AreProtected() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreProtected());
        public TNextElement AreInternal() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreInternal());
        public TNextElement AreProtectedInternal() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreProtectedInternal());
        public TNextElement ArePrivateProtected() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.ArePrivateProtected());

        // Negations

        public TNextElement AreNot(params ICanBeAnalyzed[] objects) => AreNot(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement AreNot(IEnumerable<ICanBeAnalyzed> objects) => AreNot(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TNextElement AreNot(IObjectProvider<ICanBeAnalyzed> objects) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNot(objects));

        public TNextElement DoNotCallAny(params MethodMember[] methods) => DoNotCallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement DoNotCallAny(IEnumerable<MethodMember> methods) => DoNotCallAny(new ObjectProvider<MethodMember>(methods));
        public TNextElement DoNotCallAny(IObjectProvider<MethodMember> methods) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(methods));

        public TNextElement DoNotDependOnAny() => DoNotDependOnAny(new ObjectProvider<IType>());
        public TNextElement DoNotDependOnAny(params IType[] types) => DoNotDependOnAny(new ObjectProvider<IType>(types));
        public TNextElement DoNotDependOnAny(params Type[] types) => DoNotDependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TNextElement DoNotDependOnAny(IObjectProvider<IType> types) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(types));
        public TNextElement DoNotDependOnAny(IEnumerable<IType> types) => DoNotDependOnAny(new ObjectProvider<IType>(types));
        public TNextElement DoNotDependOnAny(IEnumerable<Type> types) => DoNotDependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TNextElement DoNotHaveAnyAttributes() => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>());
        public TNextElement DoNotHaveAnyAttributes(params Attribute[] attributes) => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement DoNotHaveAnyAttributes(params Type[] attributes) => DoNotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TNextElement DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(attributes));
        public TNextElement DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes) => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TNextElement DoNotHaveAnyAttributes(IEnumerable<Type> attributes) => DoNotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TNextElement DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithArguments(argumentValues));

        public TNextElement DoNotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute.FullName, _ => attribute, argumentValues));
        public TNextElement DoNotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), argumentValues));

        public TNextElement DoNotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithNamedArguments(attributeArguments));
        public TNextElement DoNotHaveAnyAttributesWithNamedArguments(params (string, object)[] attributeArguments) => DoNotHaveAnyAttributesWithNamedArguments((IEnumerable<(string, object)>)attributeArguments);

        public TNextElement DoNotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute.FullName, _ => attribute, attributeArguments));
        public TNextElement DoNotHaveAttributeWithNamedArguments(Attribute attribute, params (string, object)[] attributeArguments) => DoNotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);
        public TNextElement DoNotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute.FullName, architecture => architecture.GetAttributeOfType(attribute), attributeArguments));
        public TNextElement DoNotHaveAttributeWithNamedArguments(Type attribute, params (string, object)[] attributeArguments) => DoNotHaveAttributeWithNamedArguments(attribute, (IEnumerable<(string, object)>)attributeArguments);

        public TNextElement DoNotHaveName(string name) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveName(name));
        public TNextElement DoNotHaveNameMatching(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameMatching(pattern));
        public TNextElement DoNotHaveNameStartingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameStartingWith(pattern));
        public TNextElement DoNotHaveNameEndingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameEndingWith(pattern));
        public TNextElement DoNotHaveNameContaining(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameContaining(pattern));

        public TNextElement DoNotHaveFullName(string fullName) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullName(fullName));
        public TNextElement DoNotHaveFullNameMatching(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameMatching(pattern));
        public TNextElement DoNotHaveFullNameStartingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameStartingWith(pattern));
        public TNextElement DoNotHaveFullNameEndingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameEndingWith(pattern));
        public TNextElement DoNotHaveFullNameContaining(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameContaining(pattern));

        public TNextElement DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedName(assemblyQualifiedName));
        public TNextElement DoNotHaveAssemblyQualifiedNameMatching(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameMatching(pattern));
        public TNextElement DoNotHaveAssemblyQualifiedNameStartingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameStartingWith(pattern));
        public TNextElement DoNotHaveAssemblyQualifiedNameEndingWith(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameEndingWith(pattern));
        public TNextElement DoNotHaveAssemblyQualifiedNameContaining(string pattern) => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameContaining(pattern));

        public TNextElement AreNotPrivate() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNotPrivate());
        public TNextElement AreNotPublic() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNotPublic());
        public TNextElement AreNotProtected() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNotProtected());
        public TNextElement AreNotInternal() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNotInternal());
        public TNextElement AreNotProtectedInternal() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNotProtectedInternal());
        public TNextElement AreNotPrivateProtected() => CreateNextElement(ObjectPredicatesDefinition<TRuleType>.AreNotPrivateProtected());
        // csharpier-ignore-end

        protected abstract TNextElement CreateNextElement(IPredicate<TRuleType> predicate);
    }
}
