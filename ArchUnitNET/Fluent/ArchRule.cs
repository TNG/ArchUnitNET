using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class ArchRule<T> : SyntaxElement<T> where T : ICanBeAnalyzed
    {
        protected ArchRule(ArchRuleCreator<T> ruleCreator) : base(ruleCreator)
        {
        }

        public bool Check(Architecture architecture)
        {
            return _ruleCreator.CheckRule(architecture);
        }
    }
}