using System;

namespace ArchUnitNET.Domain.Exceptions
{
    public class AssemblyDoesNotExistInArchitecture : Exception
    {
        public AssemblyDoesNotExistInArchitecture(string message)
            : base(message) { }
    }
}
