using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Xunit;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedVariable
// ReSharper disable NotAccessedField.Local

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class MethodCallDependencyTests
    {
        private readonly Architecture _architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Class _classWithConstructors;
        private readonly MethodMember _methodAMember;
        private readonly MethodMember _methodBMember;

        public MethodCallDependencyTests()
        {
            _classWithConstructors = _architecture.GetClassOfType(typeof(ClassWithConstructors));
            _methodAMember = _architecture
                .GetClassOfType(typeof(ClassWithMethodA))
                .GetMethodMembersWithName(nameof(ClassWithMethodA.MethodA).BuildMethodMemberName())
                .FirstOrDefault();
            _methodBMember = _architecture
                .GetClassOfType(typeof(ClassWithMethodB))
                .GetMethodMembersWithName(nameof(ClassWithMethodB.MethodB).BuildMethodMemberName())
                .FirstOrDefault();
        }

        [Fact]
        public void UsesMemberGenericArguments()
        {
            var architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
            var originClass = architecture.GetClassOfType(
                typeof(ClassWithMethodWithGenericMethodArguments)
            );
            var originMember = originClass
                .GetMembersWithName(
                    nameof(ClassWithMethodWithGenericMethodArguments.Method).BuildMethodMemberName()
                )
                .Single();
            var targetClass = architecture.GetClassOfType(typeof(ClassWithGenericMethodArguments));
            var targetMember = targetClass
                .GetMethodMembersWithName(
                    nameof(ClassWithGenericMethodArguments.Method).BuildMethodMemberName()
                )
                .Single();
            var memberGenericArgument = new GenericArgument(
                new TypeInstance<IType>(
                    architecture.GetClassOfType(typeof(ClassForGenericArgument))
                )
            );
            var methodCallDependency = new MethodCallDependency(
                targetMember,
                new MethodMemberInstance(
                    new TypeInstance<IType>(targetMember.DeclaringType, []),
                    targetMember,
                    [memberGenericArgument]
                )
            );

            Assert.Contains(
                memberGenericArgument,
                methodCallDependency.TargetMemberGenericArguments
            );
        }

        [Theory]
        [ClassData(typeof(MethodDependencyTestBuild.ConstructorTestData))]
        public void ConstructorsAddedToClass(Class classWithConstructors)
        {
            //Setup
            var constructorMembers = classWithConstructors.GetConstructors();

            //Assert
            constructorMembers.ForEach(constructor =>
            {
                Assert.Contains(constructor, classWithConstructors.Constructors);
            });
        }

        [Theory]
        [ClassData(typeof(MethodDependencyTestBuild.MethodCallDependencyTestData))]
        public void MethodCallDependenciesAreFound(
            IMember originMember,
            MethodCallDependency expectedDependency
        )
        {
            Assert.True(originMember.HasMemberDependency(expectedDependency));
            Assert.Contains(expectedDependency, originMember.GetMethodCallDependencies());
        }

        [SkipInReleaseBuildTheory]
        [ClassData(typeof(MethodDependencyTestBuild.MethodCallDependencyInAsyncMethodTestData))]
        public void MethodCallDependenciesAreFoundInAsyncMethod(
            IMember originMember,
            MethodCallDependency expectedDependency
        )
        {
            Assert.True(originMember.HasMemberDependency(expectedDependency));
            Assert.Contains(expectedDependency, originMember.GetMethodCallDependencies());
        }

        [Theory]
        [ClassData(
            typeof(MethodDependencyTestBuild.MethodCallGenericConstructorArgumentsDependencyTestData)
        )]
        public void MethodCallGenericConstructorArgumentDependenciesAreFound(
            IMember originMember,
            MethodCallDependency expectedDependency
        )
        {
            Assert.Contains(
                originMember.GetMethodCallDependencies(),
                methodCallDependency =>
                    methodCallDependency.TargetGenericArguments.SequenceEqual(
                        expectedDependency.TargetGenericArguments
                    )
            );
        }

        [Theory]
        [ClassData(
            typeof(MethodDependencyTestBuild.MethodCallGenericMethodArgumentsDependencyTestData)
        )]
        public void MethodCallGenericMethodArgumentDependenciesAreFound(
            IMember originMember,
            MethodCallDependency expectedDependency
        )
        {
            Assert.Contains(
                originMember.GetMethodCallDependencies(),
                methodCallDependency =>
                    methodCallDependency.TargetMemberGenericArguments.SequenceEqual(
                        expectedDependency.TargetMemberGenericArguments
                    )
            );
        }
    }

    public class ClassWithMethodA
    {
        public static void MethodA()
        {
            var classWithMethodB = new ClassWithMethodB();
            ClassWithMethodB.MethodB();
        }
    }

    public class ClassWithMethodB
    {
        public static void MethodB()
        {
            var classWithMethodA = new ClassWithMethodA();
            ClassWithMethodA.MethodA();
        }
    }

    public class ClassWithMethodAAsync
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public static async void MethodAAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var classWithMethodB = new ClassWithMethodB();
            ClassWithMethodB.MethodB();
        }
    }

    public class ClassWithMethodWithGenericConstructorArguments
    {
        public static void Method()
        {
            var classForGenericArguments =
                new ClassWithGenericConstructorArguments<ClassForGenericArgument>();
        }
    }

    // ReSharper disable once UnusedTypeParameter
    public class ClassWithGenericConstructorArguments<T>;

    public class ClassWithMethodWithGenericMethodArguments
    {
        public static void Method()
        {
            ClassWithGenericMethodArguments.Method<ClassForGenericArgument>();
        }
    }

    public class ClassWithGenericMethodArguments
    {
        // ReSharper disable once UnusedTypeParameter
        public static void Method<T>() { }
    }

    public class ClassForGenericArgument;

    public class ClassWithConstructors
    {
        private FieldType _fieldTest;
        private FieldType _privateFieldTest;

        public ClassWithConstructors()
            : this(new FieldType()) { }

        private ClassWithConstructors(FieldType fieldTest)
            : this(fieldTest, fieldTest) { }

        private ClassWithConstructors(FieldType fieldTest, FieldType privateFieldTest)
        {
            _fieldTest = fieldTest;
            _privateFieldTest = privateFieldTest;
        }
    }
}
