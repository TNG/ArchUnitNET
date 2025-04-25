namespace ArchUnitNET.Fluent.PlantUml
{
    public static class PlantUmlDefinition
    {
        public static PlantUmlFluentComponentDiagramInitializer ComponentDiagram()
        {
            var fluentCreator = new PlantUmlFluentComponentDiagramCreator();
            return new PlantUmlFluentComponentDiagramInitializer(fluentCreator);
        }
    }
}
