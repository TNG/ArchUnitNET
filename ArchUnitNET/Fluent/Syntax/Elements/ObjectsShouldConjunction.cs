using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShouldConjunction<
        TRuleTypeShould,
        TRuleTypeShouldConjunctionWithReason,
        TRuleType
    > : ObjectsShouldConjunctionWithDescription<TRuleTypeShould, TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunction(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider,
            IOrderedCondition<TRuleType> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public abstract TRuleTypeShouldConjunctionWithReason Because(string reason);
        public abstract TRuleTypeShouldConjunctionWithReason As(string description);
    }
}
