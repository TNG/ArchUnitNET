using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributePredicatesDefinition
    {
        public static Predicate<Attribute> AreAbstract()
        {
            return new Predicate<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || attribute.IsAbstract.Value, "are abstract");
        }

        public static Predicate<Attribute> AreSealed()
        {
            return new Predicate<Attribute>(attribute => !attribute.IsSealed.HasValue || attribute.IsSealed.Value,
                "are sealed");
        }

        //Negations

        public static Predicate<Attribute> AreNotAbstract()
        {
            return new Predicate<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || !attribute.IsAbstract.Value, "are not abstract");
        }

        public static Predicate<Attribute> AreNotSealed()
        {
            return new Predicate<Attribute>(attribute => !attribute.IsSealed.HasValue || !attribute.IsSealed.Value,
                "are not sealed");
        }
    }
}