using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax
{
    public abstract class SyntaxElement<TRuleType> : IHasDescription where TRuleType : ICanBeAnalyzed
    {
        // ReSharper disable once InconsistentNaming
        protected readonly ArchRuleCreator<TRuleType> _ruleCreator;

        protected SyntaxElement([NotNull] ArchRuleCreator<TRuleType> ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public string Description => _ruleCreator.Description;

        public override string ToString()
        {
            return Description;
        }
    }
}