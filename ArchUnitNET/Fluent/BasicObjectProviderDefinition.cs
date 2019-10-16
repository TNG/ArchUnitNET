using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public static class BasicObjectProviderDefinition
    {
        public static readonly BasicObjectProvider<IType> Types =
            new BasicObjectProvider<IType>(architecture => architecture.Types, "Types");

        public static readonly BasicObjectProvider<Attribute> Attributes =
            new BasicObjectProvider<Attribute>(architecture => architecture.Attributes, "Attributes");

        public static readonly BasicObjectProvider<Class> Classes =
            new BasicObjectProvider<Class>(architecture => architecture.Classes, "Classes");

        public static readonly BasicObjectProvider<Interface> Interfaces =
            new BasicObjectProvider<Interface>(architecture => architecture.Interfaces, "Interfaces");

        public static readonly BasicObjectProvider<IMember> Members =
            new BasicObjectProvider<IMember>(architecture => architecture.Members, "Members");

        public static readonly BasicObjectProvider<FieldMember> FieldMembers =
            new BasicObjectProvider<FieldMember>(architecture => architecture.FieldMembers, "Field members");

        public static readonly BasicObjectProvider<MethodMember> MethodMembers =
            new BasicObjectProvider<MethodMember>(architecture => architecture.MethodMembers, "Method members");

        public static readonly BasicObjectProvider<PropertyMember> PropertyMembers =
            new BasicObjectProvider<PropertyMember>(architecture => architecture.PropertyMembers, "Property members");
    }
}