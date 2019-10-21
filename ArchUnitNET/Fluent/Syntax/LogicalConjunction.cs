using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Fluent.Syntax
{
    public class LogicalConjunction : IHasDescription
    {
        private readonly Func<IEnumerable<object>, IEnumerable<object>, IEnumerable<object>> _enumerableFunction;
        private readonly Func<bool, bool, bool> _logicalConjunction;

        public LogicalConjunction(Func<bool, bool, bool> logicalConjunction,
            Func<IEnumerable<object>, IEnumerable<object>, IEnumerable<object>> enumerableFunction, string description)
        {
            _logicalConjunction = logicalConjunction;
            _enumerableFunction = enumerableFunction;
            Description = description;
        }

        public string Description { get; }

        public bool Evaluate(bool value1, bool value2)
        {
            return _logicalConjunction(value1, value2);
        }

        public IEnumerable<T> Evaluate<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            return _enumerableFunction(enumerable1.Cast<object>(), enumerable2.Cast<object>()).Cast<T>();
        }

        public override string ToString()
        {
            return Description;
        }

        private bool Equals(LogicalConjunction other)
        {
            return string.Equals(Description, other.Description);
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

            return obj.GetType() == GetType() && Equals((LogicalConjunction) obj);
        }

        public override int GetHashCode()
        {
            return Description != null ? Description.GetHashCode() : 0;
        }
    }
}