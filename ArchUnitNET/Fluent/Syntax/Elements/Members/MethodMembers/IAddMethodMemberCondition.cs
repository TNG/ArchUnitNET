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

        TNextElement BeCalledBy(IType firstType, params IType[] moreTypes);
        TNextElement BeCalledBy(Type type, params Type[] moreTypes);
        TNextElement BeCalledBy(IObjectProvider<IType> types);
        TNextElement BeCalledBy(IEnumerable<IType> types);
        TNextElement BeCalledBy(IEnumerable<Type> types);

        TNextElement HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TNextElement HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TNextElement HaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TNextElement HaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TNextElement HaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TNextElement HaveReturnType(IType firstType, params IType[] moreTypes);
        TNextElement HaveReturnType(IEnumerable<IType> types);
        TNextElement HaveReturnType(IObjectProvider<IType> types);
        TNextElement HaveReturnType(Type type, params Type[] moreTypes);
        TNextElement HaveReturnType(IEnumerable<Type> types);

        //Negations

        TNextElement BeNoConstructor();
        TNextElement NotBeVirtual();

        TNextElement NotBeCalledBy(IType firstType, params IType[] moreTypes);
        TNextElement NotBeCalledBy(Type type, params Type[] moreTypes);
        TNextElement NotBeCalledBy(IObjectProvider<IType> types);
        TNextElement NotBeCalledBy(IEnumerable<IType> types);
        TNextElement NotBeCalledBy(IEnumerable<Type> types);

        TNextElement NotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TNextElement NotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TNextElement NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TNextElement NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TNextElement NotHaveReturnType(IType firstType, params IType[] moreTypes);
        TNextElement NotHaveReturnType(IEnumerable<IType> types);
        TNextElement NotHaveReturnType(IObjectProvider<IType> types);
        TNextElement NotHaveReturnType(Type type, params Type[] moreTypes);
        TNextElement NotHaveReturnType(IEnumerable<Type> types);
    }
}
