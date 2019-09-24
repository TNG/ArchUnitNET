using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassesFilterDefinition
    {
        public static ObjectFilter<Class> AreAbstract()
        {
            return new ObjectFilter<Class>(cls => cls.IsAbstract, "are abstract");
        }

        public static ObjectFilter<Class> AreSealed()
        {
            return new ObjectFilter<Class>(cls => cls.IsSealed, "are sealed");
        }

        public static ObjectFilter<Class> AreValueTypes()
        {
            return new ObjectFilter<Class>(cls => cls.IsValueType, "are value types");
        }

        public static ObjectFilter<Class> AreEnums()
        {
            return new ObjectFilter<Class>(cls => cls.IsEnum, "are enums");
        }

        public static ObjectFilter<Class> AreStructs()
        {
            return new ObjectFilter<Class>(cls => cls.IsStruct, "are structs");
        }

        //Negations

        public static ObjectFilter<Class> AreNotAbstract()
        {
            return new ObjectFilter<Class>(cls => !cls.IsAbstract, "are not abstract");
        }

        public static ObjectFilter<Class> AreNotSealed()
        {
            return new ObjectFilter<Class>(cls => !cls.IsSealed, "are not sealed");
        }

        public static ObjectFilter<Class> AreNotValueTypes()
        {
            return new ObjectFilter<Class>(cls => !cls.IsValueType, "are not value types");
        }

        public static ObjectFilter<Class> AreNotEnums()
        {
            return new ObjectFilter<Class>(cls => !cls.IsEnum, "are not enums");
        }

        public static ObjectFilter<Class> AreNotStructs()
        {
            return new ObjectFilter<Class>(cls => !cls.IsStruct, "are not structs");
        }
    }
}