using ArchUnitNET.Domain;

namespace ArchUnitNETTests.Domain
{
    public class TestArchitectureCache : ArchitectureCache
    {
        public int Size()
        {
            return Cache.Count;
        }
    }
}
