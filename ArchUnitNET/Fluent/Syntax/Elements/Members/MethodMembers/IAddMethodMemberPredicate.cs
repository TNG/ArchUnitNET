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

        TNextElement AreCalledBy(IType firstType, params IType[] moreTypes);
        TNextElement AreCalledBy(Type type, params Type[] moreTypes);
        TNextElement AreCalledBy(IObjectProvider<IType> types);
        TNextElement AreCalledBy(IEnumerable<IType> types);
        TNextElement AreCalledBy(IEnumerable<Type> types);

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

        TNextElement AreNoConstructors();
        TNextElement AreNotVirtual();

        TNextElement AreNotCalledBy(IType firstType, params IType[] moreTypes);
        TNextElement AreNotCalledBy(Type type, params Type[] moreTypes);
        TNextElement AreNotCalledBy(IObjectProvider<IType> types);
        TNextElement AreNotCalledBy(IEnumerable<IType> types);
        TNextElement AreNotCalledBy(IEnumerable<Type> types);

        TNextElement DoNotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TNextElement DoNotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TNextElement DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TNextElement DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);

        TNextElement DoNotHaveReturnType(IType firstType, params IType[] moreTypes);
        TNextElement DoNotHaveReturnType(IEnumerable<IType> types);
        TNextElement DoNotHaveReturnType(IObjectProvider<IType> types);
        TNextElement DoNotHaveReturnType(Type type, params Type[] moreTypes);
        TNextElement DoNotHaveReturnType(IEnumerable<Type> types);
    }
}
