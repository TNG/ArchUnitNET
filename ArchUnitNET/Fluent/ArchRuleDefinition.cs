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
        public static GivenTypes Types(bool includeReferenced = false)
        {
            return new GivenTypes(
                null,
                includeReferenced
                    ? BasicObjectProviderDefinition.TypesIncludingReferenced
                    : BasicObjectProviderDefinition.Types
            );
        }

        public static GivenAttributes Attributes(bool includeReferenced = false)
        {
            return new GivenAttributes(
                null,
                includeReferenced
                    ? BasicObjectProviderDefinition.AttributesIncludingReferenced
                    : BasicObjectProviderDefinition.Attributes
            );
        }

        public static GivenClasses Classes(bool includeReferenced = false)
        {
            return new GivenClasses(
                null,
                includeReferenced
                    ? BasicObjectProviderDefinition.ClassesIncludingReferenced
                    : BasicObjectProviderDefinition.Classes
            );
        }

        public static GivenInterfaces Interfaces(bool includeReferenced = false)
        {
            return new GivenInterfaces(
                null,
                includeReferenced
                    ? BasicObjectProviderDefinition.InterfacesIncludingReferenced
                    : BasicObjectProviderDefinition.Interfaces
            );
        }

        public static GivenMembers Members()
        {
            return new GivenMembers(null, BasicObjectProviderDefinition.Members);
        }

        public static GivenFieldMembers FieldMembers()
        {
            return new GivenFieldMembers(null, BasicObjectProviderDefinition.FieldMembers);
        }

        public static GivenMethodMembers MethodMembers()
        {
            return new GivenMethodMembers(null, BasicObjectProviderDefinition.MethodMembers);
        }

        public static GivenPropertyMembers PropertyMembers()
        {
            return new GivenPropertyMembers(null, BasicObjectProviderDefinition.PropertyMembers);
        }
    }
}
