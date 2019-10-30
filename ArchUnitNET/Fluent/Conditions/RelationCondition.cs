//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;
using static ArchUnitNET.Fluent.Conditions.EnumerableOperator;

namespace ArchUnitNET.Fluent.Conditions
{
    public class RelationCondition<TRuleType, TRelatedType> : IHasDescription
        where TRuleType : ICanBeAnalyzed where TRelatedType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, IEnumerable<TRelatedType>> _relation;
        private readonly EnumerableOperator _shouldBeTrueFor;

        public RelationCondition(Func<TRuleType, IEnumerable<TRelatedType>> relation,
            EnumerableOperator shouldBeTrueFor,
            string description,
            string failDescription)
        {
            _relation = relation;
            _shouldBeTrueFor = shouldBeTrueFor;
            Description = description;
            FailDescription = failDescription;
        }

        public string FailDescription { get; }

        public string Description { get; }

        public IEnumerable<ConditionResult>
            CheckRelation(IEnumerable<TRuleType> objects, IPredicate<TRelatedType> predicate, Architecture architecture)
        {
            var objectList = objects.ToList();
            var failDescription = FailDescription + " " + predicate.Description;
            switch (_shouldBeTrueFor)
            {
                case All:
                    var failedObjects1 = objectList.Where(o =>
                        _relation(o).Except(predicate.GetMatchingObjects(_relation(o), architecture)).Any()).ToList();
                    foreach (var failedObject in failedObjects1)
                    {
                        yield return new ConditionResult(failedObject, false, failDescription);
                    }

                    foreach (var passedObject in objectList.Except(failedObjects1))
                    {
                        yield return new ConditionResult(passedObject, true);
                    }

                    yield break;
                case Any:
                    var failedObjects2 = objectList
                        .Where(o => !predicate.GetMatchingObjects(_relation(o), architecture).Any()).ToList();
                    foreach (var failedObject in failedObjects2)
                    {
                        yield return new ConditionResult(failedObject, false, failDescription);
                    }

                    foreach (var passedObject in objectList.Except(failedObjects2))
                    {
                        yield return new ConditionResult(passedObject, true);
                    }

                    yield break;
                case None:
                    var failedObjects3 = objectList
                        .Where(o => predicate.GetMatchingObjects(_relation(o), architecture).Any()).ToList();
                    foreach (var failedObject in failedObjects3)
                    {
                        yield return new ConditionResult(failedObject, false, failDescription);
                    }

                    foreach (var passedObject in objectList.Except(failedObjects3))
                    {
                        yield return new ConditionResult(passedObject, true);
                    }

                    yield break;
                default:
                    throw new IndexOutOfRangeException("The ShouldBeTrueFor Operator does not have a valid value.");
            }
        }

        private bool Equals(RelationCondition<TRuleType, TRelatedType> other)
        {
            return _shouldBeTrueFor == other._shouldBeTrueFor &&
                   FailDescription == other.FailDescription &&
                   Description == other.Description;
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

            return obj.GetType() == GetType() && Equals((RelationCondition<TRuleType, TRelatedType>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) _shouldBeTrueFor;
                hashCode = (hashCode * 397) ^ (FailDescription != null ? FailDescription.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public enum EnumerableOperator
    {
        All,
        Any,
        None
    }
}