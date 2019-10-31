//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

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

        public MethodMembersShouldConjunction HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.HaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
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

        public MethodMembersShouldConjunction NotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            _ruleCreator.AddCondition(
                MethodMemberConditionsDefinition.NotHaveDependencyInMethodBodyTo(pattern, useRegularExpressions));
            return new MethodMembersShouldConjunction(_ruleCreator);
        }
    }
}