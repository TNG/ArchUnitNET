//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ArchUnitNET.Domain.Exceptions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain.Extensions
{
    public static class NamingExtensions
    {
        public static bool NameEndsWith(this IHasName cls, string pattern)
        {
            return cls.Name.ToLower().EndsWith(pattern.ToLower());
        }

        public static bool NameStartsWith(this IHasName cls, string pattern)
        {
            return cls.Name.ToLower().StartsWith(pattern.ToLower());
        }

        public static bool NameContains(this IHasName cls, string pattern)
        {
            return pattern != null && cls.Name.ToLower().Contains(pattern.ToLower());
        }

        public static bool NameMatches(this IHasName cls, string pattern, bool useRegularExpressions = false)
        {
            if (useRegularExpressions)
            {
                return pattern != null && Regex.IsMatch(cls.Name, pattern);
            }

            return cls.NameContains(pattern);
        }

        public static bool FullNameMatches(this IHasName cls, string pattern, bool useRegularExpressions = false)
        {
            if (useRegularExpressions)
            {
                return pattern != null && Regex.IsMatch(cls.FullName, pattern);
            }

            return cls.FullNameContains(pattern);
        }

        public static bool FullNameContains(this IHasName cls, string pattern)
        {
            return pattern != null && cls.FullName.ToLower().Contains(pattern.ToLower());
        }

        [NotNull]
        public static IEnumerable<TType> WhereNameIs<TType>(this IEnumerable<TType> source, string name)
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
                    + "alias to reference assemblies that have the same fully-qualified type names.");
            }

            return withFullName.FirstOrDefault();
        }
    }
}