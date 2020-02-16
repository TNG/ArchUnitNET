//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public class GivenMethodMembersThat : GivenMembersThat<GivenMethodMembersConjunction, MethodMember>,
        IMethodMemberPredicates<GivenMethodMembersConjunction>
    {
        public GivenMethodMembersThat(IArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenMethodMembersConjunction AreConstructors()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreConstructors());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreVirtual()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreVirtual());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(patterns, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(firstType, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(type, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreCalledBy(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(patterns, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(firstType, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(type, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.HaveDependencyInMethodBodyTo(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        //Negations


        public GivenMethodMembersConjunction AreNoConstructors()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNoConstructors());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotVirtual()
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotVirtual());
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(patterns, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(firstType, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(type, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction AreNotCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(MethodMemberPredicatesDefinition.AreNotCalledBy(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(patterns, useRegularExpressions));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(IType firstType,
            params IType[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(firstType, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(type, moreTypes));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }

        public GivenMethodMembersConjunction DoNotHaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddPredicate(
                MethodMemberPredicatesDefinition.DoNotHaveDependencyInMethodBodyTo(types));
            return new GivenMethodMembersConjunction(_ruleCreator);
        }
    }
}