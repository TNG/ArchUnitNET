using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberConditionsDefinition
    {
        public static ICondition<MethodMember> BeConstructor()
        {
            return new SimpleCondition<MethodMember>(member => member.IsConstructor(), "be a constructor",
                "is no constructor");
        }

        public static ICondition<MethodMember> BeVirtual()
        {
            return new SimpleCondition<MethodMember>(member => member.IsVirtual, "be virtual", "is not virtual");
        }


        //Negations


        public static ICondition<MethodMember> BeNoConstructor()
        {
            return new SimpleCondition<MethodMember>(member => !member.IsConstructor(), "be no constructor",
                "is a constructor");
        }

        public static ICondition<MethodMember> NotBeVirtual()
        {
            return new SimpleCondition<MethodMember>(member => !member.IsVirtual, "not be virtual", "is virtual");
        }
    }
}