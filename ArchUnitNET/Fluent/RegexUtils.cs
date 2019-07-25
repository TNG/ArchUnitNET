/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Text.RegularExpressions;

namespace ArchUnitNET.Fluent
{
    public static class RegexUtils
    {
        private static readonly Regex BackingFieldRegex = new Regex(@"<(.+)>" + StaticConstants.BackingField);
        private static readonly Regex GetMethodPropertyMemberRegex = new Regex(@"get_(.+)\(\)");
        private static readonly Regex SetMethodPropertyMemberRegex = new Regex(@"set_(.+)\((.+)\)");


        public static string MatchFieldName(string fieldName)
        {
            var match = BackingFieldRegex.Match(fieldName);
            if (!match.Success)
            {
                return null;
            }

            var matchingPropertyName = match.Groups[1].Value;
            return matchingPropertyName;
        }

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