using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassesFilterDefinition
    {
        public static ObjectFilter<Class> AreAbstract()
        {
            return new ObjectFilter<Class>(cls => cls.IsAbstract, "are abstract");
        }

        //Negations

        public static ObjectFilter<Class> AreNotAbstract()
        {
            return new ObjectFilter<Class>(cls => !cls.IsAbstract, "are not abstract");
        }
    }
}