using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public readonly struct ConditionResult
    {
        public readonly bool Pass;
        [CanBeNull] public readonly string FailDescription;

        public ConditionResult(bool pass, string failDescription = null)
        {
            Pass = pass;
            FailDescription = pass ? null : failDescription;
        }
    }
}