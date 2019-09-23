using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public static class ObjectProviderDefinition
    {
        public static readonly ObjectProvider<IType> Types =
            new ObjectProvider<IType>(architecture => architecture.Types, "Types");

        public static readonly ObjectProvider<Attribute> Attributes =
            new ObjectProvider<Attribute>(architecture => architecture.Attributes, "Attributes");

        public static readonly ObjectProvider<Class> Classes =
            new ObjectProvider<Class>(architecture => architecture.Classes, "Classes");

        public static readonly ObjectProvider<Interface> Interfaces =
            new ObjectProvider<Interface>(architecture => architecture.Interfaces, "Interfaces");

        public static readonly ObjectProvider<IMember> Members =
            new ObjectProvider<IMember>(architecture => architecture.Members, "Members");

        public static readonly ObjectProvider<FieldMember> FieldMembers =
            new ObjectProvider<FieldMember>(architecture => architecture.FieldMembers, "Field members");

        public static readonly ObjectProvider<MethodMember> MethodMembers =
            new ObjectProvider<MethodMember>(architecture => architecture.MethodMembers, "Method members");

        public static readonly ObjectProvider<PropertyMember> PropertyMembers =
            new ObjectProvider<PropertyMember>(architecture => architecture.PropertyMembers, "Property members");
    }
}