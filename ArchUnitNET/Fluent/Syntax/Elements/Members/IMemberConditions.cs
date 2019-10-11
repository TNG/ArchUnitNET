using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMemberConditions<out TReturnType> : IObjectConditions<TReturnType>
    {
        TReturnType BeDeclaredIn(string pattern, bool useRegularExpressions = false);
        TReturnType BeDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType BeDeclaredIn(IType firstType, params IType[] moreTypes);
        TReturnType BeDeclaredIn(Type firstType, params Type[] moreTypes);
        TReturnType BeDeclaredIn(IObjectProvider<IType> types);
        TReturnType BeDeclaredIn(IEnumerable<IType> types);
        TReturnType BeDeclaredIn(IEnumerable<Type> types);


        //Negations

        TReturnType NotBeDeclaredIn(string pattern, bool useRegularExpressions = false);
        TReturnType NotBeDeclaredIn(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBeDeclaredIn(IType firstType, params IType[] moreTypes);
        TReturnType NotBeDeclaredIn(Type firstType, params Type[] moreTypes);
        TReturnType NotBeDeclaredIn(IObjectProvider<IType> types);
        TReturnType NotBeDeclaredIn(IEnumerable<IType> types);
        TReturnType NotBeDeclaredIn(IEnumerable<Type> types);
    }
}