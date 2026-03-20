using ArchUnitNETTests.Domain.Dependencies.Members;

namespace ArchUnitNETTests.Loader
{
    public class BackingFieldExamples
    {
        private ChildField _fieldPropertyPair;
        public PropertyType AutoProperty { get; set; }

        public PropertyType FieldPropertyPair
        {
            get => _fieldPropertyPair;
            set => _fieldPropertyPair = (ChildField)value;
        }

        public PropertyType LambdaFieldPropertyPair { get; set; }
    }
}
