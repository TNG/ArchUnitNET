using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Assigns declarers to generic parameters and creates type-constraint dependencies
    /// for both type-level and member-level generic parameters.
    /// </summary>
    internal static class AddGenericParameterDependencies
    {
        internal static void Execute(IType type)
        {
            // Type-level generic parameters
            foreach (var genericParameter in type.GenericParameters)
            {
                genericParameter.AssignDeclarer(type);
                foreach (var typeInstanceConstraint in genericParameter.TypeInstanceConstraints)
                {
                    var dependency = new TypeGenericParameterTypeConstraintDependency(
                        genericParameter,
                        typeInstanceConstraint
                    );
                    genericParameter.Dependencies.Add(dependency);
                }
            }

            // Member-level generic parameters
            foreach (var member in type.Members)
            {
                foreach (var genericParameter in member.GenericParameters)
                {
                    genericParameter.AssignDeclarer(member);
                    foreach (var typeInstanceConstraint in genericParameter.TypeInstanceConstraints)
                    {
                        var dependency = new MemberGenericParameterTypeConstraintDependency(
                            genericParameter,
                            typeInstanceConstraint
                        );
                        genericParameter.Dependencies.Add(dependency);
                    }
                }
            }
        }
    }
}
