using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldConjunction
        : ObjectsShouldConjunction<ClassesShould, ClassesShouldConjunctionWithDescription, Class>
    {
        public ClassesShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider,
            IOrderedCondition<Class> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override ClassesShouldConjunctionWithDescription As(string description) =>
            new ClassesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override ClassesShouldConjunctionWithDescription Because(string reason) =>
            new ClassesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

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
