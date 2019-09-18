using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Members;
using ArchUnitNET.Fluent.Syntax.Elements.Members.FieldMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Members.PropertyMembers;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Attributes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces;

namespace ArchUnitNET.Fluent
{
    public static class ArchRuleDefinition
    {
        public static GivenTypes Types()
        {
            var ruleCreator = new ArchRuleCreator<IType>(architecture => architecture.Types, "Types");
            return new GivenTypes(ruleCreator);
        }

        public static GivenAttributes Attributes()
        {
            var ruleCreator = new ArchRuleCreator<Attribute>(architecture => architecture.Attributes, "Attributes");
            return new GivenAttributes(ruleCreator);
        }

        public static GivenClasses Classes()
        {
            var ruleCreator = new ArchRuleCreator<Class>(architecture => architecture.Classes, "Classes");
            return new GivenClasses(ruleCreator);
        }

        public static GivenInterfaces Interfaces()
        {
            var ruleCreator = new ArchRuleCreator<Interface>(architecture => architecture.Interfaces, "Interfaces");
            return new GivenInterfaces(ruleCreator);
        }

        public static GivenMembers Members()
        {
            var ruleCreator = new ArchRuleCreator<IMember>(architecture => architecture.Members, "Members");
            return new GivenMembers(ruleCreator);
        }

        public static GivenFieldMembers FieldMembers()
        {
            var ruleCreator =
                new ArchRuleCreator<FieldMember>(architecture => architecture.FieldMembers, "Field members");
            return new GivenFieldMembers(ruleCreator);
        }

        public static GivenMethodMembers MethodMembers()
        {
            var ruleCreator =
                new ArchRuleCreator<MethodMember>(architecture => architecture.MethodMembers, "Method members");
            return new GivenMethodMembers(ruleCreator);
        }

        public static GivenPropertyMembers PropertyMembers()
        {
            var ruleCreator =
                new ArchRuleCreator<PropertyMember>(architecture => architecture.PropertyMembers, "Property members");
            return new GivenPropertyMembers(ruleCreator);
        }
    }
}