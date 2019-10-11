using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypeConditions<out TReturnType> : IObjectConditions<TReturnType>
    {
        TReturnType Be(Type firstType, params Type[] moreTypes);
        TReturnType Be(IEnumerable<Type> types);
        TReturnType BeAssignableTo(string pattern, bool useRegularExpressions = false);
        TReturnType BeAssignableTo(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType BeAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType BeAssignableTo(Type type, params Type[] moreTypes);
        TReturnType BeAssignableTo(IObjectProvider<IType> types);
        TReturnType BeAssignableTo(IEnumerable<IType> types);
        TReturnType BeAssignableTo(IEnumerable<Type> types);
        TReturnType ImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType ResideInNamespace(string pattern, bool useRegularExpressions = false);
        TReturnType ResideInAssembly(string pattern, bool useRegularExpressions = false);
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType BeNested();


        //Negations


        TReturnType NotBe(Type firstType, params Type[] moreTypes);
        TReturnType NotBe(IEnumerable<Type> types);
        TReturnType NotBeAssignableTo(string pattern, bool useRegularExpressions = false);
        TReturnType NotBeAssignableTo(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBeAssignableTo(IType type, params IType[] moreTypes);
        TReturnType NotBeAssignableTo(Type type, params Type[] moreTypes);
        TReturnType NotBeAssignableTo(IObjectProvider<IType> types);
        TReturnType NotBeAssignableTo(IEnumerable<IType> types);
        TReturnType NotBeAssignableTo(IEnumerable<Type> types);
        TReturnType NotImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType NotResideInNamespace(string pattern, bool useRegularExpressions = false);
        TReturnType NotResideInAssembly(string pattern, bool useRegularExpressions = false);
        TReturnType NotHavePropertyMemberWithName(string name);
        TReturnType NotHaveFieldMemberWithName(string name);
        TReturnType NotHaveMethodMemberWithName(string name);
        TReturnType NotHaveMemberWithName(string name);
        TReturnType NotBeNested();
    }
}