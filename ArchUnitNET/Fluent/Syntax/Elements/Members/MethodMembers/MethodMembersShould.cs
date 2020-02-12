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
    public class MethodMembersShould : MembersShould<MethodMembersShouldConjunction, MethodMember>,
        IComplexMethodMemberConditions
    {
        public MethodMembersShould(IArchRuleCreator<MethodMember> ruleCreator) : base(ruleCreator)
        {
        }

        public MethodMembersShouldConjunction BeConstructor()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(pattern, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(patterns, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(firstType, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(type, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction BeCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(patterns, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(firstType, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(type, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        //Negations


        public MethodMembersShouldConjunction BeNoConstructor()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.BeNoConstructor());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeVirtual());
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(string pattern, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(pattern, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IEnumerable<string> patterns,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(patterns, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(firstType, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(type, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotBeCalledBy(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(MethodMemberConditionsDefinition.NotBeCalledBy(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction
            NotHaveDependencyInMethodBodyTo(IEnumerable<string> patterns, bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(patterns, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(IType firstType, params IType[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(firstType, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(Type type, params Type[] moreTypes)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(type, moreTypes));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(IObjectProvider<IType> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(IEnumerable<IType> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(IEnumerable<Type> types)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(types));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}