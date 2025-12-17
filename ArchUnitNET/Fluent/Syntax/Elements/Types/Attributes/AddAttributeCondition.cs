using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public abstract class AddAttributeCondition<TNextElement>
        : AddTypeCondition<Attribute, TNextElement>,
            IAttributeConditions<TNextElement, Attribute>
    {
        internal AddAttributeCondition(IArchRuleCreator<Attribute> ruleCreator)
            : base(ruleCreator) { }

        public TNextElement BeAbstract() =>
            CreateNextElement(AttributeConditionsDefinition.BeAbstract());

        public TNextElement BeSealed() =>
            CreateNextElement(AttributeConditionsDefinition.BeSealed());

        //Negations

        public TNextElement NotBeAbstract() =>
            CreateNextElement(AttributeConditionsDefinition.NotBeAbstract());

        public TNextElement NotBeSealed() =>
            CreateNextElement(AttributeConditionsDefinition.NotBeSealed());
    }
}
