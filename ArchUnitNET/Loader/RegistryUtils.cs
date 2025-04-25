using System;
using System.Collections.Generic;

namespace ArchUnitNET.Loader
{
    internal static class RegistryUtils
    {
        public static T GetFromDictOrCreateAndAdd<T, TK>(
            TK key,
            Dictionary<TK, T> dict,
            Func<TK, T> createFunc
        )
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }

            value = createFunc(key);
            dict.Add(key, value);

            return value;
        }
    }
}
