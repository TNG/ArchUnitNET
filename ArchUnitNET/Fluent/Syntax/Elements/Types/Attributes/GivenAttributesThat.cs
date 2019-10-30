//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesThat : GivenTypesThat<GivenAttributesConjunction, Attribute>,
        IAttributePredicates<GivenAttributesConjunction>
    {
        public GivenAttributesThat(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenAttributesConjunction AreAbstract()
        {
            _ruleCreator.AddPredicate(AttributePredicatesDefinition.AreAbstract());
            return new GivenAttributesConjunction(_ruleCreator);
        }

        public GivenAttributesConjunction AreSealed()
        {
            _ruleCreator.AddPredicate(AttributePredicatesDefinition.AreSealed());
            return new GivenAttributesConjunction(_ruleCreator);
        }


        //Negations


        public GivenAttributesConjunction AreNotAbstract()
        {
            _ruleCreator.AddPredicate(AttributePredicatesDefinition.AreNotAbstract());
            return new GivenAttributesConjunction(_ruleCreator);
        }

        public GivenAttributesConjunction AreNotSealed()
        {
            _ruleCreator.AddPredicate(AttributePredicatesDefinition.AreNotSealed());
            return new GivenAttributesConjunction(_ruleCreator);
        }
    }
}