using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class GivenPlantUmlFluentComponentDiagram
    {
        private readonly PlantUmlFileBuilder _builder;

        internal GivenPlantUmlFluentComponentDiagram(PlantUmlFileBuilder builder)
        {
            _builder = builder;
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
