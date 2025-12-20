using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public abstract class PartialArchRuleConjunction
    {
        internal abstract ICanBeEvaluated LeftArchRule { get; }

        internal abstract LogicalConjunction LogicalConjunction { get; }

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

            return GetType() == obj.GetType() && Equals((PartialArchRuleConjunction)obj);
        }
        
        private bool Equals(PartialArchRuleConjunction other)
        {
            return Equals(LeftArchRule, other.LeftArchRule)
                && Equals(LogicalConjunction, other.LogicalConjunction);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = LeftArchRule != null ? LeftArchRule.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (LogicalConjunction != null ? LogicalConjunction.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
