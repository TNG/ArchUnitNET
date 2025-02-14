using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Domain.Dependencies.Members;
using Xunit;

namespace ArchUnitNETTests.Loader
{
    public class RegexUtilsTest
    {
        private static readonly Architecture Architecture =
            StaticTestArchitectures.ArchUnitNETTestArchitecture;
        private static readonly string _nonMatch = "Not expected to match.";
        private readonly PropertyMember _autoPropertyMember;
        private readonly string _expectedGetMethodFullName;
        private readonly string _expectedGetMethodName;
        private readonly string _expectedSetMethodName;
        private readonly string _nonMatchEmpty = string.Empty;

        public RegexUtilsTest()
        {
            var propertyClass = Architecture.GetClassOfType(typeof(BackingFieldExamples));
            _autoPropertyMember = propertyClass.GetPropertyMembersWithName("AutoProperty").Single();
            _expectedGetMethodName = BuildExpectedGetMethodName(_autoPropertyMember);
            _expectedGetMethodFullName = BuildExpectedGetMethodFullName(_autoPropertyMember);
            _expectedSetMethodName = BuildExpectedSetMethodName(
                _autoPropertyMember,
                _autoPropertyMember.DeclaringType
            );
        }

        private static string BuildExpectedGetMethodName(
            PropertyMember propertyMember,
            params IType[] parameterTypes
        )
        {
            var builder = new StringBuilder();
            builder.Append("get_");
            builder.Append(propertyMember.Name);
            builder = AddParameterTypesToMethodName(builder, parameterTypes);
            return builder.ToString();
        }

        private static string BuildExpectedSetMethodName(
            PropertyMember propertyMember,
            params IType[] parameterTypes
        )
        {
            var builder = new StringBuilder();
            builder.Append("set_");
            builder.Append(propertyMember.Name);
            builder = AddParameterTypesToMethodName(builder, parameterTypes);
            return builder.ToString();
        }

        private static string BuildExpectedGetMethodFullName(
            PropertyMember propertyMember,
            params IType[] parameterTypes
        )
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

        private static StringBuilder AddParameterTypesToMethodName(
            StringBuilder nameBuilder,
            params IType[] parameterTypeNames
        )
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
            Assert.Equal(
                _autoPropertyMember.Name,
                RegexUtils.MatchGetPropertyName(_expectedGetMethodFullName)
            );
        }

        [Fact]
        public void GetMethodPropertyMemberRegexMatchAsExpected()
        {
            Assert.Equal(
                _autoPropertyMember.Name,
                RegexUtils.MatchGetPropertyName(_expectedGetMethodName)
            );
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
            Assert.Equal(
                _autoPropertyMember.Name,
                RegexUtils.MatchSetPropertyName(_expectedSetMethodName)
            );
        }
    }

    public class BackingFieldExamples
    {
        private ChildField _fieldPropertyPair;
        public PropertyType AutoProperty { get; set; }

        public PropertyType FieldPropertyPair
        {
            get => _fieldPropertyPair;
            set => _fieldPropertyPair = (ChildField)value;
        }

        public PropertyType LambdaFieldPropertyPair { get; set; }
    }
}
