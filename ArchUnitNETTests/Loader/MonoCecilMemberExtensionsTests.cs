using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using Xunit;
using static ArchUnitNETTests.StaticTestArchitectures;

namespace ArchUnitNETTests.Loader
{
    public class MonoCecilMemberExtensionsTests
    {
        private static readonly Architecture LoaderTestArchitecture =
            StaticTestArchitectures.LoaderTestArchitecture;

        [Fact]
        public void GetParameters_PreservesDuplicateParameterTypes()
        {
            var @class = LoaderTestArchitecture
                .Types.OfType<Class>()
                .FirstOrDefault(c => c.Name == "ClassWithDuplicateParameters");

            Assert.NotNull(@class);

            var method = @class
                .GetMethodMembers()
                .First(m => m.FullName.Contains("MethodWithSameParameterType"));

            Assert.Equal(2, method.ParameterInstances.Count);
            Assert.All(
                method.ParameterInstances,
                p => Assert.Equal("System.String", p.Type.FullName)
            );
        }

        [Fact]
        public void GetParameters_PreservesMultipleDuplicateParameterTypes()
        {
            var @class = LoaderTestArchitecture
                .Types.OfType<Class>()
                .FirstOrDefault(c => c.Name == "ClassWithDuplicateParameters");

            Assert.NotNull(@class);

            var method = @class
                .GetMethodMembers()
                .First(m => m.FullName.Contains("MethodWithTripleDuplicate"));

            Assert.Equal(3, method.ParameterInstances.Count);
            Assert.All(
                method.ParameterInstances,
                p => Assert.Equal("System.Int32", p.Type.FullName)
            );
        }

        [Fact]
        public void GetParameters_MixedTypeWithDuplicates()
        {
            var @class = LoaderTestArchitecture
                .Types.OfType<Class>()
                .FirstOrDefault(c => c.Name == "ClassWithDuplicateParameters");

            Assert.NotNull(@class);

            var method = @class
                .GetMethodMembers()
                .First(m => m.FullName.Contains("MethodWithMixedParamTypes"));

            Assert.Equal(5, method.ParameterInstances.Count);

            var stringParameters = method.ParameterInstances.Count(p =>
                p.Type.FullName == "System.String"
            );
            var intParameters = method.ParameterInstances.Count(p =>
                p.Type.FullName == "System.Int32"
            );
            var boolParameters = method.ParameterInstances.Count(p =>
                p.Type.FullName == "System.Boolean"
            );

            Assert.Equal(3, stringParameters);
            Assert.Equal(1, intParameters);
            Assert.Equal(1, boolParameters);
        }

        [Fact]
        public void GetParameters_CustomTypeDuplicates()
        {
            var @class = LoaderTestArchitecture
                .Types.OfType<Class>()
                .FirstOrDefault(c => c.Name == "ClassWithDuplicateParameters");

            Assert.NotNull(@class);

            var method = @class
                .GetMethodMembers()
                .First(m => m.FullName.Contains("MethodWithCustomTypeDuplicates"));

            Assert.Equal(2, method.ParameterInstances.Count);
            Assert.All(
                method.ParameterInstances,
                p => Assert.Contains("CustomType", p.Type.FullName)
            );
        }
    }
}
