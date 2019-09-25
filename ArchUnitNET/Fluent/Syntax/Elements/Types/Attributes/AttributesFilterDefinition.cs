using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributesFilterDefinition
    {
        public static ObjectFilter<Attribute> AreAbstract()
        {
            return new ObjectFilter<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || attribute.IsAbstract.Value, "are abstract");
        }

        public static ObjectFilter<Attribute> AreSealed()
        {
            return new ObjectFilter<Attribute>(attribute => !attribute.IsSealed.HasValue || attribute.IsSealed.Value,
                "are sealed");
        }

        //Negations

        public static ObjectFilter<Attribute> AreNotAbstract()
        {
            return new ObjectFilter<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || !attribute.IsAbstract.Value, "are not abstract");
        }

        public static ObjectFilter<Attribute> AreNotSealed()
        {
            return new ObjectFilter<Attribute>(attribute => !attribute.IsSealed.HasValue || !attribute.IsSealed.Value,
                "are not sealed");
        }
    }
}