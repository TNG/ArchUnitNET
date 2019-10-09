using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassPredicatesDefinition
    {
        public static ObjectFilter<Class> AreAbstract()
        {
            return new ObjectFilter<Class>(cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value, "are abstract");
        }

        public static ObjectFilter<Class> AreSealed()
        {
            return new ObjectFilter<Class>(cls => !cls.IsSealed.HasValue || cls.IsSealed.Value, "are sealed");
        }

        public static ObjectFilter<Class> AreValueTypes()
        {
            return new ObjectFilter<Class>(cls => !cls.IsValueType.HasValue || cls.IsValueType.Value,
                "are value types");
        }

        public static ObjectFilter<Class> AreEnums()
        {
            return new ObjectFilter<Class>(cls => !cls.IsEnum.HasValue || cls.IsEnum.Value, "are enums");
        }

        public static ObjectFilter<Class> AreStructs()
        {
            return new ObjectFilter<Class>(cls => !cls.IsStruct.HasValue || cls.IsStruct.Value, "are structs");
        }

        //Negations

        public static ObjectFilter<Class> AreNotAbstract()
        {
            return new ObjectFilter<Class>(cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "are not abstract");
        }

        public static ObjectFilter<Class> AreNotSealed()
        {
            return new ObjectFilter<Class>(cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value, "are not sealed");
        }

        public static ObjectFilter<Class> AreNotValueTypes()
        {
            return new ObjectFilter<Class>(cls => !cls.IsValueType.HasValue || !cls.IsValueType.Value,
                "are not value types");
        }

        public static ObjectFilter<Class> AreNotEnums()
        {
            return new ObjectFilter<Class>(cls => !cls.IsEnum.HasValue || !cls.IsEnum.Value, "are not enums");
        }

        public static ObjectFilter<Class> AreNotStructs()
        {
            return new ObjectFilter<Class>(cls => !cls.IsStruct.HasValue || !cls.IsStruct.Value, "are not structs");
        }
    }
}