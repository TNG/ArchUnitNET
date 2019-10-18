using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class ExistsCondition<TRuleType> : ICondition<TRuleType> where TRuleType : ICanBeAnalyzed
    {
        private readonly bool _valueIfExists;

        public ExistsCondition(bool valueIfExists)
        {
            _valueIfExists = valueIfExists;
        }

        public string Description => _valueIfExists ? "exist" : "not exist";

        public ConditionResult Check(TRuleType obj, Architecture architecture)
        {
            return new ConditionResult(_valueIfExists, "does exist");
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

            return obj.GetType() == GetType() && Equals((ExistsCondition<TRuleType>) obj);
        }

        public override int GetHashCode()
        {
            return _valueIfExists.GetHashCode();
        }
    }
}