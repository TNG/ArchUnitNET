using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface IAddTypeCondition<TNextElement, TRuleType>
        : IAddObjectCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement Be(Type firstType, params Type[] moreTypes);
        TNextElement Be(IEnumerable<Type> types);

        TNextElement BeAssignableTo(IType firstType, params IType[] moreTypes);
        TNextElement BeAssignableTo(Type type, params Type[] moreTypes);
        TNextElement BeAssignableTo(IObjectProvider<IType> types);
        TNextElement BeAssignableTo(IEnumerable<IType> types);
        TNextElement BeAssignableTo(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> BeAssignableToTypesThat();

        TNextElement BeValueTypes();
        TNextElement BeEnums();
        TNextElement BeStructs();

        TNextElement ImplementInterface(Interface intf);
        TNextElement ImplementInterface(Type intf);

        TNextElement ImplementAnyInterfaces();
        TNextElement ImplementAnyInterfaces(params Interface[] interfaces);
        TNextElement ImplementAnyInterfaces(params Type[] interfaces);
        TNextElement ImplementAnyInterfaces(IEnumerable<Interface> interfaces);
        TNextElement ImplementAnyInterfaces(IEnumerable<Type> interfaces);
        TNextElement ImplementAnyInterfaces(IObjectProvider<Interface> interfaces);
        ShouldRelateToInterfacesThat<TNextElement, TRuleType> ImplementAnyInterfacesThat();

        TNextElement ResideInNamespace(string fullName);
        TNextElement ResideInNamespaceMatching(string pattern);

        TNextElement ResideInAssembly(string fullName);
        TNextElement ResideInAssemblyMatching(string pattern);
        TNextElement ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TNextElement ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );

        TNextElement HavePropertyMemberWithName(string name);
        TNextElement HaveFieldMemberWithName(string name);
        TNextElement HaveMethodMemberWithName(string name);
        TNextElement HaveMemberWithName(string name);

        TNextElement BeNested();

        //Negations

        TNextElement NotBe(Type firstType, params Type[] moreTypes);
        TNextElement NotBe(IEnumerable<Type> types);

        TNextElement NotBeAssignableTo(IType type, params IType[] moreTypes);
        TNextElement NotBeAssignableTo(Type type, params Type[] moreTypes);
        TNextElement NotBeAssignableTo(IObjectProvider<IType> types);
        TNextElement NotBeAssignableTo(IEnumerable<IType> types);
        TNextElement NotBeAssignableTo(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> NotBeAssignableToTypesThat();

        TNextElement NotBeValueTypes();
        TNextElement NotBeEnums();
        TNextElement NotBeStructs();

        TNextElement NotImplementInterface(Interface intf);
        TNextElement NotImplementInterface(Type intf);

        TNextElement NotImplementAnyInterfaces();
        TNextElement NotImplementAnyInterfaces(params Interface[] interfaces);
        TNextElement NotImplementAnyInterfaces(params Type[] interfaces);
        TNextElement NotImplementAnyInterfaces(IEnumerable<Interface> interfaces);
        TNextElement NotImplementAnyInterfaces(IEnumerable<Type> interfaces);
        TNextElement NotImplementAnyInterfaces(IObjectProvider<Interface> interfaces);
        ShouldRelateToInterfacesThat<TNextElement, TRuleType> NotImplementAnyInterfacesThat();

        TNextElement NotResideInNamespace(string fullName);
        TNextElement NotResideInNamespaceMatching(string pattern);

        TNextElement NotResideInAssemblyMatching(string pattern);
        TNextElement NotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TNextElement NotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );

        TNextElement NotHavePropertyMemberWithName(string name);
        TNextElement NotHaveFieldMemberWithName(string name);
        TNextElement NotHaveMethodMemberWithName(string name);
        TNextElement NotHaveMemberWithName(string name);

        TNextElement NotBeNested();
    }
}
