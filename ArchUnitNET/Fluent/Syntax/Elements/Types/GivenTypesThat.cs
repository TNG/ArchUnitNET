//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.Syntax.ConjunctionFactory;
using Assembly = System.Reflection.Assembly;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public class GivenTypesThat<TGivenRuleTypeConjunction, TRuleType>
        : GivenObjectsThat<TGivenRuleTypeConjunction, TRuleType>,
            ITypePredicates<TGivenRuleTypeConjunction, TRuleType>
        where TRuleType : IType
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public GivenTypesThat(IArchRuleCreator<TRuleType> ruleCreator)
            : base(ruleCreator) { }

        public TGivenRuleTypeConjunction Are(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.Are(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction Are(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.Are(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreAssignableTo(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreAssignableTo(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreAssignableTo(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNestedIn(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNestedIn(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNestedIn(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNestedIn(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNestedIn(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNestedIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNestedIn(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNestedIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNestedIn(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNestedIn(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreValueTypes()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreValueTypes());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreEnums()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreEnums());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreStructs()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreStructs());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.ImplementInterface(intf));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ImplementInterface(Type intf)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.ImplementInterface(intf));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInNamespace(string fullName)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.ResideInNamespace(fullName)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInNamespaceMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.ResideInNamespace(pattern)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInAssembly(string fullName)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.ResideInAssembly(fullName)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInAssemblyMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.ResideInAssemblyMatching(pattern)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction ResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.ResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.HavePropertyMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.HaveFieldMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.HaveMethodMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.HaveMemberWithName(name));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNested()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNested());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        //Negations


        public TGivenRuleTypeConjunction AreNot(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNot(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNot(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNot(types));
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(
            IType firstType,
            params IType[] moreTypes
        )
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(firstType, moreTypes)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(types)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(types)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.AreNotAssignableTo(types)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotValueTypes()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNotValueTypes());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotEnums()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNotEnums());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotStructs()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNotStructs());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotImplementInterface(intf)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotImplementInterface(Type intf)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotImplementInterface(intf)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInNamespace(string fullName)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotResideInNamespace(fullName)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInNamespaceMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotResideInNamespaceMatching(pattern)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInAssembly(string fullName)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotResideInAssembly(fullName)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInAssemblyMatching(string pattern)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotResideInAssemblyMatching(pattern)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInAssembly(
            Assembly assembly,
            params Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotResideInAssembly(
            Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies
        )
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotResideInAssembly(assembly, moreAssemblies)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotHavePropertyMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotHaveFieldMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotHaveMethodMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.AddPredicate(
                TypePredicatesDefinition<TRuleType>.DoNotHaveMemberWithName(name)
            );
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }

        public TGivenRuleTypeConjunction AreNotNested()
        {
            _ruleCreator.AddPredicate(TypePredicatesDefinition<TRuleType>.AreNotNested());
            return Create<TGivenRuleTypeConjunction, TRuleType>(_ruleCreator);
        }
    }
}
