using System;
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
        public void GenericArgumentsFromMethodCallLocalFuncTest()
        {
            var architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
            var classWithGenericMethodCall = architecture.GetClassOfType(
                typeof(ClassWithGenericMethodCall)
            );
            var genericArgumentClass = architecture.GetClassOfType(typeof(GenericArgumentClass));
            var methodMember = (MethodMember)
                classWithGenericMethodCall.Members.WhereNameIs("OuterFunc()").First();
            Assert.Contains(
                methodMember.Dependencies,
                dependency =>
                    dependency is BodyTypeMemberDependency bodyTypeMemberDependency
                    && bodyTypeMemberDependency.Target.Equals(genericArgumentClass)
            );
        }

        [Fact]
        public void GenericArgumentsFromMethodCallTest()
        {
            var architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
            var classWithGenericMethodCall = architecture.GetClassOfType(
                typeof(ClassWithGenericMethodCall)
            );
            var genericArgumentClass = architecture.GetClassOfType(
                typeof(OtherGenericArgumentClass)
            );
            var methodMember = (MethodMember)
                classWithGenericMethodCall.Members.WhereNameIs("OuterFunc()").First();
            Assert.Contains(
                methodMember.Dependencies,
                dependency =>
                    dependency is BodyTypeMemberDependency bodyTypeMemberDependency
                    && bodyTypeMemberDependency.Target.Equals(genericArgumentClass)
            );
        }
    }

    internal class ClassWithGenericMethodCall
    {
        public void OuterFunc()
        {
            LocalFunc<GenericArgumentClass>();
            PrivateFunc<OtherGenericArgumentClass>();

            void LocalFunc<T>()
            {
                throw new NotImplementedException($"Called with type {typeof(T)}");
            }
        }

        private static void PrivateFunc<T>()
        {
            throw new NotImplementedException($"Called with type {typeof(T)}");
        }
    }

    internal class OtherGenericArgumentClass { }
}
