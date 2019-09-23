using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class RelationCondition<TRuleType, TReferenceType> : IHasFailDescription
        where TRuleType : ICanBeAnalyzed where TReferenceType : ICanBeAnalyzed
    {
        private readonly Func<TRuleType, TReferenceType, bool> _relationCondition;

        public RelationCondition(Func<TRuleType, TReferenceType, bool> relationCondition, string description,
            string failDescription)
        {
            _relationCondition = relationCondition;
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

        public bool Evaluate(TRuleType obj, TReferenceType referenceObj)
        {
            return _relationCondition(obj, referenceObj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}