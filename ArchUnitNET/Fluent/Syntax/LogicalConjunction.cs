using System;

namespace ArchUnitNET.Fluent.Syntax
{
    public class LogicalConjunction
    {
        private readonly Func<bool, bool, bool> _logicalConjunction;

        public LogicalConjunction(Func<bool, bool, bool> logicalConjunction)
        {
            _logicalConjunction = logicalConjunction;
        }

        public bool Evaluate(bool value1, bool value2)
        {
            return _logicalConjunction(value1, value2);
        }
    }
}