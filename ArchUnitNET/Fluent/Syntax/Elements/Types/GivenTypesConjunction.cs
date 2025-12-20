using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class GivenTypesConjunction
        : GivenObjectsConjunction<
            GivenTypes,
            GivenTypesThat,
            TypesShould,
            GivenTypesConjunctionWithDescription,
            IType
        >
    {
        internal GivenTypesConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider,
            IPredicate<IType> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

        public override GivenTypes As(string description) =>
            new GivenTypes(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<IType>(ObjectProvider, Predicate).WithDescription(
                    description
                )
            );

        public override GivenTypesConjunctionWithDescription Because(string reason) =>
            new GivenTypesConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate.Because(reason)
            );

        public override GivenTypesThat And() =>
            new GivenTypesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.And
            );

        public override GivenTypesThat Or() =>
            new GivenTypesThat(
                PartialArchRuleConjunction,
                ObjectProvider,
                Predicate,
                LogicalConjunctionDefinition.Or
            );

        public override TypesShould Should() =>
            new TypesShould(
                PartialArchRuleConjunction,
                new PredicateObjectProvider<IType>(ObjectProvider, Predicate).WithDescriptionSuffix(
                    "should"
                )
            );
    }
}
