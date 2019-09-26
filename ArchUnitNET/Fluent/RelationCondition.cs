using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.EnumerableOperator;

namespace ArchUnitNET.Fluent
{
    public class RelationCondition<TRuleType, TRelatedType> : IHasFailDescription
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

        public string Description { get; }
        public string FailDescription { get; }

        public bool CheckRelation(TRuleType obj, IObjectFilter<TRelatedType> filter, Architecture architecture)
        {
            switch (_shouldBeTrueFor)
            {
                case All:
                    return _relation(obj).All(o => filter.CheckFilter(o, architecture));
                case Any:
                    return _relation(obj).Any(o => filter.CheckFilter(o, architecture));
                case None:
                    return _relation(obj).All(o => !filter.CheckFilter(o, architecture));
                default:
                    throw new IndexOutOfRangeException("The ShouldBeTrueFor Operator does not have a valid value.");
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