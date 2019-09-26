using System;

namespace ArchUnitNET.Fluent.Exceptions
{
    public class CannotGetObjectsOfCombinedArchRuleCreatorException : Exception
    {
        public CannotGetObjectsOfCombinedArchRuleCreatorException(string message) : base(message)
        {
        }

        public CannotGetObjectsOfCombinedArchRuleCreatorException(string message, Exception inner) : base(message,
            inner)
        {
        }
    }
}