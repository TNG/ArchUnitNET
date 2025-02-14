using System.Linq;
using System.Runtime.Versioning;

namespace ArchUnitNET.Domain.Extensions
{
    public static class AssemblyExtensions
    {
        public static bool IsDotNetStandard(this Assembly assembly)
        {
            return assembly.Name.StartsWith("netstandard")
                || assembly.GetTargetFramework().StartsWith(".NETStandard");
        }

        public static bool IsDotNetCore(this Assembly assembly)
        {
            return assembly.GetTargetFramework().StartsWith(".NETCore");
        }

        public static bool IsDotNetFramework(this Assembly assembly)
        {
            return assembly.Name.StartsWith("mscorlib")
                || assembly.GetTargetFramework().StartsWith(".NETFramework");
        }

        public static string GetTargetFramework(this Assembly assembly)
        {
            var targetFrameworkAttribute = assembly.AttributeInstances.FirstOrDefault(att =>
                att.Type.FullName == typeof(TargetFrameworkAttribute).FullName
            );

            var frameworkNameAttribute =
                targetFrameworkAttribute?.AttributeArguments.FirstOrDefault(argument =>
                    !(argument is AttributeNamedArgument namedArgument)
                    || namedArgument.Name != "FrameworkDisplayName"
                );

            return frameworkNameAttribute?.Value as string ?? "";
        }
    }
}
