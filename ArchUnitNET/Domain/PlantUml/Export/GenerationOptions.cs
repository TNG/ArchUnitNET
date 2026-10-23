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

        /// <summary>
        /// Whether slice diagrams draw the namespace tree as <c>package</c> blocks and put each
        /// slice in the package of the deepest namespace that contains all of its types. A package
        /// with no slice of its own and a single child package is merged with it. A slice whose
        /// types share no namespace is drawn outside any package. Slices of the obsolete
        /// <c>MatchingWithPackages</c> are always nested.
        /// </summary>
        public bool NestSlicesInPackages { get; set; } = false;
    }
}
