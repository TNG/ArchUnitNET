using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributesConditionDefinition
    {
        public static SimpleCondition<Attribute> BeAbstract()
        {
            return new SimpleCondition<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || attribute.IsAbstract.Value, "be abstract",
                "is not abstract");
        }

        public static SimpleCondition<Attribute> BeSealed()
        {
            return new SimpleCondition<Attribute>(attribute => !attribute.IsSealed.HasValue || attribute.IsSealed.Value,
                "be sealed", "is not sealed");
        }


        //Negations


        public static SimpleCondition<Attribute> NotBeAbstract()
        {
            return new SimpleCondition<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || !attribute.IsAbstract.Value, "not be abstract",
                "is abstract");
        }

        public static SimpleCondition<Attribute> NotBeSealed()
        {
            return new SimpleCondition<Attribute>(
                attribute => !attribute.IsSealed.HasValue || !attribute.IsSealed.Value, "not be sealed", "is sealed");
        }
    }
}