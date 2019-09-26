using System;

namespace ArchUnitNET.Fluent.Exceptions
{
    public class CannotGetObjectsOfCombinedArchRuleException : Exception
    {
        public CannotGetObjectsOfCombinedArchRuleException(string message) : base(message)
        {
        }

        public CannotGetObjectsOfCombinedArchRuleException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}