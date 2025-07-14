using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public interface ITypePredicates<out TReturnType, TRuleType>
        : IObjectPredicates<TReturnType, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        TReturnType Are(Type firstType, params Type[] moreTypes);
        TReturnType Are(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType AreAssignableTo(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType AreAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType AreAssignableTo(IType firstType, params IType[] moreTypes);
        TReturnType AreAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreAssignableTo(IObjectProvider<IType> types);
        TReturnType AreAssignableTo(IEnumerable<IType> types);
        TReturnType AreAssignableTo(IEnumerable<Type> types);
        TReturnType AreValueTypes();
        TReturnType AreEnums();
        TReturnType AreStructs();

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType ImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType ImplementInterface(Interface intf);
        TReturnType ImplementInterface(Type intf);

        [Obsolete(
            "Either ResideInNamespace() without the useRegularExpressions parameter or ResideInNamespaceMatching() should be used"
        )]
        TReturnType ResideInNamespace(string pattern, bool useRegularExpressions);
        TReturnType ResideInNamespace(string fullName);
        TReturnType ResideInNamespaceMatching(string pattern);

        [Obsolete(
            "Either ResideInAssembly() without the useRegularExpressions parameter or ResideInAssemblyMatching() should be used"
        )]
        TReturnType ResideInAssembly(string pattern, bool useRegularExpressions);
        TReturnType ResideInAssembly(string fullName);
        TReturnType ResideInAssemblyMatching(string pattern);
        TReturnType ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType HavePropertyMemberWithName(string name);
        TReturnType HaveFieldMemberWithName(string name);
        TReturnType HaveMethodMemberWithName(string name);
        TReturnType HaveMemberWithName(string name);
        TReturnType AreNested();

        //Negations

        TReturnType AreNot(Type firstType, params Type[] moreTypes);
        TReturnType AreNot(IEnumerable<Type> types);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType AreNotAssignableTo(string pattern, bool useRegularExpressions = false);

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update. You can use AreNotAssignableTo(Types().That().HaveFullName()) instead"
        )]
        TReturnType AreNotAssignableTo(
            IEnumerable<string> patterns,
            bool useRegularExpressions = false
        );
        TReturnType AreNotAssignableTo(IType type, params IType[] moreTypes);
        TReturnType AreNotAssignableTo(Type type, params Type[] moreTypes);
        TReturnType AreNotAssignableTo(IObjectProvider<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<IType> types);
        TReturnType AreNotAssignableTo(IEnumerable<Type> types);
        TReturnType AreNotValueTypes();
        TReturnType AreNotEnums();
        TReturnType AreNotStructs();

        [Obsolete(
            "Another overload of this method should be used. This will be removed in a future update."
        )]
        TReturnType DoNotImplementInterface(string pattern, bool useRegularExpressions = false);
        TReturnType DoNotImplementInterface(Interface intf);
        TReturnType DoNotImplementInterface(Type intf);

        [Obsolete(
            "Either DoNotResideInNamespace() without the useRegularExpressions parameter or DoNotResideInNamespaceMatching() should be used"
        )]
        TReturnType DoNotResideInNamespace(string pattern, bool useRegularExpressions);
        TReturnType DoNotResideInNamespace(string fullName);

        [Obsolete(
            "Either DoNotResideInAssembly() without the useRegularExpressions parameter or DoNotResideInAssemblyMatching() should be used"
        )]
        TReturnType DoNotResideInAssembly(string pattern, bool useRegularExpressions);
        TReturnType DoNotResideInAssembly(string fullName);
        TReturnType DoNotResideInAssemblyMatching(string pattern);
        TReturnType DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies);
        TReturnType DoNotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        );
        TReturnType DoNotHavePropertyMemberWithName(string name);
        TReturnType DoNotHaveFieldMemberWithName(string name);
        TReturnType DoNotHaveMethodMemberWithName(string name);
        TReturnType DoNotHaveMemberWithName(string name);
        TReturnType AreNotNested();
    }
}
