using System;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent
{
    public class SimpleCondition<T> : ICondition<T> where T : ICanBeAnalyzed
    {
        private readonly Func<T, bool> _condition;

        public SimpleCondition(Func<T, bool> condition, string description, string failDescription)
        {
            _condition = condition;
            Description = description;
            FailDescription = failDescription;
        }

        public string Description { get; }
        public string FailDescription { get; }

        public bool Check(T obj, Architecture architecture)
        {
            return Evaluate(obj);
        }

        public bool CheckNull()
        {
            return true;
        }

        public bool Evaluate(T obj)
        {
            return _condition(obj);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}