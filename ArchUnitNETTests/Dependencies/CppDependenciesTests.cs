using System.Runtime.InteropServices;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
#if WINDOWS
    public class CppDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembliesRecursively(
                new[] { typeof(CppExampleClassUser).Assembly },
                filterFunc => FilterResult.LoadAndContinue
            )
            .Build();

        [Fact]
        public void CppClassUserFound()
        {
            // Skip test if not running on Windows
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var exampleCppUser = Architecture.GetClassOfType(typeof(CppExampleClassUser));
            Assert.Contains(exampleCppUser, Architecture.Classes);
        }
    }

    internal class CppExampleClassUser
    {
        CppExampleClass _cppExampleClass = new CppExampleClass();
    }
#endif
}
