using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ConditionResult
    {
        public readonly ICanBeAnalyzed AnalyzedObject;

        [CanBeNull]
        public readonly string FailDescription;
        public readonly bool Pass;

        public ConditionResult(
            ICanBeAnalyzed analyzedObject,
            bool pass,
            string failDescription = null
        )
        {
            Pass = pass;
            AnalyzedObject = analyzedObject;
            FailDescription = pass ? null : failDescription;
        }
    }
}
