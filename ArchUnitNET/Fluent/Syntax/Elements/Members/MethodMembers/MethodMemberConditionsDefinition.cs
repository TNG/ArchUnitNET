using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
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

        public static ICondition<MethodMember> BeCalledBy(string pattern, bool useRegularExpressions = false)
        {
            return new SimpleCondition<MethodMember>(
                member => member.IsCalledBy(pattern, useRegularExpressions),
                "be called by types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "is called by a type with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<MethodMember> HaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            return new SimpleCondition<MethodMember>(
                member => member.HasDependencyInMethodBodyTo(pattern, useRegularExpressions),
                "have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"",
                "does not have dependencies in method body to a type with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
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

        public static ICondition<MethodMember> NotBeCalledBy(string pattern, bool useRegularExpressions = false)
        {
            ConditionResult Condition(MethodMember member)
            {
                var pass = true;
                var description = "is called by";
                foreach (var type in member.GetMethodCallDependencies(true).Select(dependency => dependency.Origin)
                    .Distinct())
                {
                    if (type.FullNameMatches(pattern, useRegularExpressions))
                    {
                        description += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(member, pass, description);
            }

            return new SimpleCondition<MethodMember>(Condition,
                "not be called by types with full name " + (useRegularExpressions ? "matching" : "containing") + " \"" +
                pattern + "\"");
        }

        public static ICondition<MethodMember> NotHaveDependencyInMethodBodyTo(string pattern,
            bool useRegularExpressions = false)
        {
            ConditionResult Condition(MethodMember member)
            {
                var pass = true;
                var description = "does have dependencies in method body to";
                foreach (var type in member.GetBodyTypeMemberDependencies().Select(dependency => dependency.Target)
                    .Distinct())
                {
                    if (type.FullNameMatches(pattern, useRegularExpressions))
                    {
                        description += (pass ? " " : " and ") + type.FullName;
                        pass = false;
                    }
                }

                return new ConditionResult(member, pass, description);
            }

            return new SimpleCondition<MethodMember>(Condition,
                "not have dependencies in method body to types with full name " +
                (useRegularExpressions ? "matching" : "containing") + " \"" + pattern + "\"");
        }
    }
}