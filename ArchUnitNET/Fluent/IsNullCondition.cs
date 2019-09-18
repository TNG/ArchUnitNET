using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class IsNullCondition<T> : ICondition<T> where T : ICanBeAnalyzed
    {
        private readonly bool _valueIfNull;

        public IsNullCondition(bool valueIfNull, string description, string failDescription)
        {
            _valueIfNull = valueIfNull;
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

        public bool Check(T obj, Architecture architecture)
        {
            return !_valueIfNull;
        }

        public bool CheckNull()
        {
            return _valueIfNull;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}