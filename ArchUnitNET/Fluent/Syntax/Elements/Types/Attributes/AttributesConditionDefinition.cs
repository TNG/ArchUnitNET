using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributesConditionDefinition
    {
        public static SimpleCondition<Attribute> BeAbstract()
        {
            return new SimpleCondition<Attribute>(attribute => attribute.IsAbstract, "be abstract", "is not abstract");
        }


        //Negations


        public static SimpleCondition<Attribute> NotBeAbstract()
        {
            return new SimpleCondition<Attribute>(attribute => !attribute.IsAbstract, "not be abstract", "is abstract");
        }
    }
}