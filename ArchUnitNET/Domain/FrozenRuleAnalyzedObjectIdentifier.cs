namespace ArchUnitNET.Domain
{
    public class FrozenRuleAnalyzedObjectIdentifier : FrozenRuleIdentifier
    {
        ICanBeAnalyzed _analyzedObject;
        
        public FrozenRuleAnalyzedObjectIdentifier(ICanBeAnalyzed analyzedObject)
            : base(analyzedObject.FullName)
        {
            _analyzedObject = analyzedObject;
        }

        public string GetAssemblyName()
        {
            return _analyzedObject.Assembly.Name;
        }

        public override string ToString()
        {
            return $"Type: {Identifier}";
        }

        public override bool Equals(object obj)
        {
            return obj != null
                && obj.GetType() == GetType()
                && Identifier.Equals(((FrozenRuleAnalyzedObjectIdentifier)obj).Identifier)
                && GetAssemblyName().Equals(((FrozenRuleAnalyzedObjectIdentifier)obj).GetAssemblyName());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ GetType().GetHashCode();
                hashCode = (hashCode * 397) ^ Identifier.GetHashCode();
                hashCode = (hashCode * 397) ^ GetAssemblyName().GetHashCode();
                return hashCode;
            }
        }
    }
}