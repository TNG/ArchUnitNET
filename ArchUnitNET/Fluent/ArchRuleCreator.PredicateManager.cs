using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Syntax;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent
{
    public partial class ArchRuleCreator<TRuleType>
    {
        private class PredicateManager<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private const string NotSet = "not set";
            private readonly BasicObjectProvider<T> _basicObjectProvider;
            private readonly List<PredicateElement<T>> _predicateElements;
            private bool _hasCustomDescription;

            public PredicateManager(BasicObjectProvider<T> basicObjectProvider)
            {
                _basicObjectProvider = basicObjectProvider;
                _predicateElements = new List<PredicateElement<T>>
                {
                    new PredicateElement<T>(LogicalConjunctionDefinition.ForwardSecondValue,
                        new SimplePredicate<T>(t => true, NotSet))
                };
                _hasCustomDescription = false;
            }

            public string Description => _hasCustomDescription
                ? _predicateElements.Aggregate("",
                    (current, objectFilterElement) => current + " " + objectFilterElement.Description)
                : _predicateElements.First().Description == NotSet
                    ? _basicObjectProvider.Description
                    : _basicObjectProvider.Description + " that" + _predicateElements.Aggregate("",
                          (current, objectFilterElement) => current + " " + objectFilterElement.Description);

            public IEnumerable<T> GetObjects(Architecture architecture)
            {
                return _basicObjectProvider.GetObjects(architecture).Where(obj => _predicateElements.Aggregate(true,
                    (currentResult, objectFilterElement) =>
                        objectFilterElement.CheckPredicate(currentResult, obj, architecture)));
            }

            public void AddPredicate(IPredicate<T> predicate)
            {
                _predicateElements.Last().SetPredicate(predicate);
            }

            public void AddReason(string reason)
            {
                _predicateElements.Last().AddReason(reason);
            }

            public void SetCustomDescription(string description)
            {
                _hasCustomDescription = true;
                _predicateElements.ForEach(predicateElement => predicateElement.SetCustomDescription(""));
                _predicateElements.Last().SetCustomDescription(description);
            }

            public void SetNextLogicalConjunction(LogicalConjunction logicalConjunction)
            {
                _predicateElements.Add(new PredicateElement<T>(logicalConjunction));
            }

            public override string ToString()
            {
                return Description;
            }

#pragma warning disable 693
        private class PredicateElement<T> : IHasDescription where T : ICanBeAnalyzed
        {
            private readonly LogicalConjunction _logicalConjunction;
            [CanBeNull] private string _customDescription;
            private IPredicate<T> _predicate;
            private string _reason;

            public PredicateElement(LogicalConjunction logicalConjunction, IPredicate<T> predicate = null)
            {
                _predicate = predicate;
                _logicalConjunction = logicalConjunction;
                _reason = "";
            }

            public string Description => _customDescription ?? (_predicate == null
                                             ? _logicalConjunction.Description
                                             : (_logicalConjunction.Description + " " +
                                                _predicate.GetShortDescription() + " " + _reason).Trim());

            public void AddReason(string reason)
            {
                if (_predicate == null)
                {
                    throw new InvalidOperationException(
                        "Can't add a reason to a PredicateElement before the predicate is set.");
                }

                if (_reason != "")
                {
                    throw new InvalidOperationException(
                        "Can't add a reason to a PredicateElement which already has a reason.");
                }

                _reason = "because " + reason;
            }

            public void SetCustomDescription(string description)
            {
                _customDescription = description;
            }

            public void SetPredicate(IPredicate<T> predicate)
            {
                _predicate = predicate;
            }

            public bool CheckPredicate(bool currentResult, T obj, Architecture architecture)
            {
                if (_predicate == null)
                {
                    throw new InvalidOperationException(
                        "Can't Evaluate an PredicateElement before the predicate is set.");
                }

                return _logicalConjunction.Evaluate(currentResult, _predicate.CheckPredicate(obj, architecture));
            }

            public override string ToString()
            {
                return Description;
            }
        }
    }
    }
}