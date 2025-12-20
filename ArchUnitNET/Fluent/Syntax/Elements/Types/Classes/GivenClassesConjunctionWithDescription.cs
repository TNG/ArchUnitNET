using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenClassesThat, ClassesShould, Class>
    {
        internal GivenClassesConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider,
            IPredicate<Class> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenClassesThat And() =>
            new GivenClassesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenClassesThat Or() =>
            new GivenClassesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override ClassesShould Should() =>
            new ClassesShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<Class>(ObjectProvider, Predicate).WithDescriptionSuffix(
                    "should"
                )
            );
    }
}
