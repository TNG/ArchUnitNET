namespace ArchUnitNET.Domain
{
    public class StringIdentifier
    {
        public readonly string Identifier;

        public StringIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return Identifier;
        }

        public override bool Equals(object obj)
        {
            return obj != null
                && obj.GetType() == GetType()
                && Identifier == ((StringIdentifier)obj).Identifier;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ GetType().GetHashCode();
                hashCode = (hashCode * 397) ^ Identifier.GetHashCode();
                return hashCode;
            }
        }
    }
}
