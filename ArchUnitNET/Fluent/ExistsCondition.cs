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
        public string FailDescription => _valueIfExists ? "does not exist" : "does exist";

        public bool Check(TRuleType obj, Architecture architecture)
        {
            return _valueIfExists;
        }

        public bool CheckNull()
        {
            return !_valueIfExists;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}