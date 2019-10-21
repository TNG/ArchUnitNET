using System.Linq;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class LogicalConjunctionDefinition
    {
        public static readonly LogicalConjunction And =
            new LogicalConjunction((b1, b2) => b1 && b2, (e1, e2) => e1.Intersect(e2), "and");

        public static readonly LogicalConjunction Or =
            new LogicalConjunction((b1, b2) => b1 || b2, (e1, e2) => e1.Union(e2), "or");

        public static readonly LogicalConjunction ForwardSecondValue =
            new LogicalConjunction((b1, b2) => b2, (e1, e2) => e2, "");
    }
}