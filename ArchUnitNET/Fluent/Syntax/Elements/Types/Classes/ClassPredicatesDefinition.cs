using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public static class ClassPredicatesDefinition
    {
        public static IPredicate<Class> AreAbstract()
        {
            return new SimplePredicate<Class>(
                cls => !cls.IsAbstract.HasValue || cls.IsAbstract.Value,
                "are abstract"
            );
        }

        public static IPredicate<Class> AreSealed()
        {
            return new SimplePredicate<Class>(
                cls => !cls.IsSealed.HasValue || cls.IsSealed.Value,
                "are sealed"
            );
        }

        public static IPredicate<Class> AreRecord()
        {
            return new SimplePredicate<Class>(
                cls => !cls.IsRecord.HasValue || cls.IsRecord.Value,
                "are record"
            );
        }

        public static IPredicate<Class> AreImmutable()
        {
            return new SimplePredicate<Class>(
                cls =>
                    cls.Members.Where(m => m.IsStatic == false)
                        .All(m => m.Writability.IsImmutable()),
                "are immutable"
            );
        }

        //Negations

        public static IPredicate<Class> AreNotAbstract()
        {
            return new SimplePredicate<Class>(
                cls => !cls.IsAbstract.HasValue || !cls.IsAbstract.Value,
                "are not abstract"
            );
        }

        public static IPredicate<Class> AreNotSealed()
        {
            return new SimplePredicate<Class>(
                cls => !cls.IsSealed.HasValue || !cls.IsSealed.Value,
                "are not sealed"
            );
        }

        public static IPredicate<Class> AreNotRecord()
        {
            return new SimplePredicate<Class>(
                cls => !cls.IsRecord.HasValue || !cls.IsRecord.Value,
                "are not record"
            );
        }

        public static IPredicate<Class> AreNotImmutable()
        {
            return new SimplePredicate<Class>(
                cls => cls.Members.Any(m => m.IsStatic == false && !m.Writability.IsImmutable()),
                "are not immutable"
            );
        }
    }
}
