using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes
{
    public static class AttributesFilterDefinition
    {
        public static ObjectFilter<Attribute> AreAbstract()
        {
            return new ObjectFilter<Attribute>(cls => cls.IsAbstract, "are abstract");
        }

        //Negations

        public static ObjectFilter<Attribute> AreNotAbstract()
        {
            return new ObjectFilter<Attribute>(cls => !cls.IsAbstract, "are not abstract");
        }
    }
}