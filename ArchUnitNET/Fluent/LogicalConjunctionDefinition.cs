//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Fluent
{
    public static class LogicalConjunctionDefinition
    {
        public static readonly LogicalConjunction And = new AndConjunction();

        public static readonly LogicalConjunction Or = new OrConjunction();

        public static readonly LogicalConjunction ForwardSecondValue =
            new ForwardSecondValueConjunction();

        private class AndConjunction : LogicalConjunction
        {
            public AndConjunction()
                : base((b1, b2) => b1 && b2, "and") { }

            public override IEnumerable<T> Evaluate<T>(
                IEnumerable<T> enumerable1,
                IEnumerable<T> enumerable2
            )
            {
                return enumerable1.Intersect(enumerable2);
            }
        }

        private class OrConjunction : LogicalConjunction
        {
            public OrConjunction()
                : base((b1, b2) => b1 || b2, "or") { }

            public override IEnumerable<T> Evaluate<T>(
                IEnumerable<T> enumerable1,
                IEnumerable<T> enumerable2
            )
            {
                return enumerable1.Union(enumerable2);
            }
        }

        private class ForwardSecondValueConjunction : LogicalConjunction
        {
            public ForwardSecondValueConjunction()
                : base((b1, b2) => b2, "") { }

            public override IEnumerable<T> Evaluate<T>(
                IEnumerable<T> enumerable1,
                IEnumerable<T> enumerable2
            )
            {
                return enumerable2;
            }
        }
    }
}
