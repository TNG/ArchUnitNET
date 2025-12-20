using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Conditions
{
    public interface IConditionResult
    {
        ICanBeAnalyzed AnalyzedObject { get; }

        bool Pass { get; }

        [CanBeNull]
        string Description { get; }
    }
}
