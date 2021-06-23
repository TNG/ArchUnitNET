using System;

namespace ArchUnitNET.Domain.PlantUml
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
            var hashCode = 397 ^ (_value != null ? _value.GetHashCode() : 0);
            return hashCode;
        }
    }
}