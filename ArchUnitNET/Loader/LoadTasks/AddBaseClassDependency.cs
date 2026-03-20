using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddBaseClassDependency : ILoadTask
    {
        private readonly IType _cls;
        private readonly Type _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly DomainResolver _domainResolver;

        public AddBaseClassDependency(
            IType cls,
            Type type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            _cls = cls;
            _type = type;
            _typeDefinition = typeDefinition;
            _domainResolver = domainResolver;
        }

        public void Execute()
        {
            var typeDefinitionBaseType = _typeDefinition?.BaseType;

            if (typeDefinitionBaseType == null)
            {
                return;
            }

            var baseType = _domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                typeDefinitionBaseType
            );
            if (!(baseType.Type is Class baseClass))
            {
                return;
            }

            var dependency = new InheritsBaseClassDependency(
                _cls,
                new TypeInstance<Class>(
                    baseClass,
                    baseType.GenericArguments,
                    baseType.ArrayDimensions
                )
            );
            _type.Dependencies.Add(dependency);
        }
    }
}
