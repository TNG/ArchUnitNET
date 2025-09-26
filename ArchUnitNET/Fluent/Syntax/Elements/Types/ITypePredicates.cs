using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypePredicates<out TReturnType, TRuleType>
        : IObjectPredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType Are(Type firstType, params Type[] moreTypes);
        TReturnType Are(IEnumerable<Type> types);

        TReturnType AreAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType AreAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreAssignableTo(IObjectProvider<IType> types);
        TReturnType AreAssignableTo(IEnumerable<IType> types);
        TReturnType AreAssignableTo(IEnumerable<Type> types);
        TReturnType AreValueTypes();
        TReturnType AreEnums();
        TReturnType AreStructs();

        TReturnType ImplementInterface(Interface intf);
        TReturnType ImplementInterface(Type intf);

        TReturnType ImplementAnyInterfaces();
        TReturnType ImplementAnyInterfaces(params Interface[] interfaces);
        TReturnType ImplementAnyInterfaces(params Type[] interfaces);
        TReturnType ImplementAnyInterfaces(IEnumerable<Interface> interfaces);
        TReturnType ImplementAnyInterfaces(IEnumerable<Type> interfaces);
        TReturnType ImplementAnyInterfaces(IObjectProvider<Interface> interfaces);

        TReturnType ResideInNamespace(string fullName);
        TReturnType ResideInNamespaceMatching(string pattern);

        TReturnType ResideInAssembly(string fullName);
        TReturnType ResideInAssemblyMatching(string pattern);
        TReturnType ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType AreNested();

        //Negations

        TReturnType AreNot(Type firstType, params Type[] moreTypes);
        TReturnType AreNot(IEnumerable<Type> types);

        TReturnType AreNotAssignableTo(IType type, params IType[] moreTypes);
        TReturnType AreNotAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreNotAssignableTo(IObjectProvider<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<Type> types);
        TReturnType AreNotValueTypes();
        TReturnType AreNotEnums();
        TReturnType AreNotStructs();

        TReturnType DoNotImplementInterface(Interface intf);
        TReturnType DoNotImplementInterface(Type intf);

        TReturnType DoNotImplementAnyInterfaces();
        TReturnType DoNotImplementAnyInterfaces(params Interface[] interfaces);
        TReturnType DoNotImplementAnyInterfaces(params Type[] interfaces);
        TReturnType DoNotImplementAnyInterfaces(IEnumerable<Interface> interfaces);
        TReturnType DoNotImplementAnyInterfaces(IEnumerable<Type> interfaces);
        TReturnType DoNotImplementAnyInterfaces(IObjectProvider<Interface> interfaces);

        TReturnType DoNotResideInNamespace(string fullName);

        TReturnType DoNotResideInAssembly(string fullName);
        TReturnType DoNotResideInAssemblyMatching(string pattern);
        TReturnType DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType DoNotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType DoNotHavePropertyMemberWithName(string name);
        TReturnType DoNotHaveFieldMemberWithName(string name);
        TReturnType DoNotHaveMethodMemberWithName(string name);
        TReturnType DoNotHaveMemberWithName(string name);
        TReturnType AreNotNested();
    }
}
