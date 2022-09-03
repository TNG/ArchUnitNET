//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers
{
    public class PropertyMembersShould : MembersShould<PropertyMembersShouldConjunction, PropertyMember>,
        IComplexPropertyMemberConditions
    {
        public PropertyMembersShould(IArchRuleCreator<PropertyMember> ruleCreator) : base(ruleCreator)
        {
        }

        public PropertyMembersShouldConjunction HaveGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePrivateGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePublicGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePublicGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveProtectedGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInternalGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveInternalGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedInternalGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveProtectedInternalGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateProtectedGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePrivateProtectedGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePrivateSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePublicSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePublicSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveProtectedInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveProtectedInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HavePrivateProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HavePrivateProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction HaveInitSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.HaveInitSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction BeImmutable()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.BeImmutable());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction BeVirtual()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.BeVirtual());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }


        //Negations


        public PropertyMembersShouldConjunction NotHaveGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePrivateGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePublicGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePublicGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveProtectedGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInternalGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveInternalGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedInternalGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveProtectedInternalGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateProtectedGetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePrivateProtectedGetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePrivateSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePublicSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePublicSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveProtectedInternalSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveProtectedInternalSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHavePrivateProtectedSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHavePrivateProtectedSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotHaveInitSetter()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotHaveInitSetter());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotBeImmutable()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotBeImmutable());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }

        public PropertyMembersShouldConjunction NotBeVirtual()
        {
            _ruleCreator.AddCondition(PropertyMemberConditionsDefinition.NotBeVirtual());
            return new PropertyMembersShouldConjunction(_ruleCreator);
        }
    }
}