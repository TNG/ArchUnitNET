using System;

namespace ArchUnitNET.Domain.Exceptions
{
    public class TypeDoesNotExistInArchitecture : Exception
    {
        public TypeDoesNotExistInArchitecture(string message)
            : base(message) { }
    }
}
