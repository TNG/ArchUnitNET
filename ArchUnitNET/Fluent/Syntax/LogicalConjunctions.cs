using System;

namespace ArchUnitNET.Fluent.Syntax
{
    public static class LogicalConjunctions
    {
        public static Func<bool, bool, bool> And()
        {
            return (b1, b2) => b1 && b2;
        }

        public static Func<bool, bool, bool> Or()
        {
            return (b1, b2) => b1 || b2;
        }
    }
}