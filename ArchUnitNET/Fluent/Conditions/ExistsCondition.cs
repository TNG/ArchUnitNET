using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Conditions
{
    public class ExistsCondition<TRuleType> : IOrderedCondition<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        private readonly bool _valueIfExists;

        public ExistsCondition(bool valueIfExists)
        {
            _valueIfExists = valueIfExists;
        }

        public string Description => _valueIfExists ? "exist" : "not exist";

        public IEnumerable<IConditionResult> Check(
            IEnumerable<TRuleType> objects,
            Architecture architecture
        )
        {
            if (!(objects is ICollection<TRuleType> objectCollection))
            {
                objectCollection = objects.ToList();
            }
            if (!objectCollection.Any())
            {
                return new[]
                {
                    new ConditionResult(
                        null,
                        !_valueIfExists,
                        "There are no objects matching the criteria"
                    ),
                };
            }
            return objectCollection.Select(obj => new ConditionResult(
                obj,
                _valueIfExists,
                "does exist"
            ));
        }

        public bool CheckEmpty()
        {
            return !_valueIfExists;
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(ExistsCondition<TRuleType> other)
        {
            return _valueIfExists == other._valueIfExists;
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

            return obj.GetType() == GetType() && Equals((ExistsCondition<TRuleType>)obj);
        }

        public override int GetHashCode()
        {
            return _valueIfExists.GetHashCode();
        }
    }
}
