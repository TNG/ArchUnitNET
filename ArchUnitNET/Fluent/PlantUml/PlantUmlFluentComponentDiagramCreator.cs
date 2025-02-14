using ArchUnitNET.Domain;
using ArchUnitNET.Domain.PlantUml.Export;

namespace ArchUnitNET.Fluent.PlantUml
{
    internal class PlantUmlFluentComponentDiagramCreator : IHasDescription
    {
        public readonly PlantUmlFileBuilder Builder;

        public PlantUmlFluentComponentDiagramCreator()
        {
            Description = "Class diagram";
            Builder = new PlantUmlFileBuilder();
        }

        public void AddToDescription(string description)
        {
            Description += " " + description.Trim();
        }

        public string Description { get; private set; }
    }
}
