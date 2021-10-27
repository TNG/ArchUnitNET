using System;
using System.Runtime.Serialization;

namespace ArchUnitNET.Domain.PlantUml.Exceptions
{
    [Serializable]
    internal class PlantUmlParseException : Exception
    {
        public PlantUmlParseException()
        {
        }

        public PlantUmlParseException(string message) : base(message)
        {
        }

        public PlantUmlParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlantUmlParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}