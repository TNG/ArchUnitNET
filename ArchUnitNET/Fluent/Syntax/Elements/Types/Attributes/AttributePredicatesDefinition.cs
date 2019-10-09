using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributePredicatesDefinition
    {
        public static IPredicate<Attribute> AreAbstract()
        {
            return new Predicate<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || attribute.IsAbstract.Value, "are abstract");
        }

        public static IPredicate<Attribute> AreSealed()
        {
            return new Predicate<Attribute>(attribute => !attribute.IsSealed.HasValue || attribute.IsSealed.Value,
                "are sealed");
        }

        //Negations

        public static IPredicate<Attribute> AreNotAbstract()
        {
            return new Predicate<Attribute>(
                attribute => !attribute.IsAbstract.HasValue || !attribute.IsAbstract.Value, "are not abstract");
        }

        public static IPredicate<Attribute> AreNotSealed()
        {
            return new Predicate<Attribute>(attribute => !attribute.IsSealed.HasValue || !attribute.IsSealed.Value,
                "are not sealed");
        }
    }
}