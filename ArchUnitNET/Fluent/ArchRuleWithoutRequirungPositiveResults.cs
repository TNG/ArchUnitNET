using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public class ArchRuleWithoutRequirungPositiveResults<TRuleType> : IArchRule
        where TRuleType : ICanBeAnalyzed
    {
        internal ArchRuleWithoutRequirungPositiveResults(
            PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider,
            IOrderedCondition<TRuleType> condition
        )
        {
            PartialArchRuleConjunction = partialArchRuleConjunction;
            ObjectProvider = objectProvider;
            Condition = condition;
        }

        [CanBeNull]
        protected PartialArchRuleConjunction PartialArchRuleConjunction { get; }

        protected IObjectProvider<TRuleType> ObjectProvider { get; }

        protected IOrderedCondition<TRuleType> Condition { get; }

        public string Description =>
            PartialArchRuleConjunction != null
                ? $"{PartialArchRuleConjunction.LeftArchRule.Description} {PartialArchRuleConjunction.LogicalConjunction.Description} {ObjectProvider.Description} {Condition.Description}"
                : $"{ObjectProvider.Description} {Condition.Description}";

        public bool HasNoViolations(Architecture architecture)
        {
            return Evaluate(architecture).All(result => result.Passed);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            return Condition
                .Check(ObjectProvider.GetObjects(architecture), architecture)
                .Select(conditionResult => new EvaluationResult(
                    conditionResult.AnalyzedObject,
                    new StringIdentifier(conditionResult.AnalyzedObject.FullName),
                    conditionResult.Pass,
                    conditionResult.Pass
                        ? $"{conditionResult.AnalyzedObject.FullName} passed"
                        : $"{conditionResult.AnalyzedObject.FullName} {conditionResult.Description}",
                    this,
                    architecture
                ));
        }

        public CombinedArchRuleDefinition And()
        {
            return new CombinedArchRuleDefinition(this, LogicalConjunctionDefinition.And);
        }

        public CombinedArchRuleDefinition Or()
        {
            return new CombinedArchRuleDefinition(this, LogicalConjunctionDefinition.Or);
        }

        public IArchRule And(IArchRule archRule)
        {
            return new CombinedArchRule(this, LogicalConjunctionDefinition.And, archRule);
        }

        public IArchRule Or(IArchRule archRule)
        {
            return new CombinedArchRule(this, LogicalConjunctionDefinition.Or, archRule);
        }
    }
}
