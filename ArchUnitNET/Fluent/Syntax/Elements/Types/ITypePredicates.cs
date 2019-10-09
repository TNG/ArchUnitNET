using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypePredicates<TRuleTypeConjunction> : IObjectPredicates<TRuleTypeConjunction>
    {
        TRuleTypeConjunction Are(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction Are(IEnumerable<Type> types);
        TRuleTypeConjunction AreAssignableToTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction AreAssignableTo(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreAssignableTo(Type type, params Type[] moreTypes);
        TRuleTypeConjunction AreAssignableTo(ObjectProvider<IType> types);
        TRuleTypeConjunction AreAssignableTo(IEnumerable<IType> types);
        TRuleTypeConjunction AreAssignableTo(IEnumerable<Type> types);
        TRuleTypeConjunction ImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction ResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction HavePropertyMemberWithName(string name);
        TRuleTypeConjunction HaveFieldMemberWithName(string name);
        TRuleTypeConjunction HaveMethodMemberWithName(string name);
        TRuleTypeConjunction HaveMemberWithName(string name);
        TRuleTypeConjunction AreNested();


        //Negations


        TRuleTypeConjunction AreNot(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreNot(IEnumerable<Type> types);
        TRuleTypeConjunction AreNotAssignableToTypesWithFullNameMatching(string pattern);
        TRuleTypeConjunction AreNotAssignableTo(IType type, params IType[] moreTypes);
        TRuleTypeConjunction AreNotAssignableTo(Type type, params Type[] moreTypes);
        TRuleTypeConjunction AreNotAssignableTo(ObjectProvider<IType> types);
        TRuleTypeConjunction AreNotAssignableTo(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotAssignableTo(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHavePropertyMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveFieldMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveMethodMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveMemberWithName(string name);
        TRuleTypeConjunction AreNotNested();
    }
}