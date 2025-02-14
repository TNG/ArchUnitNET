using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax
{
    public abstract class SyntaxElement<TRuleType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once InconsistentNaming
        protected readonly IArchRuleCreator<TRuleType> _ruleCreator;

        protected SyntaxElement([NotNull] IArchRuleCreator<TRuleType> ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public string Description => _ruleCreator.Description;

        public override string ToString()
        {
            return Description;
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

            return obj.GetType() == GetType() && Equals((SyntaxElement<TRuleType>)obj);
        }

        private bool Equals(SyntaxElement<TRuleType> other)
        {
            return _ruleCreator.Equals(other._ruleCreator);
        }

        public override int GetHashCode()
        {
            return _ruleCreator != null ? _ruleCreator.GetHashCode() : 0;
        }
    }
}
