using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers
{
    public static class MethodMemberConditionsDefinition
    {
        public static SimpleCondition<MethodMember> BeConstructor()
        {
            return new SimpleCondition<MethodMember>(member => member.IsConstructor(), "be constructor",
                "is no constructor");
        }

        public static SimpleCondition<MethodMember> BeVirtual()
        {
            return new SimpleCondition<MethodMember>(member => member.IsVirtual, "be virtual", "is not virtual");
        }


        //Negations


        public static SimpleCondition<MethodMember> BeNoConstructor()
        {
            return new SimpleCondition<MethodMember>(member => !member.IsConstructor(), "be no constructor",
                "is a constructor");
        }

        public static SimpleCondition<MethodMember> NotBeVirtual()
        {
            return new SimpleCondition<MethodMember>(member => !member.IsVirtual, "not be virtual", "is virtual");
        }
    }
}