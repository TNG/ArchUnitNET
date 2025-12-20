using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public abstract class AddTypePredicate<TNextElement, TRuleType>
        : AddObjectPredicate<TNextElement, TRuleType>,
            IAddTypePredicate<TNextElement, TRuleType>
        where TRuleType : IType
    {
        protected AddTypePredicate(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        // csharpier-ignore-start
        public TNextElement Are(Type firstType, params Type[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.Are(firstType, moreTypes));
        public TNextElement Are(IEnumerable<Type> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.Are(types));

        public TNextElement AreAssignableTo(IType firstType, params IType[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreAssignableTo(firstType, moreTypes));
        public TNextElement AreAssignableTo(Type firstType, params Type[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreAssignableTo(firstType, moreTypes));
        public TNextElement AreAssignableTo(IObjectProvider<IType> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreAssignableTo(types));
        public TNextElement AreAssignableTo(IEnumerable<IType> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreAssignableTo(types));
        public TNextElement AreAssignableTo(IEnumerable<Type> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreAssignableTo(types));

        public TNextElement AreNestedIn(IType firstType, params IType[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNestedIn(firstType, moreTypes));
        public TNextElement AreNestedIn(Type firstType, params Type[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNestedIn(firstType, moreTypes));
        public TNextElement AreNestedIn(IObjectProvider<IType> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNestedIn(types));
        public TNextElement AreNestedIn(IEnumerable<IType> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNestedIn(types));
        public TNextElement AreNestedIn(IEnumerable<Type> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNestedIn(types));

        public TNextElement AreValueTypes() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreValueTypes());
        public TNextElement AreEnums() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreEnums());
        public TNextElement AreStructs() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreStructs());

        public TNextElement ImplementInterface(Interface intf) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ImplementInterface(intf));
        public TNextElement ImplementInterface(Type intf) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ImplementInterface(intf));

        public TNextElement ImplementAnyInterfaces() => ImplementAnyInterfaces(new ObjectProvider<Interface>());
        public TNextElement ImplementAnyInterfaces(params Interface[] interfaces) => ImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(params Type[] interfaces) => ImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(IEnumerable<Interface> interfaces) => ImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(IEnumerable<Type> interfaces) => ImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(IObjectProvider<Interface> interfaces) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ImplementAny(interfaces));

        public TNextElement ResideInNamespace(string fullName) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ResideInNamespace(fullName));
        public TNextElement ResideInNamespaceMatching(string pattern) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ResideInNamespaceMatching(pattern));

        public TNextElement ResideInAssembly(string fullName) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ResideInAssembly(fullName));
        public TNextElement ResideInAssemblyMatching(string pattern) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ResideInAssemblyMatching(pattern));
        public TNextElement ResideInAssembly(System.Reflection.Assembly assembly, params System.Reflection.Assembly[] moreAssemblies) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies));
        public TNextElement ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies) => CreateNextElement(TypePredicatesDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies));

        public TNextElement HavePropertyMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.HavePropertyMemberWithName(name));
        public TNextElement HaveFieldMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.HaveFieldMemberWithName(name));
        public TNextElement HaveMethodMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.HaveMethodMemberWithName(name));
        public TNextElement HaveMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.HaveMemberWithName(name));

        public TNextElement AreNested() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNested());

        //Negations

        public TNextElement AreNot(Type firstType, params Type[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNot(firstType, moreTypes));
        public TNextElement AreNot(IEnumerable<Type> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNot(types));

        public TNextElement AreNotAssignableTo(IType firstType, params IType[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(firstType, moreTypes));
        public TNextElement AreNotAssignableTo(Type firstType, params Type[] moreTypes) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(firstType, moreTypes));
        public TNextElement AreNotAssignableTo(IObjectProvider<IType> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(types));
        public TNextElement AreNotAssignableTo(IEnumerable<IType> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(types));
        public TNextElement AreNotAssignableTo(IEnumerable<Type> types) => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(types));

        public TNextElement AreNotValueTypes() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotValueTypes());
        public TNextElement AreNotEnums() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotEnums());
        public TNextElement AreNotStructs() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotStructs());

        public TNextElement DoNotImplementInterface(Interface intf) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotImplementInterface(intf));
        public TNextElement DoNotImplementInterface(Type intf) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotImplementInterface(intf));

        public TNextElement DoNotImplementAnyInterfaces() => DoNotImplementAnyInterfaces(new ObjectProvider<Interface>());
        public TNextElement DoNotImplementAnyInterfaces(params Interface[] interfaces) => DoNotImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement DoNotImplementAnyInterfaces(params Type[] interfaces) => DoNotImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement DoNotImplementAnyInterfaces(IEnumerable<Interface> interfaces) => DoNotImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement DoNotImplementAnyInterfaces(IEnumerable<Type> interfaces) => DoNotImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement DoNotImplementAnyInterfaces(IObjectProvider<Interface> interfaces) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotImplementAny(interfaces));

        public TNextElement DoNotResideInNamespace(string fullName) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotResideInNamespace(fullName));
        public TNextElement DoNotResideInNamespaceMatching(string pattern) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotResideInNamespaceMatching(pattern));

        public TNextElement DoNotResideInAssembly(string fullName) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotResideInAssembly(fullName));
        public TNextElement DoNotResideInAssemblyMatching(string pattern) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotResideInAssemblyMatching(pattern));
        public TNextElement DoNotResideInAssembly(System.Reflection.Assembly assembly, params System.Reflection.Assembly[] moreAssemblies) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotResideInAssembly(assembly, moreAssemblies));
        public TNextElement DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotResideInAssembly(assembly, moreAssemblies));

        public TNextElement DoNotHavePropertyMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotHavePropertyMemberWithName(name));
        public TNextElement DoNotHaveFieldMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotHaveFieldMemberWithName(name));
        public TNextElement DoNotHaveMethodMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotHaveMethodMemberWithName(name));
        public TNextElement DoNotHaveMemberWithName(string name) => CreateNextElement(TypePredicatesDefinition<TRuleType>.DoNotHaveMemberWithName(name));

        public TNextElement AreNotNested() => CreateNextElement(TypePredicatesDefinition<TRuleType>.AreNotNested());
        // csharpier-ignore-end
    }
}
