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
    }
}
