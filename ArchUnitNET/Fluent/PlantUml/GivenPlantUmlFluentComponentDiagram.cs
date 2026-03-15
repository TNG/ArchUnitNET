using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class GivenPlantUmlFluentComponentDiagram
    {
        private readonly PlantUmlFileBuilder _builder;
        private readonly string _description;

        internal GivenPlantUmlFluentComponentDiagram(
            PlantUmlFileBuilder builder,
            string description
        )
        {
            _builder = builder;
            _description = description;
        }

        public string AsString(RenderOptions renderOptions = null)
        {
            return _builder.AsString(renderOptions);
        }

        public void WriteToFile(string path, RenderOptions renderOptions = null)
        {
            _builder.WriteToFile(path, renderOptions);
        }
    }
}
