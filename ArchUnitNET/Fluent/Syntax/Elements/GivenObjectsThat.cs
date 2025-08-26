using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>
        : SyntaxElement<TRuleType>,
            IObjectPredicates<TGivenRuleTypeConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TGivenRuleTypeConjunction FollowCustomPredicate(IPredicate<TRuleType> predicate) => AddPredicate(predicate);
        public TGivenRuleTypeConjunction FollowCustomPredicate(Func<TRuleType, bool> predicate, string description) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.FollowCustomPredicate(predicate, description));

        public TGivenRuleTypeConjunction Are(params ICanBeAnalyzed[] objects) => Are(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TGivenRuleTypeConjunction Are(IEnumerable<ICanBeAnalyzed> objects) => Are(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TGivenRuleTypeConjunction Are(IObjectProvider<ICanBeAnalyzed> objects) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.Are(objects));

        public TGivenRuleTypeConjunction CallAny(params MethodMember[] methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TGivenRuleTypeConjunction CallAny(IEnumerable<MethodMember> methods) => CallAny(new ObjectProvider<MethodMember>(methods));
        public TGivenRuleTypeConjunction CallAny(IObjectProvider<MethodMember> methods) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.CallAny(methods));

        public TGivenRuleTypeConjunction DependOnAny() => DependOnAny(new ObjectProvider<IType>());
        public TGivenRuleTypeConjunction DependOnAny(params IType[] types) => DependOnAny(new ObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction DependOnAny(params Type[] types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction DependOnAny(IObjectProvider<IType> types) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DependOnAny(types));
        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<IType> types) => DependOnAny(new ObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction DependOnAny(IEnumerable<Type> types) => DependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TGivenRuleTypeConjunction OnlyDependOn() => OnlyDependOn(new ObjectProvider<IType>());
        public TGivenRuleTypeConjunction OnlyDependOn(params IType[] types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction OnlyDependOn(params Type[] types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction OnlyDependOn(IObjectProvider<IType> types) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyDependOn(types));
        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<IType> types) => OnlyDependOn(new ObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction OnlyDependOn(IEnumerable<Type> types) => OnlyDependOn(new SystemTypeObjectProvider<IType>(types));

        public TGivenRuleTypeConjunction HaveAnyAttributes() => HaveAnyAttributes(new ObjectProvider<Attribute>());
        public TGivenRuleTypeConjunction HaveAnyAttributes(params Attribute[] attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction HaveAnyAttributes(params Type[] attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction HaveAnyAttributes(IObjectProvider<Attribute> attributes) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributes(attributes));
        public TGivenRuleTypeConjunction HaveAnyAttributes(IEnumerable<Attribute> attributes) => HaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction HaveAnyAttributes(IEnumerable<Type> attributes) => HaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TGivenRuleTypeConjunction OnlyHaveAttributes() => OnlyHaveAttributes(new ObjectProvider<Attribute>());
        public TGivenRuleTypeConjunction OnlyHaveAttributes(params Attribute[] attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction OnlyHaveAttributes(params Type[] attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction OnlyHaveAttributes(IObjectProvider<Attribute> attributes) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.OnlyHaveAttributes(attributes));
        public TGivenRuleTypeConjunction OnlyHaveAttributes(IEnumerable<Attribute> attributes) => OnlyHaveAttributes(new ObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction OnlyHaveAttributes(IEnumerable<Type> attributes) => OnlyHaveAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TGivenRuleTypeConjunction HaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithArguments(argumentValues));
        public TGivenRuleTypeConjunction HaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithArguments(new[] { firstArgumentValue }.Concat(moreArgumentValues)));

        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute, argumentValues));
        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Attribute attribute, object firstArgumentValue, params object[] moreArgumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute, new[] { firstArgumentValue }.Concat(moreArgumentValues)));
        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute, argumentValues));
        public TGivenRuleTypeConjunction HaveAttributeWithArguments(Type attribute, object firstArgumentValue, params object[] moreArgumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithArguments(attribute, new[] { firstArgumentValue }.Concat(moreArgumentValues)));

        public TGivenRuleTypeConjunction HaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAnyAttributesWithNamedArguments(attributeArguments));
        public TGivenRuleTypeConjunction HaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments) => HaveAnyAttributesWithNamedArguments(new[] { firstAttributeArgument }.Concat(moreAttributeArguments));

        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute, attributeArguments));
        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Attribute attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments) => HaveAttributeWithNamedArguments(attribute, new[] { firstAttributeArgument }.Concat(moreAttributeArguments));
        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAttributeWithNamedArguments(attribute, attributeArguments));
        public TGivenRuleTypeConjunction HaveAttributeWithNamedArguments(Type attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments) => HaveAttributeWithNamedArguments(attribute, new[] { firstAttributeArgument }.Concat(moreAttributeArguments));

        public TGivenRuleTypeConjunction HaveName(string name) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveName(name));
        public TGivenRuleTypeConjunction HaveNameMatching(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameMatching(pattern));
        public TGivenRuleTypeConjunction HaveNameStartingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameStartingWith(pattern));
        public TGivenRuleTypeConjunction HaveNameEndingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameEndingWith(pattern));
        public TGivenRuleTypeConjunction HaveNameContaining(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveNameContaining(pattern));

        public TGivenRuleTypeConjunction HaveFullName(string fullName) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullName(fullName));
        public TGivenRuleTypeConjunction HaveFullNameMatching(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameMatching(pattern));
        public TGivenRuleTypeConjunction HaveFullNameStartingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameStartingWith(pattern));
        public TGivenRuleTypeConjunction HaveFullNameEndingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameEndingWith(pattern));
        public TGivenRuleTypeConjunction HaveFullNameContaining(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveFullNameContaining(pattern));

        public TGivenRuleTypeConjunction HaveAssemblyQualifiedName(string assemblyQualifiedName) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedName(assemblyQualifiedName));
        public TGivenRuleTypeConjunction HaveAssemblyQualifiedNameMatching(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameMatching(pattern));
        public TGivenRuleTypeConjunction HaveAssemblyQualifiedNameStartingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameStartingWith(pattern));
        public TGivenRuleTypeConjunction HaveAssemblyQualifiedNameEndingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameEndingWith(pattern));
        public TGivenRuleTypeConjunction HaveAssemblyQualifiedNameContaining(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.HaveAssemblyQualifiedNameContaining(pattern));

        public TGivenRuleTypeConjunction ArePrivate() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.ArePrivate());
        public TGivenRuleTypeConjunction ArePublic() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.ArePublic());
        public TGivenRuleTypeConjunction AreProtected() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreProtected());
        public TGivenRuleTypeConjunction AreInternal() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreInternal());
        public TGivenRuleTypeConjunction AreProtectedInternal() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreProtectedInternal());
        public TGivenRuleTypeConjunction ArePrivateProtected() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.ArePrivateProtected());

        // Negations

        public TGivenRuleTypeConjunction AreNot(params ICanBeAnalyzed[] objects) => AreNot(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TGivenRuleTypeConjunction AreNot(IEnumerable<ICanBeAnalyzed> objects) => AreNot(new ObjectProvider<ICanBeAnalyzed>(objects));
        public TGivenRuleTypeConjunction AreNot(IObjectProvider<ICanBeAnalyzed> objects) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNot(objects));

        public TGivenRuleTypeConjunction DoNotCallAny(params MethodMember[] methods) => DoNotCallAny(new ObjectProvider<MethodMember>(methods));
        public TGivenRuleTypeConjunction DoNotCallAny(IEnumerable<MethodMember> methods) => DoNotCallAny(new ObjectProvider<MethodMember>(methods));
        public TGivenRuleTypeConjunction DoNotCallAny(IObjectProvider<MethodMember> methods) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotCallAny(methods));

        public TGivenRuleTypeConjunction DoNotDependOnAny() => DoNotDependOnAny(new ObjectProvider<IType>());
        public TGivenRuleTypeConjunction DoNotDependOnAny(params IType[] types) => DoNotDependOnAny(new ObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction DoNotDependOnAny(params Type[] types) => DoNotDependOnAny(new SystemTypeObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction DoNotDependOnAny(IObjectProvider<IType> types) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotDependOnAny(types));
        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<IType> types) => DoNotDependOnAny(new ObjectProvider<IType>(types));
        public TGivenRuleTypeConjunction DoNotDependOnAny(IEnumerable<Type> types) => DoNotDependOnAny(new SystemTypeObjectProvider<IType>(types));

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes() => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>());
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(params Attribute[] attributes) => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(params Type[] attributes) => DoNotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IObjectProvider<Attribute> attributes) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributes(attributes));
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IEnumerable<Attribute> attributes) => DoNotHaveAnyAttributes(new ObjectProvider<Attribute>(attributes));
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributes(IEnumerable<Type> attributes) => DoNotHaveAnyAttributes(new SystemTypeObjectProvider<Attribute>(attributes));

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithArguments(IEnumerable<object> argumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithArguments(argumentValues));
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithArguments(object firstArgumentValue, params object[] moreArgumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithArguments(new[] { firstArgumentValue }.Concat(moreArgumentValues)));

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Attribute attribute, IEnumerable<object> argumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute, argumentValues));
        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Attribute attribute, object firstArgumentValue, params object[] moreArgumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute, new[] { firstArgumentValue }.Concat(moreArgumentValues)));
        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Type attribute, IEnumerable<object> argumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute, argumentValues));
        public TGivenRuleTypeConjunction DoNotHaveAttributeWithArguments(Type attribute, object firstArgumentValue, params object[] moreArgumentValues) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithArguments(attribute, new[] { firstArgumentValue }.Concat(moreArgumentValues)));

        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithNamedArguments(IEnumerable<(string, object)> attributeArguments) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAnyAttributesWithNamedArguments(attributeArguments));
        public TGivenRuleTypeConjunction DoNotHaveAnyAttributesWithNamedArguments((string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments) => DoNotHaveAnyAttributesWithNamedArguments(new[] { firstAttributeArgument }.Concat(moreAttributeArguments));

        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Attribute attribute, IEnumerable<(string, object)> attributeArguments) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute, attributeArguments));
        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Attribute attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments) => DoNotHaveAttributeWithNamedArguments(attribute, new[] { firstAttributeArgument }.Concat(moreAttributeArguments));
        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Type attribute, IEnumerable<(string, object)> attributeArguments) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAttributeWithNamedArguments(attribute, attributeArguments));
        public TGivenRuleTypeConjunction DoNotHaveAttributeWithNamedArguments(Type attribute, (string, object) firstAttributeArgument, params (string, object)[] moreAttributeArguments) => DoNotHaveAttributeWithNamedArguments(attribute, new[] { firstAttributeArgument }.Concat(moreAttributeArguments));

        public TGivenRuleTypeConjunction DoNotHaveName(string name) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveName(name));
        public TGivenRuleTypeConjunction DoNotHaveNameMatching(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameMatching(pattern));
        public TGivenRuleTypeConjunction DoNotHaveNameStartingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameStartingWith(pattern));
        public TGivenRuleTypeConjunction DoNotHaveNameEndingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameEndingWith(pattern));
        public TGivenRuleTypeConjunction DoNotHaveNameContaining(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveNameContaining(pattern));

        public TGivenRuleTypeConjunction DoNotHaveFullName(string fullName) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullName(fullName));
        public TGivenRuleTypeConjunction DoNotHaveFullNameMatching(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameMatching(pattern));
        public TGivenRuleTypeConjunction DoNotHaveFullNameStartingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameStartingWith(pattern));
        public TGivenRuleTypeConjunction DoNotHaveFullNameEndingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameEndingWith(pattern));
        public TGivenRuleTypeConjunction DoNotHaveFullNameContaining(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveFullNameContaining(pattern));

        public TGivenRuleTypeConjunction DoNotHaveAssemblyQualifiedName(string assemblyQualifiedName) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedName(assemblyQualifiedName));
        public TGivenRuleTypeConjunction DoNotHaveAssemblyQualifiedNameMatching(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameMatching(pattern));
        public TGivenRuleTypeConjunction DoNotHaveAssemblyQualifiedNameStartingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameStartingWith(pattern));
        public TGivenRuleTypeConjunction DoNotHaveAssemblyQualifiedNameEndingWith(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameEndingWith(pattern));
        public TGivenRuleTypeConjunction DoNotHaveAssemblyQualifiedNameContaining(string pattern) => AddPredicate(ObjectPredicatesDefinition<TRuleType>.DoNotHaveAssemblyQualifiedNameContaining(pattern));

        public TGivenRuleTypeConjunction AreNotPrivate() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotPrivate());
        public TGivenRuleTypeConjunction AreNotPublic() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotPublic());
        public TGivenRuleTypeConjunction AreNotProtected() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotProtected());
        public TGivenRuleTypeConjunction AreNotInternal() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotInternal());
        public TGivenRuleTypeConjunction AreNotProtectedInternal() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotProtectedInternal());
        public TGivenRuleTypeConjunction AreNotPrivateProtected() => AddPredicate(ObjectPredicatesDefinition<TRuleType>.AreNotPrivateProtected());
        // csharpier-ignore-end

        private TGivenRuleTypeConjunction AddPredicate(IPredicate<TRuleType> predicate)
        {
            _ruleCreator.AddPredicate(predicate);
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}
