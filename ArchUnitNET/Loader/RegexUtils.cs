//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Text.RegularExpressions;

namespace ArchUnitNET.Loader
{
    public static class RegexUtils
    {
        private static readonly Regex GetMethodPropertyMemberRegex = new Regex(@"get_(.+)\(\)");
        private static readonly Regex SetMethodPropertyMemberRegex = new Regex(@"set_(.+)\((.+)\)");

        public static string MatchGetPropertyName(string methodName)
        {
            var match = GetMethodPropertyMemberRegex.Match(methodName);
            if (!match.Success)
            {
                return null;
            }

            var accessedPropertyName = match.Groups[1].Value;
            return accessedPropertyName;
        }

        public static string MatchSetPropertyName(string methodName)
        {
            var match = SetMethodPropertyMemberRegex.Match(methodName);
            if (!match.Success)
            {
                return null;
            }

            var accessedPropertyName = match.Groups[1].Value;
            return accessedPropertyName;
        }

        public static bool MatchNamespaces(string goalNamespace, string currentNamespace)
        {
            return goalNamespace == null || currentNamespace.StartsWith(goalNamespace);
        }
    }
}