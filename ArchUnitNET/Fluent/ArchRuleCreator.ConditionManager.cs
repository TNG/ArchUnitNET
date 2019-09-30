using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public partial class ArchRuleCreator<TRuleType>
    {
        private class ConditionManager<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private readonly List<ConditionElement<T>> _conditionElements;
            private Type _referenceTypeTemp;
            private object _relationConditionTemp;

            public ConditionManager()
            {
                _conditionElements = new List<ConditionElement<T>>
                {
                    new ConditionElement<T>(LogicalConjunctionDefinition.ForwardSecondValue)
                };
            }

            public string Description => _conditionElements.Aggregate("",
                (current, conditionElement) => current + " " + conditionElement.Description).Trim();

            public void BeginComplexCondition<TReferenceType>(RelationCondition<T, TReferenceType> relationCondition)
                where TReferenceType : ICanBeAnalyzed
            {
                _relationConditionTemp = relationCondition;
                _referenceTypeTemp = typeof(TReferenceType);
            }

            public void ContinueComplexCondition<TReferenceType>(IObjectFilter<TReferenceType> filter)
                where TReferenceType : ICanBeAnalyzed
            {
                if (typeof(TReferenceType) == _referenceTypeTemp)
                {
                    AddCondition(
                        new ComplexCondition<T, TReferenceType>(
                            (RelationCondition<T, TReferenceType>) _relationConditionTemp, filter));
                }
                else
                {
                    throw new InvalidCastException(
                        "ContinueComplexCondition() has to be called with the same generic type argument that was used for BeginComplexCondition().");
                }
            }

            public void AddCondition(ICondition<T> condition)
            {
                _conditionElements.Last().SetCondition(condition);
            }

            public void AddReason(string reason)
            {
                _conditionElements.Last().AddReason(reason);
            }

            public void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _conditionElements.Add(new ConditionElement<T>(logicalConjunction));
            }

            public bool CheckConditions(IEnumerable<T> filteredObjects, Architecture architecture)
            {
                var filteredObjectsList = filteredObjects.ToList();
                return filteredObjectsList.IsNullOrEmpty()
                    ? CheckEmpty()
                    : filteredObjectsList.All(obj => CheckConditions(obj, architecture));
            }

            private bool CheckConditions(T obj, Architecture architecture)
            {
                return _conditionElements.Aggregate(true,
                    (currentResult, conditionElement) => conditionElement.Check(currentResult, obj, architecture));
            }

            private bool CheckEmpty()
            {
                return _conditionElements.Aggregate(true,
                    (currentResult, conditionElement) => conditionElement.CheckEmpty(currentResult));
            }

            public IEnumerable<EvaluationResult> EvaluateConditions(IEnumerable<T> filteredObjects,
                Architecture architecture, ICanBeEvaluated archRuleCreator)
            {
                var filteredObjectsList = filteredObjects.ToList();
                if (filteredObjectsList.IsNullOrEmpty())
                {
                    return new List<EvaluationResult>
                    {
                        new EvaluationResult(null, CheckEmpty(), "There are no objects matching the criteria",
                            archRuleCreator, architecture)
                    };
                }

                return filteredObjectsList.Select(obj => EvaluateConditions(obj, architecture, archRuleCreator));
            }

            private EvaluationResult EvaluateConditions(T obj, Architecture architecture,
                ICanBeEvaluated archRuleCreator)
            {
                var passRule = CheckConditions(obj, architecture);
                var description = obj.FullName + " ";
                if (passRule)
                {
                    description += "passed";
                }
                else
                {
                    var first = true;
                    foreach (var conditionElement in _conditionElements.Where(conditionElement =>
                        !conditionElement.Check(obj, architecture)))
                    {
                        if (first)
                        {
                            description += conditionElement.FailDescription;
                            first = false;
                        }
                        else
                        {
                            description += " and " + conditionElement.FailDescription;
                        }
                    }
                }

                return new EvaluationResult(obj, passRule, description, archRuleCreator, architecture);
            }

            public override string ToString()
            {
                return Description;
            }

#pragma warning disable 693
        private class ConditionElement<T> : IHasFailDescription where T : ICanBeAnalyzed
        {
            private readonly LogicalConjunction _logicalConjunction;
            private ICondition<T> _condition;
            private string _reason;

            public ConditionElement(LogicalConjunction logicalConjunction)
            {
                _condition = null;
                _logicalConjunction = logicalConjunction;
                _reason = "";
            }

            public string Description => _condition == null
                ? ""
                : (_logicalConjunction.Description + " should " + _condition.Description + " " + _reason).Trim();

            public string FailDescription => _condition.FailDescription;

            public void AddReason(string reason)
            {
                if (_condition == null)
                {
                    throw new InvalidOperationException(
                        "Can't add a reason to a ConditionElement before the condition is set.");
                }

                if (_reason != "")
                {
                    throw new InvalidOperationException(
                        "Can't add a reason to a ConditionElement which already has a reason.");
                }

                _reason = "because " + reason;
            }

            public void SetCondition(ICondition<T> condition)
            {
                _condition = condition;
            }

            public bool Check(bool currentResult, T obj, Architecture architecture)
            {
                return _logicalConjunction.Evaluate(currentResult, Check(obj, architecture));
            }

            public bool Check(T obj, Architecture architecture)
            {
                if (_condition == null)
                {
                    throw new InvalidOperationException(
                        "Can't check a ConditionElement before the condition is set.");
                }

                return _condition.Check(obj, architecture);
            }

            public bool CheckEmpty(bool currentResult)
            {
                if (_condition == null)
                {
                    throw new InvalidOperationException(
                        "Can't check a ConditionElement before the condition is set.");
                }

                return _logicalConjunction.Evaluate(currentResult, _condition.CheckEmpty());
            }

            public override string ToString()
            {
                return Description;
            }
        }
    }
    }
}