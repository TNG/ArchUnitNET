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
        }

        public override string ToString()
        {
            return _name;
        }

        public void UseS4Style()
        {
            C4Style = true;
        }

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
                var segments = _name.Remove(0, Namespace.Length).Split('.');
                path.AddRange(segments.Take(segments.Length - 1));
                return path;
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
                result.Append("Container(" + _name + ", " + _name + ")");
                AppendHyperlink(result);
            }
            else
            {
                result.Append("[" + _name + "]");
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
                result.Append("Container(" + _name + ", " + Leaf + ")");
                AppendHyperlink(result);
            }
            else
            {
                result.Append("[" + Leaf + "] as " + _name);
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
