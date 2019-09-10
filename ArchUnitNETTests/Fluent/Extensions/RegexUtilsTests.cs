/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Dependencies.Members;
using Xunit;

namespace ArchUnitNETTests.Fluent.Extensions
{
    public class RegexUtilsTest
    {
        public RegexUtilsTest()
        {
            var propertyClass = Architecture.GetClassOfType(typeof(BackingFieldExamples));
            _autoPropertyMember = propertyClass.GetPropertyMembersWithName("AutoProperty").Single();
            _expectedBackingFieldName = BuildExpectedBackingFieldName(_autoPropertyMember);
            _expectedGetMethodName = BuildExpectedGetMethodName(_autoPropertyMember);
            _expectedGetMethodFullName = BuildExpectedGetMethodFullName(_autoPropertyMember);
            _expectedSetMethodName = BuildExpectedSetMethodName(_autoPropertyMember, _autoPropertyMember.DeclaringType);
        }

        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private readonly PropertyMember _autoPropertyMember;
        private readonly string _expectedBackingFieldName;
        private readonly string _expectedGetMethodName;
        private readonly string _expectedGetMethodFullName;
        private readonly string _expectedSetMethodName;
        private readonly string _nonMatchEmpty = string.Empty;
        private static readonly string _nonMatch = "Not expected to match.";

        private static string BuildExpectedBackingFieldName(PropertyMember propertyMember)
        {
            return propertyMember.DeclaringType.FullName + " " + propertyMember.DeclaringType.Name + "::<"
                   + propertyMember.Name + ">" + StaticConstants.BackingField;
        }

        private static string BuildExpectedGetMethodName(PropertyMember propertyMember,
            params IType[] parameterTypes)
        {
            var builder = new StringBuilder();
            builder.Append("get_");
            builder.Append(propertyMember.Name);
            builder = AddParameterTypesToMethodName(builder, parameterTypes);
            return builder.ToString();
        }

        private static string BuildExpectedSetMethodName(PropertyMember propertyMember,
            params IType[] parameterTypes)
        {
            var builder = new StringBuilder();
            builder.Append("set_");
            builder.Append(propertyMember.Name);
            builder = AddParameterTypesToMethodName(builder, parameterTypes);
            return builder.ToString();
        }

        private static string BuildExpectedGetMethodFullName(PropertyMember propertyMember,
            params IType[] parameterTypes)
        {
            var builder = new StringBuilder();
            builder.Append(propertyMember.DeclaringType.FullName);
            builder.Append(" ");
            builder.Append(propertyMember.DeclaringType.Name);
            builder.Append("::get_");
            builder.Append(propertyMember.Name);
            builder = AddParameterTypesToMethodName(builder, parameterTypes);
            return builder.ToString();
        }

        private static StringBuilder AddParameterTypesToMethodName(StringBuilder nameBuilder,
            params IType[] parameterTypeNames)
        {
            nameBuilder.Append("(");
            for (var index = 0; index < parameterTypeNames.Length; ++index)
            {
                if (index > 0)
                {
                    nameBuilder.Append(",");
                }

                nameBuilder.Append(parameterTypeNames[index].FullName);
            }

            nameBuilder.Append(")");
            return nameBuilder;
        }

        [Fact]
        public void BackingFieldRegexMatchAsExpected()
        {
            Assert.Equal(_autoPropertyMember.Name,
                RegexUtils.MatchFieldName(_expectedBackingFieldName));
        }

        [Fact]
        public void BackingFieldRegexRecognizesNonMatch()
        {
            Assert.Null(RegexUtils.MatchFieldName(_nonMatchEmpty));
            Assert.Null(RegexUtils.MatchFieldName(_nonMatch));
        }

        [Fact]
        public void GetMethodFullNameRegexRecognizesNonMatch()
        {
            Assert.Null(RegexUtils.MatchGetPropertyName(_nonMatchEmpty));
            Assert.Null(RegexUtils.MatchGetPropertyName(_nonMatch));
        }

        [Fact]
        public void GetMethodNameRegexRecognizesNonMatch()
        {
            Assert.Null(RegexUtils.MatchGetPropertyName(_nonMatchEmpty));
            Assert.Null(RegexUtils.MatchGetPropertyName(_nonMatch));
        }

        [Fact]
        public void GetMethodPropertyMemberFullNameRegexMatchAsExpected()
        {
            Assert.Equal(_autoPropertyMember.Name,
                RegexUtils.MatchGetPropertyName(_expectedGetMethodFullName));
        }

        [Fact]
        public void GetMethodPropertyMemberRegexMatchAsExpected()
        {
            Assert.Equal(_autoPropertyMember.Name,
                RegexUtils.MatchGetPropertyName(_expectedGetMethodName));
        }

        [Fact]
        public void SetMethodFullNameRegexRecognizesNonMatch()
        {
            Assert.Null(RegexUtils.MatchSetPropertyName(_nonMatchEmpty));
            Assert.Null(RegexUtils.MatchSetPropertyName(_nonMatch));
        }

        [Fact]
        public void SetMethodNameRegexRecognizesNonMatch()
        {
            Assert.Null(RegexUtils.MatchSetPropertyName(_nonMatchEmpty));
            Assert.Null(RegexUtils.MatchSetPropertyName(_nonMatch));
        }


        [Fact]
        public void SetMethodPropertyMemberRegexMatchAsExpected()
        {
            Assert.Equal(_autoPropertyMember.Name,
                RegexUtils.MatchSetPropertyName(_expectedSetMethodName));
        }
    }
}