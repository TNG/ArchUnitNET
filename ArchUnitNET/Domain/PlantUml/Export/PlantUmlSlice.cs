using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public class PlantUmlSlice : IPlantUmlElement
    {
        private readonly string _name;
        private string Hyperlink { get; }
        private string Namespace { get; }
        private string Color { get; }
        private bool C4Style { get; set; }

        // Set when the caller already knows where the slice sits; see the internal constructor.
        private readonly IReadOnlyList<string> _packages;
        private readonly string _leaf;

        public PlantUmlSlice(
            string name,
            string nameSpace = null,
            string color = null,
            string hyperlink = null
        )
        {
            PlantUmlNameChecker.AssertNoForbiddenCharacters(name, hyperlink, nameSpace);
            PlantUmlNameChecker.AssertNotNullOrEmpty(name);
            _name = name;
            Hyperlink = hyperlink;
            Namespace = nameSpace;
            Color = color;
            Alias = name;
        }

        /// <summary>
        /// Creates a nested slice whose packages and label are given rather than read off its
        /// name.
        /// </summary>
        /// <param name="name">The full slice name, which is also the alias arrows point at.</param>
        /// <param name="nameSpace">The root package, with a trailing ".".</param>
        /// <param name="packages">The packages between the root package and the slice.</param>
        /// <param name="leaf">
        /// The label inside the innermost package, or an empty string if the slice only stands
        /// for that package.
        /// </param>
        /// <param name="color">The background color, if any.</param>
        internal PlantUmlSlice(
            string name,
            string nameSpace,
            IReadOnlyList<string> packages,
            string leaf,
            string color = null
        )
            : this(name, nameSpace, color)
        {
            _packages = packages;
            _leaf = leaf;
        }

        public override string ToString()
        {
            return _name;
        }

        public void UseS4Style()
        {
            C4Style = true;
        }

        /// <summary>
        /// The id PlantUML knows this slice by, which dependencies point at. It is the slice
        /// name unless that name contains characters PlantUML does not accept in an id; see
        /// <see cref="PlantUmlDiagram"/>.
        /// </summary>
        internal string Alias { get; set; }

        internal bool IsNested => Namespace != null;

        internal bool IsC4Style => C4Style;

        internal string SliceColor => Color;

        /// <summary>
        /// The outermost package this slice is nested in, which is its whole namespace.
        /// </summary>
        internal string RootPackage => Namespace.Remove(Namespace.Length - 1);

        /// <summary>
        /// The packages this slice is nested in, outermost first. The first entry is the whole
        /// namespace of the slice, the others are the segments between it and the leaf.
        /// </summary>
        internal IReadOnlyList<string> PackagePath
        {
            get
            {
                var path = new List<string> { RootPackage };
                if (_packages != null)
                {
                    path.AddRange(_packages);
                    return path;
                }

                var segments = _name.Remove(0, Namespace.Length).Split('.');
                path.AddRange(segments.Take(segments.Length - 1));
                return path;
            }
        }

        /// <summary>
        /// The ids of the packages this slice is nested in, outermost first; see
        /// <see cref="PlantUmlSliceTree"/>.
        /// </summary>
        internal IEnumerable<string> PackageIds
        {
            get
            {
                string id = null;
                foreach (var package in PackagePath)
                {
                    id = id == null ? package : id + "." + package;
                    yield return id;
                }
            }
        }

        /// <summary>
        /// The label of the component inside its innermost package, or an empty string if the
        /// slice only stands for that package.
        /// </summary>
        internal string Leaf
        {
            get
            {
                if (_leaf != null)
                {
                    return _leaf;
                }

                var name = _name.Remove(0, Namespace.Length);
                return name.Substring(name.LastIndexOf('.') + 1);
            }
        }

        public string GetPlantUmlString(RenderOptions renderOptions)
        {
            if (IsNested)
            {
                return new PlantUmlSliceTree(this).GetPlantUmlString(renderOptions);
            }

            var result = new StringBuilder();
            if (C4Style)
            {
                result.Append("Container(" + Alias + ", " + _name + ")");
                AppendHyperlink(result);
            }
            else
            {
                result.Append("[" + _name + "]");
                if (Alias != _name)
                {
                    result.Append(" as " + Alias);
                }
                AppendHyperlink(result);
                if (Color != null)
                {
                    result.Append(" #" + Color);
                }
            }

            return result.AppendLine().ToString();
        }

        internal string GetLeafString()
        {
            var result = new StringBuilder();
            if (C4Style)
            {
                result.Append("Container(" + Alias + ", " + Leaf + ")");
                AppendHyperlink(result);
            }
            else
            {
                // PlantUML reads a "*" at the start of a label as a list bullet and drops it,
                // which would turn a label such as "*.Http" into ".Http".
                var label = Leaf.StartsWith("*", StringComparison.Ordinal) ? "~" + Leaf : Leaf;
                result.Append("[" + label + "] as " + Alias);
                AppendHyperlink(result);
                if (Color != null)
                {
                    result.Append(" #" + Color);
                }
            }

            return result.ToString();
        }

        // PlantUML only accepts a link in front of the color, not after it.
        private void AppendHyperlink(StringBuilder result)
        {
            if (Hyperlink != null)
            {
                result.Append(" [[" + Hyperlink + "]] ");
            }
        }
    }
}
