//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader
{
    internal static class MonoCecilAttributeExtensions
    {
        [NotNull]
        public static AttributeInstance CreateAttributeFromCustomAttribute(
            this CustomAttribute customAttribute,
            TypeFactory typeFactory
        )
        {
            var attributeTypeReference = customAttribute.AttributeType;
            var attributeType = typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                attributeTypeReference
            );
            var attribute = attributeType.Type is Class cls
                ? new Attribute(cls)
                : new Attribute(attributeType.Type, null, null);

            var attributeArguments = new List<AttributeArgument>();

            foreach (var constructorArgument in customAttribute.ConstructorArguments)
            {
                HandleAttributeArgument(
                    constructorArgument,
                    typeFactory,
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
                    typeFactory,
                    out var value,
                    out var type
                );
                attributeArguments.Add(new AttributeNamedArgument(name, value, type));
            }

            return new AttributeInstance(attribute, attributeArguments);
        }

        private static void HandleAttributeArgument(
            CustomAttributeArgument argument,
            TypeFactory typeFactory,
            out object value,
            out ITypeInstance<IType> type
        )
        {
            while (argument.Value is CustomAttributeArgument arg) //if would work too
            {
                argument = arg;
            }

            type = typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(argument.Type);

            if (argument.Value is IEnumerable<CustomAttributeArgument> attArgEnumerable)
            {
                value = (
                    from attArg in attArgEnumerable
                    select attArg.Value is TypeReference tr
                        ? typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(tr)
                        : attArg.Value
                ).ToArray();
            }
            else
            {
                value = argument.Value is TypeReference tr
                    ? typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(tr)
                    : argument.Value;
            }
        }
    }
}
