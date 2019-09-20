﻿namespace ArchUnitNET.Fluent.Syntax
{
    public static class LogicalConjunctionDefinition
    {
        public static readonly LogicalConjunction And = new LogicalConjunction((b1, b2) => b1 && b2, "and");
        public static readonly LogicalConjunction Or = new LogicalConjunction((b1, b2) => b1 || b2, "or");
        public static readonly LogicalConjunction ForwardSecondValue = new LogicalConjunction((b1, b2) => b2, "");
    }
}