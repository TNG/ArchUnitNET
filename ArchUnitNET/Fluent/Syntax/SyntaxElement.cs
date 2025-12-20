using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax
{
    public abstract class SyntaxElement<TRuleType> : IHasDescription
        where TRuleType : ICanBeAnalyzed
    {
        protected SyntaxElement(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
        {
            PartialArchRuleConjunction = partialArchRuleConjunction;
            ObjectProvider = objectProvider;
        }

        [CanBeNull]
        protected PartialArchRuleConjunction PartialArchRuleConjunction { get; }

        protected IObjectProvider<TRuleType> ObjectProvider { get; }

        public abstract string Description { get; }

        public override string ToString()
        {
            return Description;
        }

        // public override bool Equals(object obj)
        // {
        //     if (ReferenceEquals(null, obj))
        //     {
        //         return false;
        //     }
        //
        //     if (ReferenceEquals(this, obj))
        //     {
        //         return true;
        //     }
        //
        //     return obj.GetType() == GetType() && Equals((SyntaxElement<TRuleType>)obj);
        // }
        //
        // private bool Equals(SyntaxElement<TRuleType> other)
        // {
        //     return _ruleCreator.Equals(other._ruleCreator);
        // }
        //
        // public override int GetHashCode()
        // {
        //     return _ruleCreator != null ? _ruleCreator.GetHashCode() : 0;
        // }
    }
}
