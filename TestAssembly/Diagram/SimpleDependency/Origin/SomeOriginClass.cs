using System;
using System.Collections.Generic;
using System.Text;
using TestAssembly.Diagram.SimpleDependency.Target;

namespace TestAssembly.Diagram.SimpleDependency.Origin
{
    public class SomeOriginClass
    {
        internal void AccessTarget()
        {
            new SomeTargetClass();
        }
    }
}
