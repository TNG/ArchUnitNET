using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypePredicates<out TReturnType> : IObjectPredicates<TReturnType>
    {
        TReturnType Are(Type firstType, params Type[] moreTypes);
        TReturnType Are(IEnumerable<Type> types);
        TReturnType AreAssignableTo(string pattern, bool useRegularExpressions = false);
        TReturnType AreAssignableTo(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType AreAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType AreAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreAssignableTo(IObjectProvider<IType> types);
        TReturnType AreAssignableTo(IEnumerable<IType> types);
        TReturnType AreAssignableTo(IEnumerable<Type> types);
        TReturnType ImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType ResideInNamespace(string pattern, bool useRegularExpressions = false);
        TReturnType ResideInAssembly(string pattern, bool useRegularExpressions = false);
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType AreNested();


        //Negations


        TReturnType AreNot(Type firstType, params Type[] moreTypes);
        TReturnType AreNot(IEnumerable<Type> types);
        TReturnType AreNotAssignableTo(string pattern, bool useRegularExpressions = false);
        TReturnType AreNotAssignableTo(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType AreNotAssignableTo(IType type, params IType[] moreTypes);
        TReturnType AreNotAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreNotAssignableTo(IObjectProvider<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<Type> types);
        TReturnType DoNotImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotResideInNamespace(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotResideInAssembly(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotHavePropertyMemberWithName(string name);
        TReturnType DoNotHaveFieldMemberWithName(string name);
        TReturnType DoNotHaveMethodMemberWithName(string name);
        TReturnType DoNotHaveMemberWithName(string name);
        TReturnType AreNotNested();
    }
}