namespace ArchUnitNET.Domain
{
    public class Attribute : Class
    {
        public Attribute(IType type, bool? isAbstract, bool? isSealed)
            : base(type, isAbstract, isSealed) { }

        public Attribute(Class cls)
            : base(cls) { }
    }
}
