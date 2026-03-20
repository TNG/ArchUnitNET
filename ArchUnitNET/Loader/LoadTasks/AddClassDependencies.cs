using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddClassDependencies : ILoadTask
    {
        private readonly List<ITypeDependency> _dependencies;
        private readonly IType _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly DomainResolver _domainResolver;

        public AddClassDependencies(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver,
            List<ITypeDependency> dependencies
        )
        {
            _type = type;
            _typeDefinition = typeDefinition;
            _domainResolver = domainResolver;
            _dependencies = dependencies;
        }

        public void Execute()
        {
            AddInterfaceDependencies();
            AddMemberDependencies();
        }

        private void AddMemberDependencies()
        {
            _type.Members.ForEach(member =>
            {
                _dependencies.AddRange(member.Dependencies);
            });
        }

        private void AddInterfaceDependencies()
        {
            GetInterfacesImplementedByClass(_typeDefinition)
                .ForEach(target =>
                {
                    var targetType = _domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                        target
                    );
                    _dependencies.Add(new ImplementsInterfaceDependency(_type, targetType));
                });
        }

        private static IEnumerable<TypeReference> GetInterfacesImplementedByClass(
            TypeDefinition typeDefinition
        )
        {
            var baseType = typeDefinition.BaseType?.Resolve();
            var baseInterfaces =
                baseType != null
                    ? GetInterfacesImplementedByClass(baseType)
                    : new List<TypeReference>();

            return typeDefinition
                .Interfaces.Select(implementation => implementation.InterfaceType)
                .Concat(baseInterfaces);
        }
    }
}
