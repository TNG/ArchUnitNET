using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public interface IMethodMemberConditions<out TReturnType, out TRuleType>
        : IMemberConditions<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType BeConstructor();
        TReturnType BeVirtual();
        TReturnType BeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType BeCalledBy(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType BeCalledBy(IType firstType, params IType[] moreTypes);
        TReturnType BeCalledBy(Type type, params Type[] moreTypes);
        TReturnType BeCalledBy(IObjectProvider<IType> types);
        TReturnType BeCalledBy(IEnumerable<IType> types);
        TReturnType BeCalledBy(IEnumerable<Type> types);
        TReturnType HaveDependencyInMethodBodyTo(
            string pattern,
            bool useRegularExpressions = false
        );
        TReturnType HaveDependencyInMethodBodyTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TReturnType HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TReturnType HaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TReturnType HaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TReturnType HaveDependencyInMethodBodyTo(IEnumerable<Type> types);
        TReturnType HaveReturnType(string pattern, bool useRegularExpressions = false);
        TReturnType HaveReturnType(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType HaveReturnType(IType firstType, params IType[] moreTypes);
        TReturnType HaveReturnType(IEnumerable<IType> types);
        TReturnType HaveReturnType(IObjectProvider<IType> types);
        TReturnType HaveReturnType(Type type, params Type[] moreTypes);
        TReturnType HaveReturnType(IEnumerable<Type> types);

        //Negations


        TReturnType BeNoConstructor();
        TReturnType NotBeVirtual();
        TReturnType NotBeCalledBy(string pattern, bool useRegularExpressions = false);
        TReturnType NotBeCalledBy(IEnumerable<string> patterns, bool useRegularExpressions = false);
        TReturnType NotBeCalledBy(IType firstType, params IType[] moreTypes);
        TReturnType NotBeCalledBy(Type type, params Type[] moreTypes);
        TReturnType NotBeCalledBy(IObjectProvider<IType> types);
        TReturnType NotBeCalledBy(IEnumerable<IType> types);
        TReturnType NotBeCalledBy(IEnumerable<Type> types);
        TReturnType NotHaveDependencyInMethodBodyTo(
            string pattern,
            bool useRegularExpressions = false
        );
        TReturnType NotHaveDependencyInMethodBodyTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType NotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes);
        TReturnType NotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes);
        TReturnType NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types);
        TReturnType NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types);
        TReturnType NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types);
        TReturnType NotHaveReturnType(string pattern, bool useRegularExpressions = false);
        TReturnType NotHaveReturnType(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType NotHaveReturnType(IType firstType, params IType[] moreTypes);
        TReturnType NotHaveReturnType(IEnumerable<IType> types);
        TReturnType NotHaveReturnType(IObjectProvider<IType> types);
        TReturnType NotHaveReturnType(Type type, params Type[] moreTypes);
        TReturnType NotHaveReturnType(IEnumerable<Type> types);
    }
}
