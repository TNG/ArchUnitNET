using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Loader
{
    internal static class MonoCecilAttributeExtensions
    {
        [NotNull]
        public static AttributeInstance CreateAttributeFromCustomAttribute(
            this CustomAttribute customAttribute,
            DomainResolver domainResolver
        )
        {
            var attributeTypeReference = customAttribute.AttributeType;
            var attributeType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                attributeTypeReference
            );
            var attribute = attributeType.Type as Attribute;
            if (
                attribute == null
                && (attributeType.Type is UnavailableType || attributeType.Type is Class)
            )
            {
                attribute = new Attribute(attributeType.Type, null, null);
            }
            if (attribute == null)
            {
                throw new ArgumentException(
                    $"Attribute type {attributeType.Type.FullName} is not an attribute."
                );
            }

            var attributeArguments = new List<AttributeArgument>();

            foreach (var constructorArgument in customAttribute.ConstructorArguments)
            {
                HandleAttributeArgument(
                    constructorArgument,
                    domainResolver,
                    out var value,
                    out var type
                );
                attributeArguments.Add(new AttributeArgument(value, type));
            }

            foreach (var namedArgument in customAttribute.Fields.Concat(customAttribute.Properties))
            {
                var name = namedArgument.Name;
                HandleAttributeArgument(
                    namedArgument.Argument,
                    domainResolver,
                    out var value,
                    out var type
                );
                attributeArguments.Add(new AttributeNamedArgument(name, value, type));
            }

            return new AttributeInstance(attribute, attributeArguments);
        }

        private static void HandleAttributeArgument(
            CustomAttributeArgument argument,
            DomainResolver domainResolver,
            out object value,
            out ITypeInstance<IType> type
        )
        {
            while (argument.Value is CustomAttributeArgument arg) //if would work too
            {
                argument = arg;
            }

            type = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(argument.Type);

            if (argument.Value is IEnumerable<CustomAttributeArgument> attArgEnumerable)
            {
                value = (
                    from attArg in attArgEnumerable
                    select attArg.Value is TypeReference tr
                        ? domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(tr)
                        : attArg.Value
                ).ToArray();
            }
            else
            {
                value = argument.Value is TypeReference tr
                    ? domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(tr)
                    : argument.Value;
            }
        }
    }
}
