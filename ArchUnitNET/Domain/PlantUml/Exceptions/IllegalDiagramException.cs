using System;
using System.Runtime.Serialization;

namespace ArchUnitNET.Domain.PlantUml.Exceptions
{
    [Serializable]
    internal class IllegalDiagramException : Exception
    {
        private object p;

        public IllegalDiagramException()
        {
        }

        public IllegalDiagramException(object p)
        {
            this.p = p;
        }

        public IllegalDiagramException(string message) : base(message)
        {
        }

        public IllegalDiagramException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalDiagramException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}