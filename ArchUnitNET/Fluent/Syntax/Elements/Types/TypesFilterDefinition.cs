using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public static class TypesFilterDefinition<T> where T : IType
    {
        public static ArchitectureObjectFilter<T> Are(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return architecture.GetTypeOfType(firstType).Equals(ruleType) ||
                       moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureObjectFilter<T>(Filter, description);
        }

        public static ObjectFilter<T> ImplementInterface(string pattern)
        {
            return new ObjectFilter<T>(type => type.ImplementsInterface(pattern),
                "implement interface \"" + pattern + "\"");
        }

        public static ObjectFilter<T> ImplementInterface(Interface intf)
        {
            return new ObjectFilter<T>(type => type.ImplementsInterface(intf),
                "implement interface \"" + intf.FullName + "\"");
        }

        public static ObjectFilter<T> ResideInNamespace(string pattern)
        {
            return new ObjectFilter<T>(type => type.ResidesInNamespace(pattern),
                "reside in namespace \"" + pattern + "\"");
        }

        public static ObjectFilter<T> HavePropertyMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasPropertyMemberWithName(name),
                "have property member with name\"" + name + "\"");
        }

        public static ObjectFilter<T> HaveFieldMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasFieldMemberWithName(name),
                "have field member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> HaveMethodMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasMethodMemberWithName(name),
                "have method member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> HaveMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => type.HasMemberWithName(name), "have member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> AreNested()
        {
            return new ObjectFilter<T>(type => type.IsNested, "are nested");
        }


        //Negations


        public static ArchitectureObjectFilter<T> AreNot(Type firstType, params Type[] moreTypes)
        {
            bool Filter(T ruleType, Architecture architecture)
            {
                return !architecture.GetTypeOfType(firstType).Equals(ruleType) &&
                       !moreTypes.Any(type => architecture.GetTypeOfType(type).Equals(ruleType));
            }

            var description = moreTypes.Aggregate("are not \"" + firstType.FullName + "\"",
                (current, obj) => current + " or \"" + obj.FullName + "\"");
            return new ArchitectureObjectFilter<T>(Filter, description);
        }


        public static ObjectFilter<T> DoNotImplementInterface(string pattern)
        {
            return new ObjectFilter<T>(type => !type.ImplementsInterface(pattern),
                "do not implement interface \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotImplementInterface(Interface intf)
        {
            return new ObjectFilter<T>(type => !type.ImplementsInterface(intf),
                "do not implement interface \"" + intf.FullName + "\"");
        }

        public static ObjectFilter<T> DoNotResideInNamespace(string pattern)
        {
            return new ObjectFilter<T>(type => !type.ResidesInNamespace(pattern),
                "do not reside in namespace \"" + pattern + "\"");
        }

        public static ObjectFilter<T> DoNotHavePropertyMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasPropertyMemberWithName(name),
                "do not have property member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveFieldMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasFieldMemberWithName(name),
                "do not have field member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveMethodMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasMethodMemberWithName(name),
                "do not have method member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> DoNotHaveMemberWithName(string name)
        {
            return new ObjectFilter<T>(type => !type.HasMemberWithName(name),
                "do not have member with name \"" + name + "\"");
        }

        public static ObjectFilter<T> AreNotNested()
        {
            return new ObjectFilter<T>(type => !type.IsNested, "are not nested");
        }
    }
}