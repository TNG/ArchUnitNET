using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypeConditions<out TReturnType> : IObjectConditions<TReturnType>
    {
        TReturnType Be(Type firstType, params Type[] moreTypes);
        TReturnType Be(IEnumerable<Type> types);
        TReturnType BeAssignableToTypesWithFullNameMatching(string pattern);
        TReturnType BeAssignableToTypesWithFullNameContaining(string pattern);
        TReturnType BeAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType BeAssignableTo(Type type, params Type[] moreTypes);
        TReturnType BeAssignableTo(ObjectProvider<IType> types);
        TReturnType BeAssignableTo(IEnumerable<IType> types);
        TReturnType BeAssignableTo(IEnumerable<Type> types);
        TReturnType ImplementInterfaceWithFullNameMatching(string pattern);
        TReturnType ImplementInterfaceWithFullNameContaining(string pattern);
        TReturnType ResideInNamespaceWithFullNameMatching(string pattern);
        TReturnType ResideInNamespaceWithFullNameContaining(string pattern);
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType BeNested();


        //Negations


        TReturnType NotBe(Type firstType, params Type[] moreTypes);
        TReturnType NotBe(IEnumerable<Type> types);
        TReturnType NotBeAssignableToTypesWithFullNameMatching(string pattern);
        TReturnType NotBeAssignableToTypesWithFullNameContaining(string pattern);
        TReturnType NotBeAssignableTo(IType type, params IType[] moreTypes);
        TReturnType NotBeAssignableTo(Type type, params Type[] moreTypes);
        TReturnType NotBeAssignableTo(ObjectProvider<IType> types);
        TReturnType NotBeAssignableTo(IEnumerable<IType> types);
        TReturnType NotBeAssignableTo(IEnumerable<Type> types);
        TReturnType NotImplementInterfaceWithFullNameMatching(string pattern);
        TReturnType NotImplementInterfaceWithFullNameContaining(string pattern);
        TReturnType NotResideInNamespaceWithFullNameMatching(string pattern);
        TReturnType NotResideInNamespaceWithFullNameContaining(string pattern);
        TReturnType NotHavePropertyMemberWithName(string name);
        TReturnType NotHaveFieldMemberWithName(string name);
        TReturnType NotHaveMethodMemberWithName(string name);
        TReturnType NotHaveMemberWithName(string name);
        TReturnType NotBeNested();
    }
}