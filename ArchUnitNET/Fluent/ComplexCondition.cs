using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ComplexCondition<TRuleType, TReferenceType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed where TReferenceType : ICanBeAnalyzed
    {
        private readonly IObjectFilter<TReferenceType> _objectFilter;
        private readonly RelationCondition<TRuleType, TReferenceType> _relation;

        public ComplexCondition(RelationCondition<TRuleType, TReferenceType> relation,
            IObjectFilter<TReferenceType> objectFilter)
        {
            _relation = relation;
            _objectFilter = objectFilter;
        }

        public string Description => _relation.Description + " " + _objectFilter.Description;

        public string FailDescription => _relation.FailDescription + " " + _objectFilter.Description;

        public bool Check(TRuleType obj, Architecture architecture)
        {
            return _relation.CheckRelation(obj, _objectFilter, architecture);
        }

        public bool CheckNull()
        {
            return true;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}