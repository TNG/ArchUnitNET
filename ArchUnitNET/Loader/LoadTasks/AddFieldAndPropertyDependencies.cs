using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddFieldAndPropertyDependencies : ILoadTask
    {
        private readonly IType _type;

        public AddFieldAndPropertyDependencies(IType type)
        {
            _type = type;
        }

        public void Execute()
        {
            _type
                .GetFieldMembers()
                .ForEach(field =>
                {
                    var dependency = new FieldTypeDependency(field);
                    AddDependencyIfMissing(field, dependency);
                });

            _type
                .GetPropertyMembers()
                .ForEach(property =>
                {
                    var dependency = new PropertyTypeDependency(property);
                    AddDependencyIfMissing(property, dependency);
                });
        }

        private static void AddDependencyIfMissing(IMember member, IMemberTypeDependency dependency)
        {
            if (!member.Dependencies.Contains(dependency))
            {
                member.Dependencies.Add(dependency);
            }
        }
    }
}
