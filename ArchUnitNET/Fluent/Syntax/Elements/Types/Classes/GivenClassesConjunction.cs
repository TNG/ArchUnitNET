using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClassesConjunction
        : GivenObjectsConjunction<
            GivenClasses,
            GivenClassesThat,
            ClassesShould,
            GivenClassesConjunctionWithDescription,
            Class
        >
    {
        internal GivenClassesConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider,
            IPredicate<Class> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenClasses As(string description) =>
            new GivenClasses(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<Class>(ObjectProvider, Predicate).WithDescription(
                    description
                )
            );

        public override GivenClassesConjunctionWithDescription Because(string reason) =>
            new GivenClassesConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

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
