using System;
using ArchUnitNET.Domain.PlantUml.Exceptions;

namespace ArchUnitNET.Domain.PlantUml.Import
{
    internal class Alias
    {
        private string _value;

        public Alias(string value)
        {
            if (value.Contains("[") || value.Contains("]") || value.Contains(@""""))
            {
                throw new IllegalDiagramException(
                    string.Format(
                        @"Alias '{0}' should not contain character(s): '[' or ']' or '""'",
                        value
                    )
                );
            }

            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override bool Equals(object obj)
        {
            return obj is Alias alias && _value == alias._value;
        }

        public override int GetHashCode()
        {
            var hashCode = 397 ^ (_value != null ? _value.GetHashCode() : 0);
            return hashCode;
        }

        internal string asString()
        {
            return _value;
        }
    }
}
