using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public class GivenAttributesThat : GivenTypesThat<GivenAttributesConjunction, Attribute>,
        IAttributesThat<GivenAttributesConjunction>
    {
        public GivenAttributesThat(ArchRuleCreator<Attribute> ruleCreator) : base(ruleCreator)
        {
        }

        public GivenAttributesConjunction AreAbstract()
        {
            _ruleCreator.AddObjectFilter(attribute => attribute.IsAbstract);
            return new GivenAttributesConjunction(_ruleCreator);
        }


        //Negations


        public GivenAttributesConjunction AreNotAbstract()
        {
            _ruleCreator.AddObjectFilter(attribute => !attribute.IsAbstract);
            return new GivenAttributesConjunction(_ruleCreator);
        }
    }
}