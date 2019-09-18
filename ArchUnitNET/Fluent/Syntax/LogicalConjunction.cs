using System;

namespace ArchUnitNET.Fluent.Syntax
{
    public class
        LogicalConjunction : IHasDescription //TODO change type to readonly struct and change c# version to 7.2?
    {
        private readonly Func<bool, bool, bool> _logicalConjunction;

        public LogicalConjunction(Func<bool, bool, bool> logicalConjunction, string description)
        {
            _logicalConjunction = logicalConjunction;
            Description = description;
        }

        public string Description { get; }

        public bool Evaluate(bool value1, bool value2)
        {
            return _logicalConjunction(value1, value2);
        }

        public override string ToString()
        {
            return Description;
        }
    }
}