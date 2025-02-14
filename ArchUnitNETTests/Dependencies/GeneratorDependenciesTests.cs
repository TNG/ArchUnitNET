using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class GeneratorDependenciesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssembly(typeof(GeneratorDependenciesTests).Assembly)
            .Build();

        private readonly Class _accessedClass;
        private readonly FieldMember _accessedStaticField;
        private readonly FieldMember _accessedBoolField;
        private readonly FieldMember _accessedStaticMethodAsField;
        private readonly MethodMember _accessedConstructor;
        private readonly MethodMember _accessedStaticMethodWithBody;

        public GeneratorDependenciesTests()
        {
            _accessedClass = Architecture.GetClassOfType(typeof(AccessedClass));
            _accessedStaticField = _accessedClass
                .GetFieldMembersWithName(nameof(AccessedClass.StringField))
                .First();
            _accessedBoolField = _accessedClass
                .GetFieldMembersWithName(nameof(AccessedClass.BoolField))
                .First();
            _accessedStaticMethodAsField = _accessedClass
                .GetFieldMembersWithName(nameof(AccessedClass.StaticMethodAsField))
                .First();
            _accessedConstructor = _accessedClass.Constructors.First();
            _accessedStaticMethodWithBody = _accessedClass
                .GetMethodMembersWithName(
                    nameof(AccessedClass.StaticMethodWithBody) + "(System.Int32)"
                )
                .First();
        }

        public static IEnumerable<object[]> GetGenerators()
        {
            var classWithGenerators = Architecture.GetClassOfType(typeof(ClassWithGenerators));
            var simpleGenerator = classWithGenerators
                .GetMethodMembersWithName(nameof(ClassWithGenerators.SimpleGenerator) + "()")
                .First();
            var complexGenerator = classWithGenerators
                .GetMethodMembersWithName(nameof(ClassWithGenerators.ComplexGenerator) + "()")
                .First();
            var nestedGenerator = classWithGenerators
                .GetMethodMembersWithName(nameof(ClassWithGenerators.ContainingGenerator) + "()")
                .First();

            yield return new object[] { simpleGenerator };
            yield return new object[] { complexGenerator };
            yield return new object[] { nestedGenerator };
        }

        [Theory]
        [MemberData(nameof(GetGenerators))]
        public void AssignDependenciesInGeneratorMethodBody(MethodMember generator)
        {
            var accessedFieldMembers = generator.GetAccessedFieldMembers().ToList();
            var calledMethods = generator.GetCalledMethods().ToList();
            var bodyTypes = generator
                .GetBodyTypeMemberDependencies()
                .Select(dep => dep.Target)
                .ToList();

            Assert.Contains(_accessedBoolField, accessedFieldMembers);
            Assert.Contains(_accessedStaticField, accessedFieldMembers);
            Assert.Contains(_accessedStaticMethodAsField, accessedFieldMembers);
            Assert.Contains(_accessedConstructor, calledMethods);
            Assert.Contains(_accessedStaticMethodWithBody, calledMethods);
            Assert.Contains(_accessedClass, bodyTypes);
            Assert.Contains(bodyTypes, type => type.Name == "String");
            Assert.Contains(bodyTypes, type => type.Name == "Boolean");
            Assert.Contains(bodyTypes, type => type.Name == "Func`2");
        }
    }

    internal class ClassWithGenerators
    {
        public IEnumerable<bool> SimpleGenerator()
        {
            var a = new AccessedClass();
            var b = AccessedClass.StringField;
            var c = a.BoolField;
            var d = AccessedClass.StaticMethodAsField;
            Func<int, int> e = AccessedClass.StaticMethodWithBody;
            yield return true;
        }

        public IEnumerable<string> ComplexGenerator()
        {
            var a = new AccessedClass();
            var c = a.BoolField;
            if (c)
            {
                var b = AccessedClass.StringField;
                yield return b;
                yield return AccessedClass.StaticMethodAsField(3).ToString();
            }
            else
            {
                Func<int, int> e = AccessedClass.StaticMethodWithBody;
                for (var i = 0; i < 10; i++)
                {
                    yield return e(i).ToString();
                }
            }
        }

        public IEnumerable<int> ContainingGenerator()
        {
            IEnumerable<int> NestedGenerator()
            {
                var a = new AccessedClass();
                var b = AccessedClass.StringField;
                var c = a.BoolField;
                var d = c ? AccessedClass.StaticMethodAsField : AccessedClass.StaticMethodWithBody;
                yield return 0;
            }

            return NestedGenerator();
        }
    }

    internal class AccessedClass
    {
#pragma warning disable 649
        public bool BoolField;
        public static string StringField;

        public static int StaticMethodWithBody(int a)
        {
            return a + 1;
        }

        public static Func<int, int> StaticMethodAsField = i => i + 2;
    }
}
