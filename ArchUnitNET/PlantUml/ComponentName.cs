using System;
using System.Collections.Generic;

namespace ArchUnitNET.PlantUml
{
    internal class ComponentName
    {
        private readonly string _value;

        public ComponentName(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string AsString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            return obj is ComponentName name &&
                   _value == name._value;
        }

        public override int GetHashCode()
        {
            return -1939223833 + EqualityComparer<string>.Default.GetHashCode(_value);
        }
    }
}