using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassConditionsDefinition
    {
        public static SimpleCondition<Class> BeAbstract()
        {
            return new SimpleCondition<Class>(cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value, "be abstract",
                "is not abstract");
        }

        public static SimpleCondition<Class> BeSealed()
        {
            return new SimpleCondition<Class>(cls => !cls.IsSealed.HasValue || cls.IsSealed.Value, "be sealed",
                "is not sealed");
        }

        public static SimpleCondition<Class> BeValueTypes()
        {
            return new SimpleCondition<Class>(cls => !cls.IsValueType.HasValue || cls.IsValueType.Value,
                "be value types", "is no value type");
        }

        public static SimpleCondition<Class> BeEnums()
        {
            return new SimpleCondition<Class>(cls => !cls.IsEnum.HasValue || cls.IsEnum.Value, "be enums",
                "is no enum");
        }

        public static SimpleCondition<Class> BeStructs()
        {
            return new SimpleCondition<Class>(cls => !cls.IsStruct.HasValue || cls.IsStruct.Value, "be structs",
                "is no struct");
        }


        //Negations


        public static SimpleCondition<Class> NotBeAbstract()
        {
            return new SimpleCondition<Class>(cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "not be abstract", "is abstract");
        }

        public static SimpleCondition<Class> NotBeSealed()
        {
            return new SimpleCondition<Class>(cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value, "not be sealed",
                "is sealed");
        }

        public static SimpleCondition<Class> NotBeValueTypes()
        {
            return new SimpleCondition<Class>(cls => !cls.IsValueType.HasValue || !cls.IsValueType.Value,
                "not be value types", "is a value type");
        }

        public static SimpleCondition<Class> NotBeEnums()
        {
            return new SimpleCondition<Class>(cls => !cls.IsEnum.HasValue || !cls.IsEnum.Value, "not be enums",
                "is an enum");
        }

        public static SimpleCondition<Class> NotBeStructs()
        {
            return new SimpleCondition<Class>(cls => !cls.IsStruct.HasValue || !cls.IsStruct.Value, "not be structs",
                "is a struct");
        }
    }
}