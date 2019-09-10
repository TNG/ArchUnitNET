using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax
{
    public class SyntaxElement<T> where T : ICanBeAnalyzed
    {
        protected readonly ArchRuleCreator<T> _ruleCreator;

        protected SyntaxElement([NotNull] ArchRuleCreator<T> ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }
    }
}