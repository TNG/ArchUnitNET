namespace ArchUnitNET.PlantUml
{
    internal class Alias
    {
        private string _value;

        public Alias(string value)
        {
            if (value.Contains("[") || value.Contains("]") || value.Contains("\""))
            {
                throw new IllegalDiagramException(string.Format("Alias '{0}' should not contain character(s): '[' or ']' or '\"'", value));
            }
            _value = value;
        }

        internal string asString()
        {
            return _value;
        }

    }
}