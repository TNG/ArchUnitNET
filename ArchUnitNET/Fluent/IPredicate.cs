using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IPredicate<TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        bool CheckPredicate(TRuleType obj, Architecture architecture);
    }
}