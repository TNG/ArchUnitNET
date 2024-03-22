using System;
using System.Runtime.Serialization;

namespace ArchUnitNET.Domain.PlantUml.Exceptions
{
    [Serializable]
    internal class ComponentIntersectionException : Exception
    {
        public ComponentIntersectionException() { }

        public ComponentIntersectionException(string message)
            : base(message) { }

        public ComponentIntersectionException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ComponentIntersectionException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
