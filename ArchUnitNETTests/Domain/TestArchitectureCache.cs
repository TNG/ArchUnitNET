using ArchUnitNET.Domain;

namespace ArchUnitNETTests.Domain
{
    public class TestArchitectureCache : ArchitectureCache
    {
        public long Size()
        {
            return _cache.GetCount();
        }
    }
}
