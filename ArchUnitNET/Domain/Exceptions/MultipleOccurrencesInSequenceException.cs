using System;

namespace ArchUnitNET.Domain.Exceptions
{
    public class MultipleOccurrencesInSequenceException : Exception
    {
        public MultipleOccurrencesInSequenceException(string message)
            : base(message) { }
    }
}
