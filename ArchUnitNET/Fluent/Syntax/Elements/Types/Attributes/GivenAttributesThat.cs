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
            _ruleCreator.AddObjectFilter(AttributePredicatesDefinition.AreAbstract());
            return new GivenAttributesConjunction(_ruleCreator);
        }

        public GivenAttributesConjunction AreSealed()
        {
            _ruleCreator.AddObjectFilter(AttributePredicatesDefinition.AreSealed());
            return new GivenAttributesConjunction(_ruleCreator);
        }


        //Negations


        public GivenAttributesConjunction AreNotAbstract()
        {
            _ruleCreator.AddObjectFilter(AttributePredicatesDefinition.AreNotAbstract());
            return new GivenAttributesConjunction(_ruleCreator);
        }

        public GivenAttributesConjunction AreNotSealed()
        {
            _ruleCreator.AddObjectFilter(AttributePredicatesDefinition.AreNotSealed());
            return new GivenAttributesConjunction(_ruleCreator);
        }
    }
}