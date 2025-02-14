using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using ArchUnitNETTests.Domain.Dependencies.Attributes;
using ArchUnitNETTests.Domain.Dependencies.Members;
using TestAssembly;

// ReSharper disable InconsistentNaming

namespace ArchUnitNETTests
{
    public static class StaticTestArchitectures
    {
        public static readonly Architecture AttributeDependencyTestArchitecture = new ArchLoader()
            .LoadAssemblies(typeof(ClassWithExampleAttribute).Assembly, typeof(Class1).Assembly)
            .Build();

        public static readonly Architecture ArchUnitNETTestArchitecture = new ArchLoader()
            .LoadAssemblies(typeof(BaseClass).Assembly)
            .Build();

        public static readonly Architecture AttributeArchitecture = new ArchLoader()
            .LoadAssemblies(typeof(AttributeNamespace.ClassWithoutAttributes).Assembly)
            .Build();

        public static readonly Architecture DependencyArchitecture = new ArchLoader()
            .LoadAssemblies(typeof(TypeDependencyNamespace.BaseClass).Assembly)
            .Build();

        public static readonly Architecture LoaderTestArchitecture = new ArchLoader()
            .LoadAssemblies(
                typeof(LoaderTestAssembly.LoaderTestAssembly).Assembly,
                typeof(OtherLoaderTestAssembly.OtherLoaderTestAssembly).Assembly
            )
            .Build();

        public static readonly Architecture VisibilityArchitecture = new ArchLoader()
            .LoadAssemblies(typeof(VisibilityNamespace.PublicClass).Assembly)
            .Build();

        public static readonly Architecture ArchUnitNETTestAssemblyArchitecture = new ArchLoader()
            .LoadAssemblies(typeof(Class1).Assembly)
            .Build();

        public static readonly Architecture FullArchUnitNETArchitecture = new ArchLoader()
            .LoadAssemblies(
                typeof(Architecture).Assembly,
                typeof(BaseClass).Assembly,
                typeof(Class1).Assembly,
                typeof(FailedArchRuleException).Assembly
            )
            .Build();

        public static readonly Architecture ArchUnitNETTestArchitectureWithDependencies =
            new ArchLoader()
                .LoadAssembliesIncludingDependencies(typeof(BaseClass).Assembly)
                .Build();

        public static readonly Architecture ArchUnitNETTestAssemblyArchitectureWithDependencies =
            new ArchLoader().LoadAssembliesIncludingDependencies(typeof(Class1).Assembly).Build();

        public static readonly Architecture FullArchUnitNETArchitectureWithDependencies =
            new ArchLoader()
                .LoadAssembliesIncludingDependencies(
                    typeof(Architecture).Assembly,
                    typeof(BaseClass).Assembly,
                    typeof(Class1).Assembly,
                    typeof(FailedArchRuleException).Assembly
                )
                .Build();
    }
}
