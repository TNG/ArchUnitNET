using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public interface IAddMemberCondition<TNextElement, TRuleType>
        : IAddObjectCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement BeDeclaredIn();
        TNextElement BeDeclaredIn(params IType[] types);
        TNextElement BeDeclaredIn(params Type[] types);
        TNextElement BeDeclaredIn(IObjectProvider<IType> types);
        TNextElement BeDeclaredIn(IEnumerable<IType> types);
        TNextElement BeDeclaredIn(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> BeDeclaredInTypesThat();

        TNextElement BeStatic();
        TNextElement BeImmutable();

        //Negations
        TNextElement NotBeDeclaredIn();
        TNextElement NotBeDeclaredIn(params IType[] types);
        TNextElement NotBeDeclaredIn(params Type[] types);
        TNextElement NotBeDeclaredIn(IObjectProvider<IType> types);
        TNextElement NotBeDeclaredIn(IEnumerable<IType> types);
        TNextElement NotBeDeclaredIn(IEnumerable<Type> types);
        ShouldRelateToTypesThat<TNextElement, TRuleType> NotBeDeclaredInTypesThat();

        TNextElement NotBeStatic();
        TNextElement NotBeImmutable();
    }
}
