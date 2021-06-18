using System;
using System.Collections.Generic;
using System.Text;

namespace TestAssembly.Diagram.NoDependencies.SomeNamespace
{
    public class IndependentOrigin
    {
        #pragma warning disable CS0169
        private readonly DependencyWithinNamespace _legalField;
        #pragma warning restore CS0169
    }
}
