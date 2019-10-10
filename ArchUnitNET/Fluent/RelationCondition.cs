using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using static ArchUnitNET.Fluent.EnumerableOperator;

namespace ArchUnitNET.Fluent
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

        public bool CheckRelation(TRuleType obj, IPredicate<TRelatedType> filter, Architecture architecture)
        {
            switch (_shouldBeTrueFor)
            {
                case All:
                    return _relation(obj).All(o => filter.CheckPredicate(o, architecture));
                case Any:
                    return _relation(obj).Any(o => filter.CheckPredicate(o, architecture));
                case None:
                    return _relation(obj).All(o => !filter.CheckPredicate(o, architecture));
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