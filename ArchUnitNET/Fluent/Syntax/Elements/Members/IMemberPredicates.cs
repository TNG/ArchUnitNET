using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMemberPredicates<out TRuleTypeConjunction, TRuleType>
        : IObjectPredicates<TRuleTypeConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction AreStatic();
        TRuleTypeConjunction AreImmutable();

        //Negations

        TRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction AreNotStatic();
        TRuleTypeConjunction AreNotImmutable();
    }
}
