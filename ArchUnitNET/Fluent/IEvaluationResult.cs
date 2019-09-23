using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public interface IEvaluationResult
    {
        ICanBeAnalyzed Object { get; }
        bool Passed { get; }
        string Description { get; }
        string ArchRuleDescription { get; }
        Architecture Architecture { get; }
    }
}