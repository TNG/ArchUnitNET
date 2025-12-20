using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShould : AddClassCondition<ClassesShouldConjunction>
    {
        private readonly IOrderedCondition<Class> _leftCondition;
        private readonly LogicalConjunction _logicalConjunction;

        public ClassesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider
        )
            : this(partialArchRuleConjunction, objectProvider, null, null) { }

        public ClassesShould(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider,
            IOrderedCondition<Class> leftCondition,
            LogicalConjunction logicalConjunction
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            _leftCondition = leftCondition;
            _logicalConjunction = logicalConjunction;
        }

        internal override ClassesShouldConjunction CreateNextElement(
            IOrderedCondition<Class> condition
        ) =>
            new ClassesShouldConjunction(
                PartialArchRuleConjunction,
                ObjectProvider,
                _leftCondition == null
                    ? condition
                    : new CombinedCondition<Class>(_leftCondition, _logicalConjunction, condition)
            );
    }
}
