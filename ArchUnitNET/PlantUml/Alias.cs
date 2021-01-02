using System;
using System.Collections.Generic;

namespace ArchUnitNET.PlantUml
{
    internal class Alias
    {
        private string _value;

        public Alias(string value)
        {
            if (value.Contains("[") || value.Contains("]") || value.Contains(@""""))
            {
                throw new IllegalDiagramException(string.Format(@"Alias '{0}' should not contain character(s): '[' or ']' or '""'", value));
            }
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override bool Equals(object obj)
        {
            return obj is Alias alias &&
                   _value == alias._value;
        }

        public override int GetHashCode()
        {
            return -1939223833 + EqualityComparer<string>.Default.GetHashCode(_value);
        }

        internal string asString()
        {
            return _value;
        }
    }
}