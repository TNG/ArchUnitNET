using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public class ArchRuleWithoutRequiringPositiveResults<TRuleType> : IArchRule
        where TRuleType : ICanBeAnalyzed
    {
        internal ArchRuleWithoutRequiringPositiveResults(
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
            if (PartialArchRuleConjunction != null)
            {
                return PartialArchRuleConjunction.LogicalConjunction.Evaluate(
                    PartialArchRuleConjunction.LeftArchRule.HasNoViolations(architecture),
                    EvaluateCondition(architecture).All(result => result.Passed)
                );
            }
            return EvaluateCondition(architecture).All(result => result.Passed);
        }

        public IEnumerable<EvaluationResult> Evaluate(Architecture architecture)
        {
            if (PartialArchRuleConjunction != null)
            {
                return PartialArchRuleConjunction
                    .LeftArchRule.Evaluate(architecture)
                    .Concat(EvaluateCondition(architecture));
            }
            return EvaluateCondition(architecture);
        }

        private IEnumerable<EvaluationResult> EvaluateCondition(Architecture architecture)
        {
            return Condition
                .Check(ObjectProvider.GetObjects(architecture), architecture)
                .Select(conditionResult => new EvaluationResult(
                    conditionResult.AnalyzedObject,
                    new StringIdentifier(conditionResult.AnalyzedObject?.FullName ?? ""),
                    conditionResult.Pass,
                    conditionResult.AnalyzedObject != null
                        ? conditionResult.Pass
                            ? $"{conditionResult.AnalyzedObject.FullName} passed"
                            : $"{conditionResult.AnalyzedObject.FullName} {conditionResult.Description}"
                        : conditionResult.Description,
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

        public override string ToString()
        {
            return Description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return GetType() == obj.GetType()
                && Equals((ArchRuleWithoutRequiringPositiveResults<TRuleType>)obj);
        }

        private bool Equals(ArchRuleWithoutRequiringPositiveResults<TRuleType> other)
        {
            return Equals(PartialArchRuleConjunction, other.PartialArchRuleConjunction)
                && Equals(ObjectProvider, other.ObjectProvider)
                && Equals(Condition, other.Condition);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode =
                    PartialArchRuleConjunction != null
                        ? PartialArchRuleConjunction.GetHashCode()
                        : 0;
                hashCode =
                    (hashCode * 397) ^ (ObjectProvider != null ? ObjectProvider.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Condition != null ? Condition.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
