using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    /// <summary>
    /// A package (or C4 boundary) that holds nested slices, rendered as a single block.
    /// </summary>
    /// <remarks>
    /// Every nested slice carries the full chain of packages it lives in. Rendering each slice on
    /// its own would re-open its parent packages once per slice. PlantUML merges those re-opened
    /// packages, but the source then reads nothing like a hand-written diagram. Collecting the
    /// slices into a tree first opens every package exactly once.
    /// </remarks>
    internal class PlantUmlSliceTree : IPlantUmlElement
    {
        private readonly string _name;
        private readonly bool _c4Style;
        private string _color;

        // Child packages and components in the order they were first seen, so the output keeps
        // the order in which the slices were added.
        private readonly List<object> _children = new List<object>();

        /// <summary>
        /// Creates the tree for the root package of the given nested slice, holding that slice.
        /// </summary>
        public PlantUmlSliceTree(PlantUmlSlice slice)
            : this(slice.RootPackage, slice.IsC4Style)
        {
            Add(slice);
        }

        private PlantUmlSliceTree(string name, bool c4Style)
        {
            _name = name;
            _c4Style = c4Style;
        }

        /// <summary>
        /// Adds a nested slice whose outermost package is the root of this tree, creating the
        /// packages between the root and the slice as needed.
        /// </summary>
        public void Add(PlantUmlSlice slice)
        {
            var node = this;
            foreach (var segment in slice.PackagePath.Skip(1))
            {
                node = node.GetOrAddPackage(segment, slice.IsC4Style);
            }

            if (slice.Leaf != "")
            {
                node._children.Add(slice);
            }
            else if (slice.SliceColor != null)
            {
                node._color = slice.SliceColor;
            }
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            var result = new StringBuilder();
            Render(result, 0);
            return result.AppendLine().ToString();
        }

        private PlantUmlSliceTree GetOrAddPackage(string name, bool c4Style)
        {
            var package = _children
                .OfType<PlantUmlSliceTree>()
                .FirstOrDefault(child => child._name == name);
            if (package == null)
            {
                package = new PlantUmlSliceTree(name, c4Style);
                _children.Add(package);
            }

            return package;
        }

        private void Render(StringBuilder result, int depth)
        {
            var indent = new string(' ', 2 * depth);
            var childIndent = new string(' ', 2 * (depth + 1));
            if (_c4Style)
            {
                result.AppendLine(indent + "Boundary(" + _name + ", " + _name + ") {");
            }
            else if (_color != null)
            {
                result.AppendLine(indent + "package " + _name + " #" + _color + " {");
            }
            else
            {
                result.AppendLine(indent + "package " + _name + " {");
            }

            foreach (var child in _children)
            {
                if (child is PlantUmlSliceTree package)
                {
                    package.Render(result, depth + 1);
                }
                else
                {
                    result.AppendLine(childIndent + ((PlantUmlSlice)child).GetLeafString());
                }
            }

            result.AppendLine(indent + "}");
        }
    }
}
