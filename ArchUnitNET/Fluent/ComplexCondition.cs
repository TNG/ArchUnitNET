using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ComplexCondition<TRuleType, TReferenceType> : ICondition<TRuleType>
        where TRuleType : ICanBeAnalyzed where TReferenceType : ICanBeAnalyzed
    {
        private readonly ObjectFilter<TReferenceType> _objectFilter;
        private readonly ObjectProvider<TReferenceType> _objectProvider;
        private readonly RelationCondition<TRuleType, TReferenceType> _relationCondition;

        public ComplexCondition(ObjectProvider<TReferenceType> objectProvider,
            RelationCondition<TRuleType, TReferenceType> relationCondition, ObjectFilter<TReferenceType> objectFilter)
        {
            _objectProvider = objectProvider;
            _relationCondition = relationCondition;
            _objectFilter = objectFilter;
        }

        public string Description => _relationCondition.Description + " " + _objectFilter.Description;

        public string FailDescription =>
            _relationCondition.FailDescription + " " + _objectFilter.Description;

        public bool Check(TRuleType obj, Architecture architecture)
        {
            return _objectProvider.GetObjects(architecture)
                .Where(refObj => _relationCondition.Evaluate(obj, refObj))
                .All(relatedObj => _objectFilter.CheckFilter(relatedObj));
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