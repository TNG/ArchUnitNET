using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ConditionResult : IConditionResult
    {
        public ICanBeAnalyzed AnalyzedObject { get; }

        public bool Pass { get; }

        public string Description { get; }

        public ConditionResult(ICanBeAnalyzed analyzedObject, bool pass, string description = null)
        {
            Pass = pass;
            AnalyzedObject = analyzedObject;
            Description = pass ? (description ?? "passed") : description;
        }
    }
}
