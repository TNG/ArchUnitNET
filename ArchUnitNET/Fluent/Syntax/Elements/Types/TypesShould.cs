using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class TypesShould : AddTypeCondition<TypesShouldConjunction, IType>
    {
        private readonly IOrderedCondition<IType> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public TypesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public TypesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider,
            IOrderedCondition<IType> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override TypesShouldConjunction CreateNextElement(
            IOrderedCondition<IType> condition
        )
        {
            return new TypesShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<IType>(_leftCondition, _logicalConjunction, condition)
            );
        }
    }
}
