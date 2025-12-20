using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class GivenTypesConjunctionWithDescription
        : GivenObjectsConjunctionWithDescription<GivenTypesThat, TypesShould, IType>
    {
        internal GivenTypesConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider,
            IPredicate<IType> predicate
        )
            : base(partialArchRuleConjunction, objectProvider, predicate) { }

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
