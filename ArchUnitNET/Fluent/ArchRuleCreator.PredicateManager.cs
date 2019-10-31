//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Fluent.Predicates;
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
                var objects = _basicObjectProvider.GetObjects(architecture).ToList();
                IEnumerable<T> filteredObjects = new List<T>();
                return _predicateElements.Aggregate(filteredObjects,
                    (current, predicateElement) => predicateElement.CheckPredicate(current, objects, architecture));
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

                return obj.GetType() == GetType() && Equals((PredicateManager<T>) obj);
            }

            private bool Equals(PredicateManager<T> other)
            {
                return Equals(_basicObjectProvider, other._basicObjectProvider) &&
                       _predicateElements.SequenceEqual(other._predicateElements) &&
                       _hasCustomDescription == other._hasCustomDescription;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = _basicObjectProvider != null ? _basicObjectProvider.GetHashCode() : 0;
                    return _predicateElements.Aggregate(hashCode,
                        (current, predicateElement) =>
                            (current * 397) ^ (predicateElement != null ? predicateElement.GetHashCode() : 0));
                }
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

                public IEnumerable<T> CheckPredicate(IEnumerable<T> currentObjects, IEnumerable<T> allObjects,
                    Architecture architecture)
                {
                    if (_predicate == null)
                    {
                        throw new InvalidOperationException(
                            "Can't check a PredicateElement before the predicate is set.");
                    }

                    return _logicalConjunction.Evaluate(currentObjects,
                        _predicate.GetMatchingObjects(allObjects, architecture));
                }

                public override string ToString()
                {
                    return Description;
                }

                private bool Equals(PredicateElement<T> other)
                {
                    return Equals(_logicalConjunction, other._logicalConjunction) &&
                           _customDescription == other._customDescription &&
                           Equals(_predicate, other._predicate) &&
                           _reason == other._reason;
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

                    return obj.GetType() == GetType() && Equals((PredicateElement<T>) obj);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        var hashCode = _logicalConjunction != null ? _logicalConjunction.GetHashCode() : 0;
                        hashCode = (hashCode * 397) ^
                                   (_customDescription != null ? _customDescription.GetHashCode() : 0);
                        hashCode = (hashCode * 397) ^ (_predicate != null ? _predicate.GetHashCode() : 0);
                        hashCode = (hashCode * 397) ^ (_reason != null ? _reason.GetHashCode() : 0);
                        return hashCode;
                    }
                }
            }
        }
    }
}