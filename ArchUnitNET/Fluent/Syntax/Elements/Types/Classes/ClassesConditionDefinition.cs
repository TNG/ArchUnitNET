using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassesConditionDefinition
    {
        public static SimpleCondition<Class> BeAbstract()
        {
            return new SimpleCondition<Class>(cls => cls.IsAbstract, "be abstract", "is not abstract");
        }


        //Negations


        public static SimpleCondition<Class> NotBeAbstract()
        {
            return new SimpleCondition<Class>(cls => !cls.IsAbstract, "not be abstract", "is abstract");
        }
    }
}