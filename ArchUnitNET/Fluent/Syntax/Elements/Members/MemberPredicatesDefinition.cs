using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members
{
    public static class MemberPredicatesDefinition<T>
        where T : IMember
    {
        public static IPredicate<T> AreDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> members, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return members.Intersect(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = objectProvider.FormatDescription(
                "are declared in no type (always false)",
                "are declared in",
                "are declared in"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreStatic()
        {
            return new SimplePredicate<T>(
                member => member.IsStatic.HasValue && member.IsStatic.Value,
                "are static"
            );
        }

        public static IPredicate<T> AreReadOnly()
        {
            return new SimplePredicate<T>(
                member => member.Writability == Writability.ReadOnly,
                "are read only"
            );
        }

        public static IPredicate<T> AreImmutable()
        {
            return new SimplePredicate<T>(
                member => member.Writability.IsImmutable(),
                "are immutable"
            );
        }

        //Negations

        public static IPredicate<T> AreNotDeclaredIn(IObjectProvider<IType> objectProvider)
        {
            IEnumerable<T> Filter(IEnumerable<T> members, Architecture architecture)
            {
                var types = objectProvider.GetObjects(architecture);
                return members.Except(types.SelectMany(type => type.Members).OfType<T>());
            }

            var description = objectProvider.FormatDescription(
                "are declared in any type (always true)",
                "are not declared in",
                "are not declared in"
            );
            return new ArchitecturePredicate<T>(Filter, description);
        }

        public static IPredicate<T> AreNotStatic()
        {
            return new SimplePredicate<T>(
                member => member.IsStatic.HasValue && !member.IsStatic.Value,
                "are not static"
            );
        }

        public static IPredicate<T> AreNotReadOnly()
        {
            return new SimplePredicate<T>(
                member => member.Writability != Writability.ReadOnly,
                "are not read only"
            );
        }

        public static IPredicate<T> AreNotImmutable()
        {
            return new SimplePredicate<T>(
                member => !member.Writability.IsImmutable(),
                "are not immutable"
            );
        }
    }
}
