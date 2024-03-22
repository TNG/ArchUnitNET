namespace ArchUnitNET.Domain.PlantUml.Export
{
    public interface IPlantUmlElement
    {
        string GetPlantUmlString(RenderOptions renderOptions);
    }
}
