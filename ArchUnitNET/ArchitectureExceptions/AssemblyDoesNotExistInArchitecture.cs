using System;

namespace ArchUnitNET.ArchitectureExceptions
{
    public class AssemblyDoesNotExistInArchitecture : Exception
    {
        public AssemblyDoesNotExistInArchitecture(string message) : base(message)
        {
        }

        public AssemblyDoesNotExistInArchitecture(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}