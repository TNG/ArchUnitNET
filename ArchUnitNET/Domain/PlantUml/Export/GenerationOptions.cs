using System;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class GenerationOptions
    {
        public Func<ITypeDependency, bool> DependencyFilter { get; set; }
        public bool IncludeDependenciesToOther { get; set; }
        public bool IncludeNodesWithoutDependencies { get; set; } = true;
        public bool LimitDependencies { get; set; } = false;
        public bool C4Style { get; set; } = false;
    }
}
