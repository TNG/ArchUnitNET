using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class RelationCondition<T, TReference> : IHasFailDescription
        where T : ICanBeAnalyzed where TReference : ICanBeAnalyzed
    {
        private readonly Func<T, TReference, bool> _relationCondition;

        public RelationCondition(Func<T, TReference, bool> relationCondition, string description,
            string failDescription)
        {
            _relationCondition = relationCondition;
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

        public bool Evaluate(T obj, TReference referenceObj)
        {
            return _relationCondition(obj, referenceObj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}