/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.ArchitectureExceptions;
using ArchUnitNET.Domain;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Slice<string>> SlicedBy(this IEnumerable<IType> source, string fullName)
        {
            return source.GroupBy(type => type.FullName)
                .Select(sliceItems => new Slice<string>(sliceItems.Key, sliceItems.ToList()));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        [NotNull]
        public static IEnumerable<TType> WhereNameIs<TType>(this IEnumerable<TType> source, string name)
            where TType : IHasName
        {
            return source.Where(hasName => hasName.Name == name);
        }

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

        public static IEnumerable<string> ToStringEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select(obj => obj.ToString());
        }
    }
}