using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberPredicatesDefinition
    {
        public static ObjectFilter<MethodMember> AreConstructors()
        {
            return new ObjectFilter<MethodMember>(member => member.IsConstructor(), "are constructors");
        }

        public static ObjectFilter<MethodMember> AreVirtual()
        {
            return new ObjectFilter<MethodMember>(member => member.IsVirtual, "are virtual");
        }


        //Negations


        public static ObjectFilter<MethodMember> AreNoConstructors()
        {
            return new ObjectFilter<MethodMember>(member => !member.IsConstructor(), "are no constructors");
        }

        public static ObjectFilter<MethodMember> AreNotVirtual()
        {
            return new ObjectFilter<MethodMember>(member => !member.IsVirtual, "are not virtual");
        }
    }
}