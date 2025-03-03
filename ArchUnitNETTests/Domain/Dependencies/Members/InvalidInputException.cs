using System;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message)
            : base(message) { }

        public InvalidInputException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
