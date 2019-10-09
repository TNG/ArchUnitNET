using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface
        IMemberConditions<TRuleTypeShouldConjunction, TRuleType> : IObjectConditions<TRuleTypeShouldConjunction,
            TRuleType>
        where TRuleType : IMember
        where TRuleTypeShouldConjunction : SyntaxElement<TRuleType>
    {
        TRuleTypeShouldConjunction BeDeclaredInTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction BeDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction BeDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction BeDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<IType> types);
        TRuleTypeShouldConjunction BeDeclaredIn(IEnumerable<Type> types);
        TRuleTypeShouldConjunction HaveBodyTypeMemberDependencies();
        TRuleTypeShouldConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction HaveMethodCallDependencies();
        TRuleTypeShouldConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction HaveFieldTypeDependencies();
        TRuleTypeShouldConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern);


        //Negations

        TRuleTypeShouldConjunction NotBeDeclaredInTypesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotBeDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeShouldConjunction NotBeDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeShouldConjunction NotBeDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<IType> types);
        TRuleTypeShouldConjunction NotBeDeclaredIn(IEnumerable<Type> types);
        TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependencies();
        TRuleTypeShouldConjunction NotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHaveMethodCallDependencies();
        TRuleTypeShouldConjunction NotHaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeShouldConjunction NotHaveFieldTypeDependencies();
        TRuleTypeShouldConjunction NotHaveFieldTypeDependenciesWithFullNameMatching(string pattern);
    }
}