using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class TypesShouldConjunctionWithDescription
        : ObjectsShouldConjunctionWithDescription<TypesShould, IType>
    {
        public TypesShouldConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider,
            IOrderedCondition<IType> condition
        )
            : base(partialArchRuleConjunction, objectProvider, condition) { }

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
