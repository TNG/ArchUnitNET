using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class EvaluationResult<TRuleType> : IEvaluationResult where TRuleType : ICanBeAnalyzed
    {
        private readonly IArchRuleCreator<TRuleType> _archRuleCreator;

        public EvaluationResult(TRuleType obj, bool passed, string description,
            IArchRuleCreator<TRuleType> archRuleCreator, Architecture architecture)
        {
            Object = obj;
            Passed = passed;
            Description = description;
            _archRuleCreator = archRuleCreator;
            Architecture = architecture;
        }

        public ICanBeAnalyzed Object { get; }
        public bool Passed { get; }
        public string Description { get; }
        public string ArchRuleDescription => _archRuleCreator.Description;
        public Architecture Architecture { get; }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(EvaluationResult<TRuleType> other)
        {
            return string.Equals(Description, other.Description) &&
                   Equals(Object, other.Object) &&
                   Equals(Passed, other.Passed) &&
                   Equals(_archRuleCreator, other._archRuleCreator) &&
                   Equals(Architecture, other.Architecture);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((EvaluationResult<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Description != null ? Description.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Object != null ? Object.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Passed.GetHashCode();
                hashCode = (hashCode * 397) ^ (_archRuleCreator != null ? _archRuleCreator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Architecture != null ? Architecture.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}