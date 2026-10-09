using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using OptimizedAssembly;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Dependencies
{
    /// <summary>
    /// Async and iterator methods in an optimized build. There the compiler emits the async
    /// state machine as a struct, which is initialized in place rather than created with
    /// <c>newobj</c>, so it has to be found through the method's state machine attribute.
    /// </summary>
    public class OptimizedStateMachineDependenciesTests
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.OptimizedArchitecture;

        private readonly MethodMember _calledMethod;

        public OptimizedStateMachineDependenciesTests()
        {
            _calledMethod = Architecture
                .GetClassOfType(typeof(CalledClass))
                .GetMethodMembersWithName(nameof(CalledClass.CalledMethod) + "()")
                .First();
        }

        [Fact]
        public void AsyncStateMachineIsAStruct()
        {
            var stateMachineType = typeof(ClassWithAsyncMethod)
                .GetMethod(nameof(ClassWithAsyncMethod.AsyncMethod))
                .GetCustomAttribute<AsyncStateMachineAttribute>()
                .StateMachineType;

            Assert.True(stateMachineType.IsValueType);
        }

        [Fact]
        public void AsyncMethodBodyDependenciesAreFound()
        {
            var asyncMethod = Architecture
                .GetClassOfType(typeof(ClassWithAsyncMethod))
                .GetMethodMembersWithName(nameof(ClassWithAsyncMethod.AsyncMethod) + "()")
                .First();

            Assert.Contains(_calledMethod, asyncMethod.GetCalledMethods());

            var rule = Classes()
                .That()
                .HaveFullName(typeof(ClassWithAsyncMethod).FullName)
                .Should()
                .NotDependOnAny(typeof(CalledClass));
            Assert.False(rule.HasNoViolations(Architecture));
        }

        [Fact]
        public void IteratorMethodBodyDependenciesAreFound()
        {
            var iteratorMethod = Architecture
                .GetClassOfType(typeof(ClassWithIteratorMethod))
                .GetMethodMembersWithName(nameof(ClassWithIteratorMethod.IteratorMethod) + "()")
                .First();

            Assert.Contains(_calledMethod, iteratorMethod.GetCalledMethods());

            var rule = Classes()
                .That()
                .HaveFullName(typeof(ClassWithIteratorMethod).FullName)
                .Should()
                .NotDependOnAny(typeof(CalledClass));
            Assert.False(rule.HasNoViolations(Architecture));
        }
    }
}
