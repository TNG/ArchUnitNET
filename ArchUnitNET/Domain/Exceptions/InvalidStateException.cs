using System;

namespace ArchUnitNET.Domain.Exceptions
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException(string message)
            : base(message) { }
    }
}
