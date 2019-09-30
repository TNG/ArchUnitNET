using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public class EvaluationResult
    {
        public EvaluationResult([CanBeNull] ICanBeAnalyzed obj, bool passed, string description,
            ICanBeEvaluated archRule, Architecture architecture)
        {
            EvaluatedObject = obj;
            Passed = passed;
            Description = description;
            ArchRule = archRule;
            Architecture = architecture;
        }

        public ICanBeEvaluated ArchRule { get; }
        [CanBeNull] public ICanBeAnalyzed EvaluatedObject { get; }
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
                   Equals(EvaluatedObject, other.EvaluatedObject) &&
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
                hashCode = (hashCode * 397) ^ (EvaluatedObject != null ? EvaluatedObject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Passed.GetHashCode();
                hashCode = (hashCode * 397) ^ (ArchRule != null ? ArchRule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Architecture != null ? Architecture.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}