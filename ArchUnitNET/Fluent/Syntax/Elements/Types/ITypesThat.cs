using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypesThat<TRuleTypeConjunction> : IObjectsThat<TRuleTypeConjunction>
    {
        TRuleTypeConjunction Are(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction Are(IEnumerable<Type> types);
        TRuleTypeConjunction ImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction ImplementInterface(Interface intf);
        TRuleTypeConjunction ResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction HavePropertyMemberWithName(string name);
        TRuleTypeConjunction HaveFieldMemberWithName(string name);
        TRuleTypeConjunction HaveMethodMemberWithName(string name);
        TRuleTypeConjunction HaveMemberWithName(string name);
        TRuleTypeConjunction AreNested();


        //Negations


        TRuleTypeConjunction AreNot(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreNot(IEnumerable<Type> types);
        TRuleTypeConjunction DoNotImplementInterfaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotImplementInterface(Interface intf);
        TRuleTypeConjunction DoNotResideInNamespaceWithFullNameMatching(string pattern);
        TRuleTypeConjunction DoNotHavePropertyMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveFieldMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveMethodMemberWithName(string name);
        TRuleTypeConjunction DoNotHaveMemberWithName(string name);
        TRuleTypeConjunction AreNotNested();
    }
}