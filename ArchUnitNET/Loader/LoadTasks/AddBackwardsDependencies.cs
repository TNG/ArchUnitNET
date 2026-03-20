using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Registers each of the type's dependencies as a backwards dependency on the
    /// target type, and each member-member dependency as a backwards dependency on
    /// the target member.
    /// </summary>
    internal static class AddBackwardsDependencies
    {
        internal static void Execute(IType type)
        {
            type.Dependencies.ForEach(dependency =>
                dependency.Target.BackwardsDependencies.Add(dependency)
            );

            var memberMemberDependencies = type
                .Members.SelectMany(member => member.MemberDependencies)
                .OfType<IMemberMemberDependency>();
            memberMemberDependencies.ForEach(memberDependency =>
                memberDependency.TargetMember.MemberBackwardsDependencies.Add(memberDependency)
            );
        }
    }
}
