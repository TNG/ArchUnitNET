using System;
using System.Collections.Generic;
using System.IO;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public abstract class AddTypeCondition<TRuleType, TNextElement>
        : AddObjectCondition<TRuleType, TNextElement>,
            ITypeConditions<TNextElement, TRuleType>
        where TRuleType : IType
    {
        internal AddTypeCondition(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        // csharpier-ignore-start
        public TNextElement Be(Type firstType, params Type[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.Be(firstType, moreTypes));
        public TNextElement Be(IEnumerable<Type> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.Be(types));

        public TNextElement BeAssignableTo(IType firstType, params IType[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeAssignableTo(firstType, moreTypes));
        public TNextElement BeAssignableTo(Type firstType, params Type[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeAssignableTo(firstType, moreTypes));
        public TNextElement BeAssignableTo(IObjectProvider<IType> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeAssignableTo(types));
        public TNextElement BeAssignableTo(IEnumerable<IType> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeAssignableTo(types));
        public TNextElement BeAssignableTo(IEnumerable<Type> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeAssignableTo(types));

        public TNextElement BeNestedIn(IType firstType, params IType[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeNestedIn(firstType, moreTypes));
        public TNextElement BeNestedIn(Type firstType, params Type[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeNestedIn(firstType, moreTypes));
        public TNextElement BeNestedIn(IObjectProvider<IType> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeNestedIn(types));
        public TNextElement BeNestedIn(IEnumerable<IType> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeNestedIn(types));
        public TNextElement BeNestedIn(IEnumerable<Type> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeNestedIn(types));

        public TNextElement BeValueTypes() => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeValueTypes());
        public TNextElement BeEnums() => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeEnums());
        public TNextElement BeStructs() => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeStructs());

        public TNextElement ImplementInterface(Interface intf) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ImplementInterface(intf));
        public TNextElement ImplementInterface(Type intf) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ImplementInterface(intf));

        public TNextElement ImplementAnyInterfaces() => ImplementAnyInterfaces(new ObjectProvider<Interface>());
        public TNextElement ImplementAnyInterfaces(params Interface[] interfaces) => ImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(params Type[] interfaces) => ImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(IEnumerable<Interface> interfaces) => ImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(IEnumerable<Type> interfaces) => ImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement ImplementAnyInterfaces(IObjectProvider<Interface> interfaces) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ImplementAny(interfaces));

        public TNextElement ResideInNamespace(string fullName) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ResideInNamespace(fullName));
        public TNextElement ResideInNamespaceMatching(string pattern) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ResideInNamespaceMatching(pattern));

        public TNextElement ResideInAssembly(string fullName) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ResideInAssembly(fullName));
        public TNextElement ResideInAssemblyMatching(string pattern) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ResideInAssemblyMatching(pattern));
        public TNextElement ResideInAssembly(System.Reflection.Assembly assembly, params System.Reflection.Assembly[] moreAssemblies) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies));
        public TNextElement ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies) => CreateNextElement(TypeConditionsDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies));

        public TNextElement HavePropertyMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.HavePropertyMemberWithName(name));
        public TNextElement HaveFieldMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.HaveFieldMemberWithName(name));
        public TNextElement HaveMethodMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.HaveMethodMemberWithName(name));
        public TNextElement HaveMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.HaveMemberWithName(name));

        public TNextElement BeNested() => CreateNextElement(TypeConditionsDefinition<TRuleType>.BeNested());

        //Relation Conditions

        public ShouldRelateToTypesThat<TNextElement, TRuleType> BeTypesThat() => BeginComplexTypeCondition(TypeConditionsDefinition<TRuleType>.BeTypesThat());
        public ShouldRelateToTypesThat<TNextElement, TRuleType> BeAssignableToTypesThat() => BeginComplexTypeCondition(TypeConditionsDefinition<TRuleType>.BeAssignableToTypesThat());
        public ShouldRelateToInterfacesThat<TNextElement, TRuleType> ImplementAnyInterfacesThat() => BeginComplexInterfaceCondition(TypeConditionsDefinition<TRuleType>.ImplementAnyInterfacesThat());

        //Negations

        public TNextElement NotBe(Type firstType, params Type[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBe(firstType, moreTypes));
        public TNextElement NotBe(IEnumerable<Type> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBe(types));

        public TNextElement NotBeAssignableTo(IType firstType, params IType[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(firstType, moreTypes));
        public TNextElement NotBeAssignableTo(Type firstType, params Type[] moreTypes) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(firstType, moreTypes));
        public TNextElement NotBeAssignableTo(IObjectProvider<IType> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(types));
        public TNextElement NotBeAssignableTo(IEnumerable<IType> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(types));
        public TNextElement NotBeAssignableTo(IEnumerable<Type> types) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeAssignableTo(types));

        public TNextElement NotBeValueTypes() => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeValueTypes());
        public TNextElement NotBeEnums() => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeEnums());
        public TNextElement NotBeStructs() => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeStructs());

        public TNextElement NotImplementInterface(Interface intf) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotImplementInterface(intf));
        public TNextElement NotImplementInterface(Type intf) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotImplementInterface(intf));

        public TNextElement NotImplementAnyInterfaces() => NotImplementAnyInterfaces(new ObjectProvider<Interface>());
        public TNextElement NotImplementAnyInterfaces(params Interface[] interfaces) => NotImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement NotImplementAnyInterfaces(params Type[] interfaces) => NotImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement NotImplementAnyInterfaces(IEnumerable<Interface> interfaces) => NotImplementAnyInterfaces(new ObjectProvider<Interface>(interfaces));
        public TNextElement NotImplementAnyInterfaces(IEnumerable<Type> interfaces) => NotImplementAnyInterfaces(new SystemTypeObjectProvider<Interface>(interfaces));
        public TNextElement NotImplementAnyInterfaces(IObjectProvider<Interface> interfaces) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotImplementAny(interfaces));

        public TNextElement NotResideInNamespace(string fullName) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotResideInNamespace(fullName));
        public TNextElement NotResideInNamespaceMatching(string pattern) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotResideInNamespaceMatching(pattern));

        public TNextElement NotResideInAssembly(string fullName) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotResideInAssembly(fullName));
        public TNextElement NotResideInAssemblyMatching(string pattern) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotResideInAssemblyMatching(pattern));
        public TNextElement NotResideInAssembly(System.Reflection.Assembly assembly, params System.Reflection.Assembly[] moreAssemblies) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotResideInAssembly(assembly, moreAssemblies));
        public TNextElement NotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotResideInAssembly(assembly, moreAssemblies));

        public TNextElement NotHavePropertyMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotHavePropertyMemberWithName(name));
        public TNextElement NotHaveFieldMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotHaveFieldMemberWithName(name));
        public TNextElement NotHaveMethodMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotHaveMethodMemberWithName(name));
        public TNextElement NotHaveMemberWithName(string name) => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotHaveMemberWithName(name));

        public TNextElement NotBeNested() => CreateNextElement(TypeConditionsDefinition<TRuleType>.NotBeNested());

        public TNextElement AdhereToPlantUmlDiagram(string file) => CreateNextElement(TypeConditionsDefinition<TRuleType>.AdhereToPlantUmlDiagram(file));
        public TNextElement AdhereToPlantUmlDiagram(Stream stream) => CreateNextElement(TypeConditionsDefinition<TRuleType>.AdhereToPlantUmlDiagram(stream));

        //Relation Condition Negations

        public ShouldRelateToTypesThat<TNextElement, TRuleType> NotBeAssignableToTypesThat() => BeginComplexTypeCondition(TypeConditionsDefinition<TRuleType>.NotBeAssignableToTypesThat());
        public ShouldRelateToInterfacesThat<TNextElement, TRuleType> NotImplementAnyInterfacesThat() => BeginComplexInterfaceCondition(TypeConditionsDefinition<TRuleType>.NotImplementAnyInterfacesThat());
        // csharpier-ignore-end
    }
}
