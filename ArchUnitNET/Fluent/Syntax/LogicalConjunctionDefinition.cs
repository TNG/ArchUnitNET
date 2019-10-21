namespace ArchUnitNET.Fluent.Syntax
{
    public static class LogicalConjunctionDefinition
    {
        public static readonly LogicalConjunction And = new And();

        public static readonly LogicalConjunction Or = new Or();

        public static readonly LogicalConjunction ForwardSecondValue = new ForwardSecondValue();
    }
}