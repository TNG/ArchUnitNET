using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IMemberPredicates<out TRuleTypeConjunction, TRuleType>
        : IObjectPredicates<TRuleTypeConjunction, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TRuleTypeConjunction AreDeclaredIn(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TRuleTypeConjunction AreDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction AreDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction AreStatic();
        TRuleTypeConjunction AreImmutable();

        //Negations


        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TRuleTypeConjunction AreNotDeclaredIn(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotDeclaredIn(Types().That().HaveFullName()) instead"
        )]
        TRuleTypeConjunction AreNotDeclaredIn(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TRuleTypeConjunction AreNotDeclaredIn(IType firstType, params IType[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(Type firstType, params Type[] moreTypes);
        TRuleTypeConjunction AreNotDeclaredIn(IObjectProvider<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<IType> types);
        TRuleTypeConjunction AreNotDeclaredIn(IEnumerable<Type> types);
        TRuleTypeConjunction AreNotStatic();
        TRuleTypeConjunction AreNotImmutable();
    }
}
