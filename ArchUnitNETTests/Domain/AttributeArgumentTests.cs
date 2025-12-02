using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using Attribute = System.Attribute;

namespace ArchUnitNETTests.Domain
{
    public class AttributeArgumentTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(AttributeArgumentTests).Assembly)
            .Build();

        private readonly Class _classWithMultipleAttributesWithParameters;
        private readonly Class _classWithTypeParameterAttribute;
        private readonly Class _classWithArrayParameterAttribute;
        private readonly Class _classWithMixedParameterAttribute;

        private readonly ArchUnitNET.Domain.Attribute _attributeWithStringParameters;
        private readonly ArchUnitNET.Domain.Attribute _attributeWithObjectParameter;

        public AttributeArgumentTests()
        {
            _classWithMultipleAttributesWithParameters = Architecture.GetClassOfType(
                typeof(ClassWithMultipleAttributesWithParameters)
            );
            _classWithTypeParameterAttribute = Architecture.GetClassOfType(
                typeof(ClassWithTypeParameterAttribute)
            );
            _classWithArrayParameterAttribute = Architecture.GetClassOfType(
                typeof(ClassWithArrayParameterAttribute)
            );
            _classWithMixedParameterAttribute = Architecture.GetClassOfType(
                typeof(ClassWithMixedParameterAttribute)
            );
            _attributeWithStringParameters = Architecture.GetAttributeOfType(
                typeof(AttributeWithStringParameters)
            );
            _attributeWithObjectParameter = Architecture.GetAttributeOfType(
                typeof(AttributeWithObjectParameter)
            );
        }

        [Fact]
        public void AssignArrayParametersToAttributeInstance()
        {
            var argumentValues = _classWithArrayParameterAttribute
                .AttributeInstances.First()
                .AttributeArguments.Select(arg => (object[])arg.Value)
                .ToList();
            Assert.Contains(new object[] { 1, 2, 3 }, argumentValues);
            Assert.Contains(new object[] { "a", "b", "c", "d" }, argumentValues);
            Assert.Contains(new object[] { }, argumentValues);
        }

        [Fact]
        public void AssignTypeParametersToAttributeInstance()
        {
            var argumentValues = _classWithTypeParameterAttribute
                .AttributeInstances.First()
                .AttributeArguments.Select(arg => (ITypeInstance<IType>)arg.Value)
                .Select(instance => instance.Type)
                .ToList();
            Assert.Contains(_classWithTypeParameterAttribute, argumentValues);
            Assert.Contains(_classWithArrayParameterAttribute, argumentValues);
            Assert.Contains(_classWithMixedParameterAttribute, argumentValues);
        }

        [Fact]
        public void AssignMixedParametersToAttributeInstance()
        {
            var argumentValues = _classWithMixedParameterAttribute
                .AttributeInstances.First()
                .AttributeArguments.Select(arg => arg.Value)
                .ToList();
            Assert.Contains((uint)10, argumentValues);
            Assert.Contains(true, argumentValues);
            Assert.Contains("test", argumentValues);
        }

        [Fact]
        public void FindMultipleAttributesOfTheSameType()
        {
            Assert.Equal(
                3,
                _classWithMultipleAttributesWithParameters.Attributes.Count(att =>
                    Equals(att, _attributeWithStringParameters)
                )
            );
            Assert.Equal(
                1,
                _classWithMultipleAttributesWithParameters.Attributes.Count(att =>
                    Equals(att, _attributeWithObjectParameter)
                )
            );
        }

        [Fact]
        public void AssignNamesToAttributeParameters()
        {
            var namedArguments = _classWithMultipleAttributesWithParameters
                .AttributeInstances.SelectMany(instance => instance.AttributeArguments)
                .OfType<AttributeNamedArgument>()
                .ToList();
            Assert.Contains(
                namedArguments,
                arg => arg.Name == "Parameter2" && (string)arg.Value == "param2_1"
            );
            Assert.Contains(
                namedArguments,
                arg => arg.Name == "Parameter3" && (string)arg.Value == "param3_2"
            );
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class AttributeWithStringParameters : Attribute
    {
        public AttributeWithStringParameters(string parameter1)
        {
            Parameter1 = parameter1;
        }

        public AttributeWithStringParameters(string parameter1, string parameter2)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
        }

        public string Parameter1 { get; }
        public string Parameter2;
        public string Parameter3 { get; set; }
    }

    internal class AttributeWithObjectParameter : Attribute
    {
        public AttributeWithObjectParameter(params object[] arguments) { }

        public AttributeWithObjectParameter(object arg) { }

        public object Type { get; }
        public object Type2;
        public object Type3 { get; set; }
    }

    [AttributeWithStringParameters("param1_0")]
    [AttributeWithStringParameters("param1_1", Parameter2 = "param2_1")]
    [AttributeWithStringParameters("param1_2", "param2_2", Parameter3 = "param3_2")]
    [AttributeWithObjectParameter(null)]
    internal class ClassWithMultipleAttributesWithParameters { }

    [AttributeWithObjectParameter(
        typeof(ClassWithTypeParameterAttribute),
        Type2 = typeof(ClassWithArrayParameterAttribute),
        Type3 = typeof(ClassWithMixedParameterAttribute)
    )]
    internal class ClassWithTypeParameterAttribute
    {
        public ClassWithTypeParameterAttribute()
        {
            var a = new[] { new[] { "a", "b" }, new[] { "c", "d" } };
        }
    }

    [AttributeWithObjectParameter(
        new[] { 1, 2, 3 },
        Type2 = new[] { "a", "b", "c", "d" },
        Type3 = new bool[] { }
    )]
    internal class ClassWithArrayParameterAttribute { }

    [AttributeWithObjectParameter((uint)10, Type2 = true, Type3 = "test")]
    internal class ClassWithMixedParameterAttribute { }
}
