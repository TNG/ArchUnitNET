using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IAddMethodMemberPredicate<out TNextElement, TRuleType>
        : IAddMemberPredicate<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement AreConstructors();
        TNextElement AreVirtual();

        TNextElement AreCalledBy();
        TNextElement AreCalledBy(params IType[] types);
        TNextElement AreCalledBy(params Type[] types);
        TNextElement AreCalledBy(IObjectProvider<IType> types);
        TNextElement AreCalledBy(IEnumerable<IType> types);
        TNextElement AreCalledBy(IEnumerable<Type> types);

        TNextElement HaveDependencyInMethodBodyTo();
        TNextElement HaveDependencyInMethodBodyTo(params IType[] types);
        TNextElement HaveDependencyInMethodBodyTo(params Type[] types);
        TNextElement HaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TNextElement HaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TNextElement HaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TNextElement HaveReturnType();
        TNextElement HaveReturnType(params IType[] types);
        TNextElement HaveReturnType(params Type[] types);
        TNextElement HaveReturnType(IObjectProvider<IType> types);
        TNextElement HaveReturnType(IEnumerable<IType> types);
        TNextElement HaveReturnType(IEnumerable<Type> types);

        //Negations

        TNextElement AreNoConstructors();
        TNextElement AreNotVirtual();

        TNextElement AreNotCalledBy();
        TNextElement AreNotCalledBy(params IType[] types);
        TNextElement AreNotCalledBy(params Type[] types);
        TNextElement AreNotCalledBy(IObjectProvider<IType> types);
        TNextElement AreNotCalledBy(IEnumerable<IType> types);
        TNextElement AreNotCalledBy(IEnumerable<Type> types);

        TNextElement DoNotHaveDependencyInMethodBodyTo();
        TNextElement DoNotHaveDependencyInMethodBodyTo(params IType[] types);
        TNextElement DoNotHaveDependencyInMethodBodyTo(params Type[] types);
        TNextElement DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TNextElement DoNotHaveReturnType();
        TNextElement DoNotHaveReturnType(params IType[] types);
        TNextElement DoNotHaveReturnType(params Type[] types);
        TNextElement DoNotHaveReturnType(IObjectProvider<IType> types);
        TNextElement DoNotHaveReturnType(IEnumerable<IType> types);
        TNextElement DoNotHaveReturnType(IEnumerable<Type> types);
    }
}
