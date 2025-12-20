using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Syntax;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public abstract class ArchRule<TRuleType> : IArchRule
        where TRuleType : ICanBeAnalyzed
    {
        protected ArchRule(
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

        /// <summary>
        /// By default, rules are evaluated so positive results are required to be present.
        /// This call defeats this check on the rule.
        /// </summary>
        public ArchRuleWithoutRequirungPositiveResults<TRuleType> WithoutRequiringPositiveResults()
        {
            return new ArchRuleWithoutRequirungPositiveResults<TRuleType>(
                PartialArchRuleConjunction,
                ObjectProvider,
                Condition
            );
        }

        public bool HasNoViolations(Architecture architecture)
        {
            if (PartialArchRuleConjunction != null)
            {
                return PartialArchRuleConjunction.LogicalConjunction.Evaluate(
                    PartialArchRuleConjunction.LeftArchRule.HasNoViolations(architecture),
                    EvaluateCondition(architecture).All(result => result.Passed)
                );
            }
            return Evaluate(architecture).All(result => result.Passed);
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
            var objects = ObjectProvider.GetObjects(architecture);
            if (!(objects is ICollection<TRuleType> objectsCollection))
            {
                objectsCollection = objects.ToList();
            }
            var conditionResults = Condition.Check(objectsCollection, architecture).ToList();
            if (conditionResults.Count == 0)
            {
                yield return new EvaluationResult(
                    this,
                    new StringIdentifier(Description),
                    false,
                    "The rule requires positive evaluation, not just absence of violations. Use WithoutRequiringPositiveResults() or improve your rule's predicates.",
                    this,
                    architecture
                );
            }
            foreach (var conditionResult in conditionResults)
            {
                yield return new EvaluationResult(
                    conditionResult.AnalyzedObject,
                    new StringIdentifier(conditionResult.AnalyzedObject?.FullName ?? ""),
                    conditionResult.Pass,
                    conditionResult.AnalyzedObject != null
                        ? $"{conditionResult.AnalyzedObject.FullName} {conditionResult.Description}"
                        : conditionResult.Description,
                    this,
                    architecture
                );
            }
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

            return GetType() == obj.GetType() && Equals((ArchRule<TRuleType>)obj);
        }

        private bool Equals(ArchRule<TRuleType> other)
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
