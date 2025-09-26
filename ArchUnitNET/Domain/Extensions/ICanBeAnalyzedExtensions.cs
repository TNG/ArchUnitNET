using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.Extensions
{
    internal static class ICanBeAnalyzedExtensions
    {
        internal static IEnumerable<object> GetAllAttributeArgumentValues(
            this ICanBeAnalyzed obj,
            Attribute attribute = null
        )
        {
            var attributeInstances =
                attribute == null
                    ? obj.AttributeInstances
                    : obj.AttributeInstances.Where(instance => instance.Type.Equals(attribute));
            return attributeInstances
                .SelectMany(instance => instance.AttributeArguments.Select(arg => arg.Value))
                .Select(value =>
                    value is ITypeInstance<IType> typeInstance ? typeInstance.Type : value
                );
        }

        internal static IEnumerable<(string, object)> GetAllNamedAttributeArgumentTuples(
            this ICanBeAnalyzed obj,
            Attribute attribute = null
        )
        {
            var attributeInstances =
                attribute == null
                    ? obj.AttributeInstances
                    : obj.AttributeInstances.Where(instance => instance.Type.Equals(attribute));
            return attributeInstances.SelectMany(instance =>
                instance
                    .AttributeArguments.OfType<AttributeNamedArgument>()
                    .Select(arg =>
                        (
                            arg.Name,
                            arg.Value is ITypeInstance<IType> typeInstance
                                ? typeInstance.Type
                                : arg.Value
                        )
                    )
            );
        }
    }
}
