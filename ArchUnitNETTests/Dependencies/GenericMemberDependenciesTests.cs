using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class GenericMemberDependenciesTests
    {
        [Fact]
        public void GenericArgumentsFromMethodCallTest()
        {
            var architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
            var classWithGenericMethodCall = architecture.GetClassOfType(
                typeof(ClassWithGenericMethodCall)
            );
            var genericArgumentClass = architecture.GetClassOfType(
                typeof(GenericArgumentClass)
            );
            var methodMember = (MethodMember)classWithGenericMethodCall.Members
                .WhereNameIs("OuterFunc()")
                .First();
            Assert.Contains(methodMember.Dependencies, dependency => dependency is MethodSignatureDependency bodyTypeMemberDependency &&
                                                                     bodyTypeMemberDependency.Target.Equals(genericArgumentClass));
        }
    }

    public class ClassWithGenericMethodCall
    {
        public void OuterFunc()
        {
            LocalFunc<GenericArgumentClass>();

            void LocalFunc<T>() { }
        }
    }
}
