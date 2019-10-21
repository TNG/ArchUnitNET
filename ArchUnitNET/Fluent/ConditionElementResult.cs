using ArchUnitNET.Fluent.Syntax;

namespace ArchUnitNET.Fluent
{
    public class ConditionElementResult
    {
        public readonly ConditionResult ConditionResult;
        public readonly LogicalConjunction LogicalConjunction;

        public ConditionElementResult(ConditionResult conditionResult, LogicalConjunction logicalConjunction)
        {
            ConditionResult = conditionResult;
            LogicalConjunction = logicalConjunction;
        }
    }
}