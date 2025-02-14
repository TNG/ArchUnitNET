using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    public class GivenPlantUmlFluentComponentDiagram
    {
        private readonly PlantUmlFluentComponentDiagramCreator _fluentComponentDiagramCreator;

        internal GivenPlantUmlFluentComponentDiagram(
            PlantUmlFluentComponentDiagramCreator fluentComponentDiagramCreator
        )
        {
            _fluentComponentDiagramCreator = fluentComponentDiagramCreator;
        }

        public string AsString(RenderOptions renderOptions = null)
        {
            return _fluentComponentDiagramCreator.Builder.AsString(renderOptions);
        }

        public void WriteToFile(string path, RenderOptions renderOptions = null)
        {
            _fluentComponentDiagramCreator.Builder.WriteToFile(path, renderOptions);
        }
    }
}
