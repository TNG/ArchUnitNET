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
    public class ShouldRelateToTypesThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType> :
        ShouldRelateToObjectsThat<TRuleTypeShouldConjunction, TReferenceType, TRuleType>,
        ITypePredicates<TRuleTypeShouldConjunction, TReferenceType>
        where TReferenceType : IType
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once MemberCanBeProtected.Global
        public ShouldRelateToTypesThat(IArchRuleCreator<TRuleType> ruleCreator) : base(ruleCreator)
        {
        }

        public TRuleTypeShouldConjunction Are(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.Are(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction Are(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.Are(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreValueTypes()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreValueTypes());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreEnums()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreEnums());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreStructs()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreStructs());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ImplementInterface(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ImplementInterface(Type intf)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        public TRuleTypeShouldConjunction ResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ResideInNamespace(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ResideInAssembly(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ResideInAssembly(assembly, moreAssemblies));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction ResideInAssembly(Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.ResideInAssembly(assembly, moreAssemblies));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.HaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.HaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction HaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.HaveMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNested()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }


        //Negations


        public TRuleTypeShouldConjunction AreNot(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNot(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNot(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNot(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(patterns, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(Type firstType, params Type[] moreTypes)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(firstType, moreTypes));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IObjectProvider<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IEnumerable<IType> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotAssignableTo(IEnumerable<Type> types)
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotAssignableTo(types));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotValueTypes()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotValueTypes());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotEnums()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotEnums());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotStructs()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotStructs());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotImplementInterface(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(Interface intf)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotImplementInterface(Type intf)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotImplementInterface(intf));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInNamespace(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotResideInNamespace(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInAssembly(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotResideInAssembly(pattern, useRegularExpressions));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInAssembly(Assembly assembly, params Assembly[] moreAssemblies)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotResideInAssembly(assembly, moreAssemblies));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotResideInAssembly(Domain.Assembly assembly,
            params Domain.Assembly[] moreAssemblies)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotResideInAssembly(assembly, moreAssemblies));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHavePropertyMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHavePropertyMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveFieldMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHaveFieldMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMethodMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction DoNotHaveMemberWithName(string name)
        {
            _ruleCreator.ContinueComplexCondition(
                TypePredicatesDefinition<TReferenceType>.DoNotHaveMethodMemberWithName(name));
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }

        public TRuleTypeShouldConjunction AreNotNested()
        {
            _ruleCreator.ContinueComplexCondition(TypePredicatesDefinition<TReferenceType>.AreNotNested());
            return Create<TRuleTypeShouldConjunction, TRuleType>(_ruleCreator);
        }
    }
}