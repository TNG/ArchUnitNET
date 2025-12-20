using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class TypesShouldConjunction
        : ObjectsShouldConjunction<TypesShould, TypesShouldConjunctionWithDescription, IType>
    {
        public TypesShouldConjunction(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider,
            IOrderedCondition<IType> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

        public override TypesShouldConjunctionWithDescription As(string description) =>
            new TypesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.As(description)
            );

        public override TypesShouldConjunctionWithDescription Because(string reason) =>
            new TypesShouldConjunctionWithDescription(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition.Because(reason)
            );

        public override TypesShould AndShould() =>
            new TypesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.And
            );

        public override TypesShould OrShould() =>
            new TypesShould(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition,
                LogicalConjunctionDefinition.Or
            );
    }
}
