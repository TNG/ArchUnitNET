using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public static class BasicObjectProviderDefinition
    {
        public static readonly BasicObjectProvider<IType> Types = new BasicObjectProvider<IType>(
            architecture => architecture.Types,
            "Types"
        );

        public static readonly BasicObjectProvider<Attribute> Attributes =
            new BasicObjectProvider<Attribute>(
                architecture => architecture.Attributes,
                "Attributes"
            );

        public static readonly BasicObjectProvider<Class> Classes = new BasicObjectProvider<Class>(
            architecture => architecture.Classes,
            "Classes"
        );

        public static readonly BasicObjectProvider<Interface> Interfaces =
            new BasicObjectProvider<Interface>(
                architecture => architecture.Interfaces,
                "Interfaces"
            );

        public static readonly BasicObjectProvider<IMember> Members =
            new BasicObjectProvider<IMember>(architecture => architecture.Members, "Members");

        public static readonly BasicObjectProvider<FieldMember> FieldMembers =
            new BasicObjectProvider<FieldMember>(
                architecture => architecture.FieldMembers,
                "Field members"
            );

        public static readonly BasicObjectProvider<MethodMember> MethodMembers =
            new BasicObjectProvider<MethodMember>(
                architecture => architecture.MethodMembers,
                "Method members"
            );

        public static readonly BasicObjectProvider<PropertyMember> PropertyMembers =
            new BasicObjectProvider<PropertyMember>(
                architecture => architecture.PropertyMembers,
                "Property members"
            );

        public static readonly BasicObjectProvider<IType> TypesIncludingReferenced =
            new BasicObjectProvider<IType>(
                architecture => architecture.Types.Concat(architecture.ReferencedTypes),
                "Types (including referenced)"
            );

        public static readonly BasicObjectProvider<Attribute> AttributesIncludingReferenced =
            new BasicObjectProvider<Attribute>(
                architecture => architecture.Attributes.Concat(architecture.ReferencedAttributes),
                "Attributes (including referenced)"
            );

        public static readonly BasicObjectProvider<Class> ClassesIncludingReferenced =
            new BasicObjectProvider<Class>(
                architecture => architecture.Classes.Concat(architecture.ReferencedClasses),
                "Classes (including referenced)"
            );

        public static readonly BasicObjectProvider<Interface> InterfacesIncludingReferenced =
            new BasicObjectProvider<Interface>(
                architecture => architecture.Interfaces.Concat(architecture.ReferencedInterfaces),
                "Interfaces (including referenced)"
            );
    }
}
