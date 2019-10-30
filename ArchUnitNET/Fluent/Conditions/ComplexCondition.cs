//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ComplexCondition<TRuleType, TReferenceType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed where TReferenceType : ICanBeAnalyzed
    {
        private readonly IPredicate<TReferenceType> _predicate;
        private readonly RelationCondition<TRuleType, TReferenceType> _relation;

        public ComplexCondition(RelationCondition<TRuleType, TReferenceType> relation,
            IPredicate<TReferenceType> predicate)
        {
            _relation = relation;
            _predicate = predicate;
        }

        public string Description => _relation.Description + " " + _predicate.Description;

        public IEnumerable<ConditionResult> Check(IEnumerable<TRuleType> objects, Architecture architecture)
        {
            return _relation.CheckRelation(objects, _predicate, architecture);
        }

        public bool CheckEmpty()
        {
            return true;
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ComplexCondition<TRuleType, TReferenceType> other)
        {
            return Equals(_predicate, other._predicate) && Equals(_relation, other._relation);
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

            return obj.GetType() == GetType() && Equals((ComplexCondition<TRuleType, TReferenceType>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_predicate != null ? _predicate.GetHashCode() : 0) * 397) ^
                       (_relation != null ? _relation.GetHashCode() : 0);
            }
        }
    }
}