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
            return Equals(PartialArchRuleConjunction, other.PartialArchRuleConjunction)
                && Equals(ObjectProvider, other.ObjectProvider);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode =
                    PartialArchRuleConjunction != null
                        ? PartialArchRuleConjunction.GetHashCode()
                        : 0;
                hashCode = (hashCode * 397) ^ (ObjectProvider != null ? ObjectProvider.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
