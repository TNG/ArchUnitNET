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
        /// Whether slice diagrams nest each slice in <c>package</c> blocks along the namespace its
        /// types matched, instead of drawing every slice on its own under its name. A slice whose
        /// types matched different namespaces before the first capture group, or that does not
        /// come from a slice pattern, is drawn on its own either way. Slices of the obsolete
        /// <c>MatchingWithPackages</c> are always nested.
        /// </summary>
        public bool NestSlicesInPackages { get; set; } = false;
    }
}
