using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ComplexCondition<T, TReference> : ICondition<T>
        where T : ICanBeAnalyzed where TReference : ICanBeAnalyzed
    {
        private readonly ObjectFilter<TReference> _objectFilter;
        private readonly ObjectProvider<TReference> _objectProvider;
        private readonly RelationCondition<T, TReference> _relationCondition;

        public ComplexCondition(ObjectProvider<TReference> objectProvider,
            RelationCondition<T, TReference> relationCondition, ObjectFilter<TReference> objectFilter)
        {
            _objectProvider = objectProvider;
            _relationCondition = relationCondition;
            _objectFilter = objectFilter;
        }

        public string Description => _relationCondition.Description + " " + _objectFilter.Description;

        public string FailDescription =>
            _relationCondition.FailDescription + " " + _objectFilter.Description;

        public bool Check(T obj, Architecture architecture)
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