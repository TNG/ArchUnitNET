using System.Collections.Generic;

namespace ArchUnitNET.Domain.PlantUml.Export
{
    public interface IPlantUmlContainer : IPlantUmlElement
    {
        List<IPlantUmlElement> PlantUmlElements { get; }
        void AddElement(IPlantUmlElement plantUmlElement);
        void AddElements(IEnumerable<IPlantUmlElement> plantUmlElements);
    }
}
