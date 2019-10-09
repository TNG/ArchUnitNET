using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberPredicatesDefinition
    {
        public static Predicate<MethodMember> AreConstructors()
        {
            return new Predicate<MethodMember>(member => member.IsConstructor(), "are constructors");
        }

        public static Predicate<MethodMember> AreVirtual()
        {
            return new Predicate<MethodMember>(member => member.IsVirtual, "are virtual");
        }


        //Negations


        public static Predicate<MethodMember> AreNoConstructors()
        {
            return new Predicate<MethodMember>(member => !member.IsConstructor(), "are no constructors");
        }

        public static Predicate<MethodMember> AreNotVirtual()
        {
            return new Predicate<MethodMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}