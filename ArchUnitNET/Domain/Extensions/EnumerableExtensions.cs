using System;
using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, params Action<T>[] actions)
        {
            foreach (var element in source)
            {
                foreach (var action in actions)
                {
                    action(element);
                }
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static string FormatDescription<T>(
            this IEnumerable<T> source,
            string emptyDescription,
            string singleDescription,
            string multipleDescription,
            Func<T, string> elementDescription = null
        )
        {
            var list = source as IList<T> ?? source.ToList();
            elementDescription = elementDescription ?? (element => $"\"{element}\"");
            switch (list.Count)
            {
                case 0:
                    return emptyDescription;
                case 1:
                    return $"{singleDescription} {string.Join(" and ", list.Select(elementDescription))}";
                default:
                    return $"{multipleDescription} {string.Join(" and ", list.Select(elementDescription))}";
            }
        }

        internal static IEnumerable<object> ResolveAttributeArguments(
            this IEnumerable<object> objects,
            Architecture architecture
        )
        {
            return objects.Select(obj =>
                obj is Type type ? architecture.GetITypeOfType(type) : obj
            );
        }

        internal static IEnumerable<(string, object)> ResolveNamedAttributeArgumentTuples(
            this IEnumerable<(string, object)> namedArguments,
            Architecture architecture
        )
        {
            return namedArguments.Select(arg =>
                (arg.Item1, arg.Item2 is Type type ? architecture.GetITypeOfType(type) : arg.Item2)
            );
        }
    }
}
