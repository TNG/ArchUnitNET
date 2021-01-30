using System.Collections.Generic;

namespace ArchUnitNET.PlantUml
{
    internal class Stereotype
    {
        private readonly string _value;
        public Stereotype(string stereotype)
        {
            _value = stereotype ?? throw new System.ArgumentNullException(nameof(stereotype));
        }
        public string AsString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            return obj is Stereotype stereotype &&
                   _value == stereotype._value;
        }

        public override int GetHashCode()
        {
            return -1939223833 + EqualityComparer<string>.Default.GetHashCode(_value);
        }
    }
}