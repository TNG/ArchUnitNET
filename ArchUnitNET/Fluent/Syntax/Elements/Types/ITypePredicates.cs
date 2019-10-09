using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypePredicates<out TReturnType> : IObjectPredicates<TReturnType>
    {
        TReturnType Are(Type firstType, params Type[] moreTypes);
        TReturnType Are(IEnumerable<Type> types);
        TReturnType AreAssignableToTypesWithFullNameMatching(string pattern);
        TReturnType AreAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType AreAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreAssignableTo(ObjectProvider<IType> types);
        TReturnType AreAssignableTo(IEnumerable<IType> types);
        TReturnType AreAssignableTo(IEnumerable<Type> types);
        TReturnType ImplementInterfaceWithFullNameMatching(string pattern);
        TReturnType ResideInNamespaceWithFullNameMatching(string pattern);
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType AreNested();


        //Negations


        TReturnType AreNot(Type firstType, params Type[] moreTypes);
        TReturnType AreNot(IEnumerable<Type> types);
        TReturnType AreNotAssignableToTypesWithFullNameMatching(string pattern);
        TReturnType AreNotAssignableTo(IType type, params IType[] moreTypes);
        TReturnType AreNotAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreNotAssignableTo(ObjectProvider<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<Type> types);
        TReturnType DoNotImplementInterfaceWithFullNameMatching(string pattern);
        TReturnType DoNotResideInNamespaceWithFullNameMatching(string pattern);
        TReturnType DoNotHavePropertyMemberWithName(string name);
        TReturnType DoNotHaveFieldMemberWithName(string name);
        TReturnType DoNotHaveMethodMemberWithName(string name);
        TReturnType DoNotHaveMemberWithName(string name);
        TReturnType AreNotNested();
    }
}