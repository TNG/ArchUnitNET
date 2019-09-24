using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassesConditionDefinition
    {
        public static SimpleCondition<Class> BeAbstract()
        {
            return new SimpleCondition<Class>(cls => cls.IsAbstract, "be abstract", "is not abstract");
        }

        public static SimpleCondition<Class> BeSealed()
        {
            return new SimpleCondition<Class>(cls => cls.IsSealed, "be sealed", "is not sealed");
        }

        public static SimpleCondition<Class> BeValueTypes()
        {
            return new SimpleCondition<Class>(cls => cls.IsValueType, "be value types", "is no value type");
        }

        public static SimpleCondition<Class> BeEnums()
        {
            return new SimpleCondition<Class>(cls => cls.IsEnum, "be enums", "is no enum");
        }

        public static SimpleCondition<Class> BeStructs()
        {
            return new SimpleCondition<Class>(cls => cls.IsStruct, "be structs", "is no struct");
        }


        //Negations


        public static SimpleCondition<Class> NotBeAbstract()
        {
            return new SimpleCondition<Class>(cls => !cls.IsAbstract, "not be abstract", "is abstract");
        }

        public static SimpleCondition<Class> NotBeSealed()
        {
            return new SimpleCondition<Class>(cls => !cls.IsSealed, "not be sealed", "is sealed");
        }

        public static SimpleCondition<Class> NotBeValueTypes()
        {
            return new SimpleCondition<Class>(cls => !cls.IsValueType, "not be value types", "is a value type");
        }

        public static SimpleCondition<Class> NotBeEnums()
        {
            return new SimpleCondition<Class>(cls => !cls.IsEnum, "not be enums", "is an enum");
        }

        public static SimpleCondition<Class> NotBeStructs()
        {
            return new SimpleCondition<Class>(cls => !cls.IsStruct, "not be structs", "is a struct");
        }
    }
}