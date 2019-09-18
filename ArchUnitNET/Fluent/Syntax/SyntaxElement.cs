using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax
{
    public abstract class SyntaxElement<T> where T : ICanBeAnalyzed
    {
        // ReSharper disable once InconsistentNaming
        protected readonly ArchRuleCreator<T> _ruleCreator;

        protected SyntaxElement([NotNull] ArchRuleCreator<T> ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public override string ToString()
        {
            return _ruleCreator.ToString();
        }
    }
}