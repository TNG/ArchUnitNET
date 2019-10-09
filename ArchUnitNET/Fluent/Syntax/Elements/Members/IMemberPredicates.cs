using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMemberPredicates<out TRuleTypeConjunction> : IObjectPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction AreDeclaredInTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction AreDeclaredInTypesWithFullNameContaining(string pattern);
        TRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction HaveBodyTypeMemberDependencies();
        TRuleTypeConjunction HaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction HaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern);
        TRuleTypeConjunction HaveMethodCallDependencies();
        TRuleTypeConjunction HaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction HaveMethodCallDependenciesWithFullNameContaining(string pattern);
        TRuleTypeConjunction HaveFieldTypeDependencies();
        TRuleTypeConjunction HaveFieldTypeDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction HaveFieldTypeDependenciesWithFullNameContaining(string pattern);


        //Negations


        TRuleTypeConjunction AreNotDeclaredInTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction AreNotDeclaredInTypesWithFullNameContaining(string pattern);
        TRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependencies();
        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveBodyTypeMemberDependenciesWithFullNameContaining(string pattern);
        TRuleTypeConjunction DoNotHaveMethodCallDependencies();
        TRuleTypeConjunction DoNotHaveMethodCallDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveMethodCallDependenciesWithFullNameContaining(string pattern);
        TRuleTypeConjunction DoNotHaveFieldTypeDependencies();
        TRuleTypeConjunction DoNotHaveFieldTypeDependenciesWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHaveFieldTypeDependenciesWithFullNameContaining(string pattern);
    }
}