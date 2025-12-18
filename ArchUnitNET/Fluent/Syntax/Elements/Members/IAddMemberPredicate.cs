using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IAddMemberPredicate<out TNextElement, TRuleType>
        : IAddObjectPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement AreDeclaredIn(IType firstType, params IType[] moreTypes);
        TNextElement AreDeclaredIn(Type firstType, params Type[] moreTypes);
        TNextElement AreDeclaredIn(IObjectProvider<IType> types);
        TNextElement AreDeclaredIn(IEnumerable<IType> types);
        TNextElement AreDeclaredIn(IEnumerable<Type> types);
        TNextElement AreStatic();
        TNextElement AreImmutable();

        //Negations

        TNextElement AreNotDeclaredIn(IType firstType, params IType[] moreTypes);
        TNextElement AreNotDeclaredIn(Type firstType, params Type[] moreTypes);
        TNextElement AreNotDeclaredIn(IObjectProvider<IType> types);
        TNextElement AreNotDeclaredIn(IEnumerable<IType> types);
        TNextElement AreNotDeclaredIn(IEnumerable<Type> types);
        TNextElement AreNotStatic();
        TNextElement AreNotImmutable();
    }
}
