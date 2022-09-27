namespace ArchUnitNET.Domain
{
    public enum Writability
    {
        ReadOnly,
        InitOnly,
        Writable
    }

    public static class WritabilityExtensions
    {
        public static bool IsImmutable(this Writability? writability)
        {
            return writability == null
                || writability == Writability.ReadOnly
                || writability == Writability.InitOnly;
        }
    }
}
