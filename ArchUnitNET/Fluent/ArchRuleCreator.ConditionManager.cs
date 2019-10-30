//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

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

            public string Description => _conditionElements
                .Aggregate("", (current, conditionElement) => current + " " + conditionElement.Description).Trim();

            public void BeginComplexCondition<TReferenceType>(RelationCondition<T, TReferenceType> relationCondition)
                where TReferenceType : ICanBeAnalyzed
            {
                _relationConditionTemp = relationCondition;
                _referenceTypeTemp = typeof(TReferenceType);
            }

            public void ContinueComplexCondition<TReferenceType>(IPredicate<TReferenceType> filter)
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

            public void SetCustomDescription(string description)
            {
                _conditionElements.ForEach(conditionElement => conditionElement.SetCustomDescription(""));
                _conditionElements.Last().SetCustomDescription(description);
            }

            public void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _conditionElements.Add(new ConditionElement<T>(logicalConjunction));
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
                    yield return new EvaluationResult(null, CheckEmpty(), "There are no objects matching the criteria",
                        archRuleCreator, architecture);
                    yield break;
                }

                var conditionResults = _conditionElements.Select(conditionElement =>
                    conditionElement.Check(filteredObjectsList, architecture).ToList()).ToList();

                for (var i = 0; i < filteredObjectsList.Count; i++)
                {
                    yield return CreateEvaluationResult(conditionResults.Select(results => results[i]), architecture,
                        archRuleCreator);
                }
            }

            private static EvaluationResult CreateEvaluationResult(
                IEnumerable<ConditionElementResult> conditionElementResults,
                Architecture architecture, ICanBeEvaluated archRuleCreator)
            {
                var conditionElementResultsList = conditionElementResults.ToList();
                var analyzedObject = conditionElementResultsList.First().ConditionResult.AnalyzedObject;
                var passRule = conditionElementResultsList.Aggregate(true,
                    (currentResult, conditionElementResult) =>
                        conditionElementResult.LogicalConjunction.Evaluate(currentResult,
                            conditionElementResult.ConditionResult.Pass));
                var description = analyzedObject.FullName;
                if (passRule)
                {
                    description += " passed";
                }
                else
                {
                    var first = true;
                    var failDescriptionCache =
                        new List<string>(); //Prevent failDescriptions like "... failed because ... is public and is public"
                    foreach (var conditionResult in conditionElementResultsList.Select(result => result.ConditionResult)
                        .Where(condResult =>
                            !condResult.Pass && !failDescriptionCache.Contains(condResult.FailDescription)))
                    {
                        if (!first)
                        {
                            description += " and";
                        }

                        description += " " + conditionResult.FailDescription;
                        failDescriptionCache.Add(conditionResult.FailDescription);
                        first = false;
                    }
                }

                return new EvaluationResult(analyzedObject, passRule, description, archRuleCreator, architecture);
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

                return obj.GetType() == GetType() && Equals((ConditionManager<T>) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return _conditionElements.Aggregate(397,
                        (current, conditionElement) =>
                            (current * 397) ^ (conditionElement != null ? conditionElement.GetHashCode() : 0));
                }
            }

            private bool Equals(ConditionManager<T> other)
            {
                return _conditionElements.SequenceEqual(other._conditionElements) &&
                       _referenceTypeTemp == other._referenceTypeTemp &&
                       _relationConditionTemp == other._relationConditionTemp;
            }

#pragma warning disable 693
            private class ConditionElement<T> : IHasDescription where T : ICanBeAnalyzed
            {
                private readonly LogicalConjunction _logicalConjunction;
                private ICondition<T> _condition;
                [CanBeNull] private string _customDescription;
                private string _reason;

                public ConditionElement(LogicalConjunction logicalConjunction)
                {
                    _condition = null;
                    _logicalConjunction = logicalConjunction;
                    _reason = "";
                }

                public string Description => _customDescription ?? (_condition == null
                                                 ? ""
                                                 : (_logicalConjunction.Description + " should " +
                                                    _condition.GetShortDescription() + " " + _reason).Trim());

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

                public void SetCustomDescription(string description)
                {
                    _customDescription = description;
                }

                public IEnumerable<ConditionElementResult> Check(IEnumerable<T> objects, Architecture architecture)
                {
                    if (_condition == null)
                    {
                        throw new InvalidOperationException(
                            "Can't check a ConditionElement before the condition is set.");
                    }

                    return _condition.Check(objects, architecture)
                        .Select(result => new ConditionElementResult(result, _logicalConjunction));
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

                    return obj.GetType() == GetType() && Equals((ConditionElement<T>) obj);
                }

                private bool Equals(ConditionElement<T> other)
                {
                    return Equals(_logicalConjunction, other._logicalConjunction) &&
                           Equals(_condition, other._condition) &&
                           _customDescription == other._customDescription &&
                           _reason == other._reason;
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        var hashCode = _logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0;
                        hashCode = (hashCode * 397) ^ (_condition != null ? _condition.GetHashCode() : 0);
                        hashCode = (hashCode * 397) ^
                                   (_customDescription != null ? _customDescription.GetHashCode() : 0);
                        hashCode = (hashCode * 397) ^ (_reason != null ? _reason.GetHashCode() : 0);
                        return hashCode;
                    }
                }
            }

            private class ConditionElementResult
            {
                public readonly ConditionResult ConditionResult;
                public readonly LogicalConjunction LogicalConjunction;

                public ConditionElementResult(ConditionResult conditionResult, LogicalConjunction logicalConjunction)
                {
                    ConditionResult = conditionResult;
                    LogicalConjunction = logicalConjunction;
                }
            }
        }
    }
}