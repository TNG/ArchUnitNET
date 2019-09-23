using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class EvaluationResult
    {
        public EvaluationResult(ICanBeAnalyzed obj, bool passed, string description,
            ICanBeEvaluated archRule, Architecture architecture)
        {
            Object = obj;
            Passed = passed;
            Description = description;
            ArchRule = archRule;
            Architecture = architecture;
        }

        public ICanBeEvaluated ArchRule { get; }
        public ICanBeAnalyzed Object { get; }
        public bool Passed { get; }
        public string Description { get; }
        public Architecture Architecture { get; }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(EvaluationResult other)
        {
            return string.Equals(Description, other.Description) &&
                   Equals(Object, other.Object) &&
                   Equals(Passed, other.Passed) &&
                   Equals(ArchRule, other.ArchRule) &&
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

            return obj.GetType() == GetType() && Equals((EvaluationResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Description != null ? Description.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Object != null ? Object.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Passed.GetHashCode();
                hashCode = (hashCode * 397) ^ (ArchRule != null ? ArchRule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Architecture != null ? Architecture.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}