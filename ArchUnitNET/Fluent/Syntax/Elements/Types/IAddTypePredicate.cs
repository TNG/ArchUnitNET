using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface IAddTypePredicate<out TNextElement, TRuleType>
        : IAddObjectPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement Are(Type firstType, params Type[] moreTypes);
        TNextElement Are(IEnumerable<Type> types);

        TNextElement AreAssignableTo(IType firstType, params IType[] moreTypes);
        TNextElement AreAssignableTo(Type type, params Type[] moreTypes);
        TNextElement AreAssignableTo(IObjectProvider<IType> types);
        TNextElement AreAssignableTo(IEnumerable<IType> types);
        TNextElement AreAssignableTo(IEnumerable<Type> types);
        TNextElement AreValueTypes();
        TNextElement AreEnums();
        TNextElement AreStructs();

        TNextElement ImplementInterface(Interface intf);
        TNextElement ImplementInterface(Type intf);

        TNextElement ImplementAnyInterfaces();
        TNextElement ImplementAnyInterfaces(params Interface[] interfaces);
        TNextElement ImplementAnyInterfaces(params Type[] interfaces);
        TNextElement ImplementAnyInterfaces(IEnumerable<Interface> interfaces);
        TNextElement ImplementAnyInterfaces(IEnumerable<Type> interfaces);
        TNextElement ImplementAnyInterfaces(IObjectProvider<Interface> interfaces);

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
        TNextElement AreNested();

        //Negations

        TNextElement AreNot(Type firstType, params Type[] moreTypes);
        TNextElement AreNot(IEnumerable<Type> types);

        TNextElement AreNotAssignableTo(IType type, params IType[] moreTypes);
        TNextElement AreNotAssignableTo(Type type, params Type[] moreTypes);
        TNextElement AreNotAssignableTo(IObjectProvider<IType> types);
        TNextElement AreNotAssignableTo(IEnumerable<IType> types);
        TNextElement AreNotAssignableTo(IEnumerable<Type> types);
        TNextElement AreNotValueTypes();
        TNextElement AreNotEnums();
        TNextElement AreNotStructs();

        TNextElement DoNotImplementInterface(Interface intf);
        TNextElement DoNotImplementInterface(Type intf);

        TNextElement DoNotImplementAnyInterfaces();
        TNextElement DoNotImplementAnyInterfaces(params Interface[] interfaces);
        TNextElement DoNotImplementAnyInterfaces(params Type[] interfaces);
        TNextElement DoNotImplementAnyInterfaces(IEnumerable<Interface> interfaces);
        TNextElement DoNotImplementAnyInterfaces(IEnumerable<Type> interfaces);
        TNextElement DoNotImplementAnyInterfaces(IObjectProvider<Interface> interfaces);

        TNextElement DoNotResideInNamespace(string fullName);

        TNextElement DoNotResideInAssembly(string fullName);
        TNextElement DoNotResideInAssemblyMatching(string pattern);
        TNextElement DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TNextElement DoNotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TNextElement DoNotHavePropertyMemberWithName(string name);
        TNextElement DoNotHaveFieldMemberWithName(string name);
        TNextElement DoNotHaveMethodMemberWithName(string name);
        TNextElement DoNotHaveMemberWithName(string name);
        TNextElement AreNotNested();
    }
}
