using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassConditionsDefinition
    {
        public static IOrderedCondition<Class> BeAbstract()
        {
            return new SimpleCondition<Class>(
                cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value,
                "be abstract",
                "is not abstract"
            );
        }

        public static IOrderedCondition<Class> BeSealed()
        {
            return new SimpleCondition<Class>(
                cls => !cls.IsSealed.HasValue || cls.IsSealed.Value,
                "be sealed",
                "is not sealed"
            );
        }

        public static IOrderedCondition<Class> BeRecord()
        {
            return new SimpleCondition<Class>(
                cls => !cls.IsRecord.HasValue || cls.IsRecord.Value,
                "be record",
                "is not record"
            );
        }

        public static IOrderedCondition<Class> BeImmutable()
        {
            return new SimpleCondition<Class>(
                cls =>
                    cls.Members.Where(m => m.IsStatic == false)
                        .All(m => m.Writability.IsImmutable()),
                "be immutable",
                "is not immutable"
            );
        }

        //Negations

        public static IOrderedCondition<Class> NotBeAbstract()
        {
            return new SimpleCondition<Class>(
                cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "not be abstract",
                "is abstract"
            );
        }

        public static IOrderedCondition<Class> NotBeSealed()
        {
            return new SimpleCondition<Class>(
                cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value,
                "not be sealed",
                "is sealed"
            );
        }

        public static IOrderedCondition<Class> NotBeRecord()
        {
            return new SimpleCondition<Class>(
                cls => !cls.IsRecord.HasValue || !cls.IsRecord.Value,
                "not be record",
                "is record"
            );
        }

        public static IOrderedCondition<Class> NotBeImmutable()
        {
            return new SimpleCondition<Class>(
                cls => cls.Members.Any(m => m.IsStatic == false && !m.Writability.IsImmutable()),
                "not be immutable",
                "is immutable"
            );
        }
    }
}
