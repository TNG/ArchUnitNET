using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IPredicate<in TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        bool CheckPredicate(TRuleType obj, Architecture architecture);
    }
}