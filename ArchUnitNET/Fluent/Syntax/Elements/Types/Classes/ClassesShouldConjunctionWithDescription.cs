using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<ClassesShould, Class>
    {
        internal ClassesShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider,
            IOrderedCondition<Class> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override ClassesShould AndShould() =>
            new ClassesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override ClassesShould OrShould() =>
            new ClassesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
