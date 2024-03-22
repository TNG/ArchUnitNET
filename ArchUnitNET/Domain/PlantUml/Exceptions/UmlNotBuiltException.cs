using System;
using System.Runtime.Serialization;

namespace ArchUnitNET.Domain.PlantUml.Exceptions
{
    [Serializable]
    internal class UmlNotBuiltException : Exception
    {
        public UmlNotBuiltException() { }

        public UmlNotBuiltException(string message)
            : base(message) { }

        public UmlNotBuiltException(string message, Exception innerException)
            : base(message, innerException) { }

        protected UmlNotBuiltException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
