using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;

// ReSharper disable UnusedTypeParameter

namespace ArchUnitNETTests.Domain.Dependencies.Types
{
    public class GenericInterfaceTests
    {
        private readonly Architecture _architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;

        private readonly Interface _genericInterface;
        private readonly Class _genericInterfaceImplementation;
        private readonly Class _genericType;

        public GenericInterfaceTests()
        {
            _genericInterface = _architecture.GetInterfaceOfType(typeof(IGenericInterface<>));
            _genericInterfaceImplementation = _architecture.GetClassOfType(
                typeof(GenericInterfaceImplementation)
            );
            _genericType = _architecture.GetClassOfType(typeof(GenericType));
        }

        [Fact]
        public void ClassImplementsGenericInterface()
        {
            var implementsInterfaceDependency = _genericInterfaceImplementation
                .Dependencies.OfType<ImplementsInterfaceDependency>()
                .First();
            var genericArgumentsOfImplementedInterface = implementsInterfaceDependency
                .TargetGenericArguments.Select(argument => argument.Type)
                .ToList();

            Assert.True(_genericInterfaceImplementation.ImplementsInterface(_genericInterface));
            Assert.Single(genericArgumentsOfImplementedInterface);
            Assert.Contains(_genericType, genericArgumentsOfImplementedInterface);
        }
    }

    public interface IGenericInterface<TGenericType> { }

    public class GenericType { }

    public class GenericInterfaceImplementation : IGenericInterface<GenericType> { }
}
