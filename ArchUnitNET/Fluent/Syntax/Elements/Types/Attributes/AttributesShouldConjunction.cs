﻿using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class AttributesShouldConjunction : ObjectsShouldConjunction<AttributesShould, Attribute>
    {
        public AttributesShouldConjunction(ArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }
    }
}