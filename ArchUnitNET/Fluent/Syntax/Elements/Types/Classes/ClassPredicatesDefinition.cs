using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassPredicatesDefinition
    {
        public static Predicate<Class> AreAbstract()
        {
            return new Predicate<Class>(cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value, "are abstract");
        }

        public static Predicate<Class> AreSealed()
        {
            return new Predicate<Class>(cls => !cls.IsSealed.HasValue || cls.IsSealed.Value, "are sealed");
        }

        public static Predicate<Class> AreValueTypes()
        {
            return new Predicate<Class>(cls => !cls.IsValueType.HasValue || cls.IsValueType.Value,
                "are value types");
        }

        public static Predicate<Class> AreEnums()
        {
            return new Predicate<Class>(cls => !cls.IsEnum.HasValue || cls.IsEnum.Value, "are enums");
        }

        public static Predicate<Class> AreStructs()
        {
            return new Predicate<Class>(cls => !cls.IsStruct.HasValue || cls.IsStruct.Value, "are structs");
        }

        //Negations

        public static Predicate<Class> AreNotAbstract()
        {
            return new Predicate<Class>(cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "are not abstract");
        }

        public static Predicate<Class> AreNotSealed()
        {
            return new Predicate<Class>(cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value, "are not sealed");
        }

        public static Predicate<Class> AreNotValueTypes()
        {
            return new Predicate<Class>(cls => !cls.IsValueType.HasValue || !cls.IsValueType.Value,
                "are not value types");
        }

        public static Predicate<Class> AreNotEnums()
        {
            return new Predicate<Class>(cls => !cls.IsEnum.HasValue || !cls.IsEnum.Value, "are not enums");
        }

        public static Predicate<Class> AreNotStructs()
        {
            return new Predicate<Class>(cls => !cls.IsStruct.HasValue || !cls.IsStruct.Value, "are not structs");
        }
    }
}