using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberPredicatesDefinition
    {
        public static IPredicate<MethodMember> AreConstructors()
        {
            return new Predicate<MethodMember>(member => member.IsConstructor(), "are constructors");
        }

        public static IPredicate<MethodMember> AreVirtual()
        {
            return new Predicate<MethodMember>(member => member.IsVirtual, "are virtual");
        }


        //Negations


        public static IPredicate<MethodMember> AreNoConstructors()
        {
            return new Predicate<MethodMember>(member => !member.IsConstructor(), "are no constructors");
        }

        public static IPredicate<MethodMember> AreNotVirtual()
        {
            return new Predicate<MethodMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}