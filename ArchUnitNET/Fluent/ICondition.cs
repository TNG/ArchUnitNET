using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface ICondition<T> : IHasFailDescription where T : ICanBeAnalyzed
    {
        bool Check(T obj, Architecture architecture);
        bool CheckNull();
    }
}