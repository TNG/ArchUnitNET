using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IAddMemberPredicate<out TNextElement, TRuleType>
        : IAddObjectPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        // csharpier-ignore-start
        TNextElement AreDeclaredIn();
        TNextElement AreDeclaredIn(params IType[] types);
        TNextElement AreDeclaredIn(params Type[] types);
        TNextElement AreDeclaredIn(IObjectProvider<IType> types);
        TNextElement AreDeclaredIn(IEnumerable<IType> types);
        TNextElement AreDeclaredIn(IEnumerable<Type> types);
        TNextElement AreStatic();
        TNextElement AreImmutable();

        //Negations

        TNextElement AreNotDeclaredIn();
        TNextElement AreNotDeclaredIn(params IType[] types);
        TNextElement AreNotDeclaredIn(params Type[] types);
        TNextElement AreNotDeclaredIn(IObjectProvider<IType> types);
        TNextElement AreNotDeclaredIn(IEnumerable<IType> types);
        TNextElement AreNotDeclaredIn(IEnumerable<Type> types);
        TNextElement AreNotStatic();
        TNextElement AreNotImmutable();
        // csharpier-ignore-end
    }
}
