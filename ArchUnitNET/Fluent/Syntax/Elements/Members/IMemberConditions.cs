using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMemberConditions<out TReturnType> : IObjectConditions<TReturnType>
    {
        TReturnType BeDeclaredInTypesWithFullNameMatching(string pattern);
        TReturnType BeDeclaredInTypesWithFullNameContaining(string pattern);
        TReturnType BeDeclaredIn(IType firstType, params IType[] moreTypes);
        TReturnType BeDeclaredIn(Type firstType, params Type[] moreTypes);
        TReturnType BeDeclaredIn(IObjectProvider<IType> types);
        TReturnType BeDeclaredIn(IEnumerable<IType> types);
        TReturnType BeDeclaredIn(IEnumerable<Type> types);
        TReturnType HaveBodyTypeMemberDependencies();
        TReturnType HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TReturnType HaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern);
        TReturnType HaveMethodCallDependencies();
        TReturnType HaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TReturnType HaveMethodCallDependenciesWithFullNameContaining(string pattern);
        TReturnType HaveFieldTypeDependencies();
        TReturnType HaveFieldTypeDependenciesWithFullNameMatching(string pattern);
        TReturnType HaveFieldTypeDependenciesWithFullNameContaining(string pattern);


        //Negations

        TReturnType NotBeDeclaredInTypesWithFullNameMatching(string pattern);
        TReturnType NotBeDeclaredInTypesWithFullNameContaining(string pattern);
        TReturnType NotBeDeclaredIn(IType firstType, params IType[] moreTypes);
        TReturnType NotBeDeclaredIn(Type firstType, params Type[] moreTypes);
        TReturnType NotBeDeclaredIn(IObjectProvider<IType> types);
        TReturnType NotBeDeclaredIn(IEnumerable<IType> types);
        TReturnType NotBeDeclaredIn(IEnumerable<Type> types);
        TReturnType NotHaveBodyTypeMemberDependencies();
        TReturnType NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TReturnType NotHaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern);
        TReturnType NotHaveMethodCallDependencies();
        TReturnType NotHaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TReturnType NotHaveMethodCallDependenciesWithFullNameContaining(string pattern);
        TReturnType NotHaveFieldTypeDependencies();
        TReturnType NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern);
        TReturnType NotHaveFieldTypeDependenciesWithFullNameContaining(string pattern);
    }
}