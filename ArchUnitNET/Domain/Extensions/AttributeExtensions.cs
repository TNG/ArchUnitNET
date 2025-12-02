using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAttribute(this IHasAttributes a, string fullName)
        {
            return a.Attributes.Any(attribute => attribute.FullNameEquals(fullName));
        }

        public static bool HasAttributeMatching(this IHasAttributes a, string pattern)
        {
            return a.Attributes.Any(attribute => attribute.FullNameMatches(pattern));
        }

        public static bool OnlyHasAttributes(this IHasAttributes a, string name)
        {
            return a.Attributes.IsNullOrEmpty()
                || a.Attributes.All(attribute => attribute.FullNameEquals(name));
        }

        public static bool OnlyHasAttributesMatching(this IHasAttributes a, string pattern)
        {
            return a.Attributes.IsNullOrEmpty()
                || a.Attributes.All(attribute => attribute.FullNameMatches(pattern));
        }

        internal static IEnumerable<object> GetAllAttributeArgumentValues(
            this AttributeInstance instance
        ) =>
            instance
                .AttributeArguments.Select(arg => arg.Value)
                .Select(value =>
                    value is ITypeInstance<IType> typeInstance ? typeInstance.Type : value
                );

        internal static IEnumerable<(string, object)> GetAllNamedAttributeArgumentTuples(
            this AttributeInstance instance
        ) =>
            instance
                .AttributeArguments.OfType<AttributeNamedArgument>()
                .Select(arg =>
                    (
                        arg.Name,
                        arg.Value is ITypeInstance<IType> typeInstance
                            ? typeInstance.Type
                            : arg.Value
                    )
                );
    }
}
