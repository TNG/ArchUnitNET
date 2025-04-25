using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class GivenSlices : IObjectProvider<Slice>
    {
        private readonly SliceRuleCreator _ruleCreator;

        public GivenSlices(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        public string Description => _ruleCreator.Description;

        public SlicesShould Should()
        {
            _ruleCreator.AddToDescription("should");
            return new SlicesShould(_ruleCreator);
        }

        public IEnumerable<Slice> GetObjects(Architecture architecture)
        {
            return _ruleCreator.GetSlices(architecture);
        }
    }
}
