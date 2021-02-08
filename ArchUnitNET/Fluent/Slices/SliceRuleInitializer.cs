//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Identifiers;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceRuleInitializer
    {
        private readonly SliceRuleCreator _ruleCreator;

        public SliceRuleInitializer(SliceRuleCreator ruleCreator)
        {
            _ruleCreator = ruleCreator;
        }

        /// <summary>
        /// </summary>
        /// <param name="pattern">
        ///     check https://www.archunit.org/userguide/html/000_Index.html#_slices for examples for pattern
        ///     usage
        /// </param>
        /// <returns></returns>
        public GivenSlices Matching(string pattern)
        {
            _ruleCreator.SetSliceAssignment(new SliceAssignment(t => AssignFunc(t, pattern),
                "matching \"" + pattern + "\""));
            return new GivenSlices(_ruleCreator);
        }

        private static SliceIdentifier AssignFunc(IType type, string pattern)
        {
            var containsSingleAsterisk = pattern.Contains("(*)");
            var containsDoubleAsterisk = pattern.Contains("(**)");
            var indexOfAsteriskInPattern = pattern.IndexOf("(*", StringComparison.Ordinal);
            if (!containsSingleAsterisk && !containsDoubleAsterisk)
            {
                throw new ArgumentException("Patterns for Slices have to contain (*) or (**).");
            }

            if (indexOfAsteriskInPattern != pattern.LastIndexOf("(*", StringComparison.Ordinal))
            {
                throw new ArgumentException("Patterns for Slices can only contain (*) or (**) once.");
            }

            var namespc = type.Namespace.FullName;
            var slicePrefix = pattern.Remove(indexOfAsteriskInPattern);
            var slicePostfix = pattern.Substring(pattern.IndexOf("*)", StringComparison.Ordinal) + 2);

            if (slicePrefix.StartsWith("."))
            {
                slicePrefix = slicePrefix.Substring(1);
                if (!namespc.Contains(slicePrefix))
                {
                    return SliceIdentifier.Ignore();
                }
            }
            else if (!namespc.StartsWith(slicePrefix))
            {
                return SliceIdentifier.Ignore();
            }

            if (slicePostfix.EndsWith("."))
            {
                slicePostfix = slicePostfix.Remove(slicePostfix.Length - 1);
                if (!namespc.Substring(namespc.IndexOf(slicePrefix, StringComparison.Ordinal) + slicePrefix.Length)
                    .Contains(slicePostfix))
                {
                    return SliceIdentifier.Ignore();
                }
            }
            else if (!namespc.EndsWith(slicePostfix))
            {
                return SliceIdentifier.Ignore();
            }

            var sliceString = namespc;

            if (slicePrefix != "")
            {
                sliceString = namespc
                    .Substring(namespc.IndexOf(slicePrefix, StringComparison.Ordinal) + slicePrefix.Length)
                    .TrimStart('.');
            }

            if (!sliceString.Contains(slicePostfix))
            {
                throw new ArgumentException("\"" + type.FullName +
                                            "\" is not clearly assignable to a slice with the pattern: \"" +
                                            pattern + "\"");
            }

            if (slicePostfix == "")
            {
                if (containsSingleAsterisk && sliceString.IndexOf(".", StringComparison.Ordinal) >= 0)
                {
                    sliceString = sliceString.Remove(sliceString.IndexOf(".", StringComparison.Ordinal));
                }
            }
            else
            {
                sliceString = sliceString.Remove(sliceString.IndexOf(slicePostfix, StringComparison.Ordinal));
                if (containsSingleAsterisk && sliceString.Trim('.').Contains("."))
                {
                    return SliceIdentifier.Ignore();
                }
            }

            return SliceIdentifier.Of(sliceString);
        }
    }
}