using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class GivenSlices : IObjectProvider<Slice>
    {
        private readonly SliceAssignment _sliceAssignment;

        public GivenSlices(SliceAssignment sliceAssignment)
        {
            _sliceAssignment = sliceAssignment;
        }

        public string Description => _sliceAssignment.Description;

        public SlicesShould Should()
        {
            return new SlicesShould(_sliceAssignment);
        }

        public IEnumerable<Slice> GetObjects(Architecture architecture)
        {
            return GetSlices(architecture);
        }

        public string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        )
        {
            return $"{multipleDescription} {Description}";
        }

        private IEnumerable<Slice> GetSlices(Architecture architecture)
        {
            return _sliceAssignment
                .Apply(architecture.Types)
                .Where(slice => !slice.Identifier.Ignored);
        }
    }
}
