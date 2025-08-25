using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain.Exceptions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain.Extensions
{
    public static class NamingExtensions
    {
        public static bool NameEquals(this IHasName cls, string name)
        {
            return string.Equals(cls.Name, name, StringComparison.OrdinalIgnoreCase);
        }

        public static bool NameMatches(this IHasName cls, string pattern)
        {
            return pattern != null && Regex.IsMatch(cls.Name, pattern);
        }

        public static bool NameEndsWith(
            this IHasName cls,
            string pattern,
            StringComparison stringComparison = StringComparison.CurrentCulture
        )
        {
            return cls.Name.EndsWith(pattern, stringComparison);
        }

        public static bool NameStartsWith(
            this IHasName cls,
            string pattern,
            StringComparison stringComparison = StringComparison.CurrentCulture
        )
        {
            return cls.Name.StartsWith(pattern, stringComparison);
        }

        public static bool NameContains(
            this IHasName cls,
            string pattern,
            StringComparison stringComparison = StringComparison.Ordinal
        )
        {
            return cls.Name.IndexOf(pattern, stringComparison) >= 0;
        }

        public static bool FullNameEquals(this IHasName cls, string fullName)
        {
            return string.Equals(cls.FullName, fullName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool FullNameMatches(this IHasName cls, string pattern)
        {
            return pattern != null && Regex.IsMatch(cls.FullName, pattern);
        }

        public static bool FullNameEndsWith(this IHasName cls, string pattern)
        {
            return cls.FullName.EndsWith(pattern, StringComparison.OrdinalIgnoreCase);
        }

        public static bool FullNameStartsWith(this IHasName cls, string pattern)
        {
            return cls.FullName.StartsWith(pattern, StringComparison.OrdinalIgnoreCase);
        }

        public static bool FullNameContains(this IHasName cls, string pattern)
        {
            return pattern != null && cls.FullName.ToLower().Contains(pattern.ToLower());
        }

        public static bool AssemblyQualifiedNameEquals(
            this IHasAssemblyQualifiedName cls,
            string assemblyQualifiedName
        )
        {
            return string.Equals(
                cls.AssemblyQualifiedName,
                assemblyQualifiedName,
                StringComparison.OrdinalIgnoreCase
            );
        }

        public static bool AssemblyQualifiedNameMatches(
            this IHasAssemblyQualifiedName cls,
            string pattern
        )
        {
            return pattern != null && Regex.IsMatch(cls.AssemblyQualifiedName, pattern);
        }

        public static bool AssemblyQualifiedNameEndsWith(
            this IHasAssemblyQualifiedName cls,
            string pattern
        )
        {
            return cls.AssemblyQualifiedName.EndsWith(pattern, StringComparison.OrdinalIgnoreCase);
        }

        public static bool AssemblyQualifiedNameStartsWith(
            this IHasAssemblyQualifiedName cls,
            string pattern
        )
        {
            return cls.AssemblyQualifiedName.StartsWith(
                pattern,
                StringComparison.OrdinalIgnoreCase
            );
        }

        public static bool AssemblyQualifiedNameContains(
            this IHasAssemblyQualifiedName cls,
            string pattern
        )
        {
            return pattern != null
                && cls.AssemblyQualifiedName.ToLower().Contains(pattern.ToLower());
        }

        [NotNull]
        public static IEnumerable<TType> WhereNameIs<TType>(
            this IEnumerable<TType> source,
            string name
        )
            where TType : IHasName
        {
            return source.Where(hasName => hasName.Name == name);
        }

        [CanBeNull]
        public static TType WhereFullNameIs<TType>(this IEnumerable<TType> source, string fullName)
            where TType : IHasName
        {
            var withFullName = source.Where(type => type.FullName == fullName).ToList();

            if (withFullName.Count > 1)
            {
                throw new MultipleOccurrencesInSequenceException(
                    $"Full name {fullName} found multiple times in provided types. Please use extern "
                        + "alias to reference assemblies that have the same fully-qualified type names."
                );
            }

            return withFullName.FirstOrDefault();
        }

        [CanBeNull]
        public static TType WhereAssemblyQualifiedNameIs<TType>(
            this IEnumerable<TType> source,
            string assemblyQualifiedName
        )
            where TType : IHasAssemblyQualifiedName
        {
            return source.FirstOrDefault(type =>
                type.AssemblyQualifiedName == assemblyQualifiedName
            );
        }
    }
}
