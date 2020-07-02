//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System;
using ArchUnitNET.Domain;

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
        /// <param name="pattern">check https://www.archunit.org/userguide/html/000_Index.html#_slices for examples for pattern usage</param>
        /// <returns></returns>
        public GivenSlices Matching(string pattern)
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

            SliceIdentifier AssignFunc(IType type)
            {
                var namespc = type.Namespace.FullName;
                var firstPart = pattern.Remove(indexOfAsteriskInPattern);
                var secondPart = pattern.Substring(pattern.IndexOf("*)", StringComparison.Ordinal) + 2);

                if (firstPart.StartsWith("."))
                {
                    firstPart = firstPart.Substring(1);
                    if (!namespc.Contains(firstPart))
                    {
                        return SliceIdentifier.Ignore();
                    }
                }
                else if (!namespc.StartsWith(firstPart))
                {
                    return SliceIdentifier.Ignore();
                }

                if (secondPart.EndsWith("."))
                {
                    secondPart = secondPart.Remove(secondPart.Length - 1);
                    if (!namespc.Substring(namespc.IndexOf(firstPart, StringComparison.Ordinal) + firstPart.Length).Contains(secondPart))
                    {
                        return SliceIdentifier.Ignore();
                    }
                }
                else if (!namespc.EndsWith(secondPart))
                {
                    return SliceIdentifier.Ignore();
                }

                var sliceString = namespc;

                if (firstPart != "")
                {
                    sliceString = namespc.Substring(namespc.IndexOf(firstPart, StringComparison.Ordinal) + firstPart.Length).TrimStart('.');
                }

                if (!sliceString.Contains(secondPart))
                {
                    throw new ArgumentException("\"" + type.FullName + "\" is not clearly assignable to a slice with the pattern: \"" + pattern + "\"");
                }

                if (secondPart == "")
                {
                    if (containsSingleAsterisk && sliceString.IndexOf(".", StringComparison.Ordinal) >= 0)
                    {
                        sliceString = sliceString.Remove(sliceString.IndexOf(".", StringComparison.Ordinal));
                    }
                }
                else
                {
                    sliceString = sliceString.Remove(sliceString.IndexOf(secondPart, StringComparison.Ordinal));
                    if (containsSingleAsterisk && sliceString.Trim('.').Contains("."))
                    {
                        return SliceIdentifier.Ignore();
                    }
                }

                return SliceIdentifier.Of(sliceString);
            }

            _ruleCreator.SetSliceAssignment(new SliceAssignment(AssignFunc, "matching \"" + pattern + "\""));
            return new GivenSlices(_ruleCreator);
        }
    }
}