using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Creates <see cref="FieldTypeDependency"/> and <see cref="PropertyTypeDependency"/>
    /// instances for each field and property member of the type.
    /// </summary>
    internal static class AddFieldAndPropertyDependencies
    {
        internal static void Execute(IType type)
        {
            type.GetFieldMembers()
                .ForEach(field =>
                {
                    var dependency = new FieldTypeDependency(field);
                    if (!field.MemberDependencies.Contains(dependency))
                    {
                        field.MemberDependencies.Add(dependency);
                    }
                });

            type.GetPropertyMembers()
                .ForEach(property =>
                {
                    var dependency = new PropertyTypeDependency(property);
                    if (!property.MemberDependencies.Contains(dependency))
                    {
                        property.MemberDependencies.Add(dependency);
                    }
                });
        }
    }
}
