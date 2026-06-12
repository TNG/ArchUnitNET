using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IAddMethodMemberCondition<TNextElement, TRuleType>
        : IAddMemberCondition<TNextElement, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TNextElement BeConstructor();
        TNextElement BeVirtual();

        TNextElement BeCalledBy();
        TNextElement BeCalledBy(params IType[] types);
        TNextElement BeCalledBy(params Type[] types);
        TNextElement BeCalledBy(IObjectProvider<IType> types);
        TNextElement BeCalledBy(IEnumerable<IType> types);
        TNextElement BeCalledBy(IEnumerable<Type> types);

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

        TNextElement BeNoConstructor();
        TNextElement NotBeVirtual();

        TNextElement NotBeCalledBy();
        TNextElement NotBeCalledBy(params IType[] types);
        TNextElement NotBeCalledBy(params Type[] types);
        TNextElement NotBeCalledBy(IObjectProvider<IType> types);
        TNextElement NotBeCalledBy(IEnumerable<IType> types);
        TNextElement NotBeCalledBy(IEnumerable<Type> types);

        TNextElement NotHaveDependencyInMethodBodyTo();
        TNextElement NotHaveDependencyInMethodBodyTo(params IType[] types);
        TNextElement NotHaveDependencyInMethodBodyTo(params Type[] types);
        TNextElement NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TNextElement NotHaveReturnType();
        TNextElement NotHaveReturnType(params IType[] types);
        TNextElement NotHaveReturnType(params Type[] types);
        TNextElement NotHaveReturnType(IObjectProvider<IType> types);
        TNextElement NotHaveReturnType(IEnumerable<IType> types);
        TNextElement NotHaveReturnType(IEnumerable<Type> types);
    }
}
