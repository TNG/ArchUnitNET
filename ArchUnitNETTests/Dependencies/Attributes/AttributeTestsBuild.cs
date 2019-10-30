//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;

namespace ArchUnitNETTests.Dependencies.Attributes
{
    public static class AttributeTestsBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.AttributeDependencyTestArchitecture;

        private static object[] BuildTypeAttributeTestData(Type classType, Type attributeType)
        {
            var targetClass = Architecture.GetITypeOfType(classType);
            var attributeClass =
                Architecture.GetClassOfType(attributeType);
            var attribute = targetClass.GetAttributeOfType(attributeClass);

            return new object[] {targetClass, attributeClass, attribute};
        }


        private static object[] BuildMemberAttributeTestData(Type classType, string memberName, Type attributeType)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (memberName == null)
            {
                throw new ArgumentNullException(nameof(memberName));
            }

            if (attributeType == null)
            {
                throw new ArgumentNullException(nameof(attributeType));
            }

            var targetClass = Architecture.GetClassOfType(classType);
            var targetMember = targetClass.Members[memberName];
            targetMember.RequiredNotNull();
            var attributeClass = Architecture.GetClassOfType(attributeType);
            var attribute = targetMember.GetAttributeFromMember(attributeClass);
            attribute.RequiredNotNull();

            return new object[] {targetMember, attributeClass, attribute};
        }

        public class TypeAttributesAreFoundData : IEnumerable<object[]>
        {
            private readonly List<object[]> _typeAttributeData = new List<object[]>
            {
                BuildTypeAttributeTestData(typeof(ClassWithExampleAttribute), typeof(ExampleClassAttribute)),
                BuildTypeAttributeTestData(typeof(DelegateWithAttribute), typeof(ExampleDelegateAttribute)),
                BuildTypeAttributeTestData(typeof(EnumWithAttribute), typeof(ExampleEnumAttribute)),
                BuildTypeAttributeTestData(typeof(IInterfaceWithExampleAttribute),
                    typeof(ExampleInterfaceAttribute)),
                BuildTypeAttributeTestData(typeof(StructWithAttribute), typeof(ExampleStructAttribute))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _typeAttributeData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class MemberAttributesAreFoundData : IEnumerable<object[]>
        {
            private readonly List<object[]> _memberAttributeData = new List<object[]>
            {
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.FieldA), typeof(ExampleFieldAttribute)),
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.MethodWithAttribute).BuildMethodMemberName(),
                    typeof(ExampleMethodAttribute)),
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.MethodWithParameterAttribute)
                        .BuildMethodMemberName(typeof(string)), typeof(ExampleParameterAttribute)),
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.PropertyA), typeof(ExamplePropertyAttribute)),
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.MethodWithReturnAttribute).BuildMethodMemberName(),
                    typeof(ExampleReturnAttribute)),
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.set_ParameterProperty).BuildMethodMemberName(typeof(string)),
                    typeof(ExampleParameterAttribute)),
                BuildMemberAttributeTestData(typeof(ClassWithExampleAttribute),
                    nameof(ClassWithExampleAttribute.FieldWithAbstractAttributeImplemented),
                    typeof(ChildOfAbstractAttribute))
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _memberAttributeData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}