using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesThat : GivenTypesThat<GivenAttributesConjunction, Attribute>,
        IAttributesThat<GivenAttributesConjunction>
    {
        public GivenAttributesThat(IArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenAttributesConjunction AreAbstract()
        {
            _ruleCreator.AddObjectFilter(AttributesFilterDefinition.AreAbstract());
            return new GivenAttributesConjunction(_ruleCreator);
        }

        public GivenAttributesConjunction AreSealed()
        {
            _ruleCreator.AddObjectFilter(AttributesFilterDefinition.AreSealed());
            return new GivenAttributesConjunction(_ruleCreator);
        }


        //Negations


        public GivenAttributesConjunction AreNotAbstract()
        {
            _ruleCreator.AddObjectFilter(AttributesFilterDefinition.AreNotAbstract());
            return new GivenAttributesConjunction(_ruleCreator);
        }

        public GivenAttributesConjunction AreNotSealed()
        {
            _ruleCreator.AddObjectFilter(AttributesFilterDefinition.AreNotSealed());
            return new GivenAttributesConjunction(_ruleCreator);
        }
    }
}