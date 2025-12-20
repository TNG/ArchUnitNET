using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class ObjectsShouldConjunctionWithDescription<TRuleTypeShould, TRuleType>
        : ArchRule<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected ObjectsShouldConjunctionWithDescription(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider,
            IOrderedCondition<TRuleType> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public abstract TRuleTypeShould AndShould();
        public abstract TRuleTypeShould OrShould();
    }
}
