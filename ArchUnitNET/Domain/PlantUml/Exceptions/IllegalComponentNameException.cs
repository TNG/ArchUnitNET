using System;
using System.Runtime.Serialization;

namespace ArchUnitNET.Domain.PlantUml.Exceptions
{
    [Serializable]
    internal class IllegalComponentNameException : Exception
    {
        public IllegalComponentNameException() { }

        public IllegalComponentNameException(string message)
            : base(message) { }

        public IllegalComponentNameException(string message, Exception innerException)
            : base(message, innerException) { }

        protected IllegalComponentNameException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
