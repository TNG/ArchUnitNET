using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsConjunction<
        TGivenRuleType,
        TGivenRuleTypeThat,
        TRuleTypeShould,
        TGivenRuleTypeConjunctionWithReason,
        TRuleType
    > : GivenObjectsConjunctionWithDescription<TGivenRuleTypeThat, TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider,
            IPredicate<TRuleType> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public abstract TGivenRuleType As(string description);
        public abstract TGivenRuleTypeConjunctionWithReason Because(string reason);
    }
}
