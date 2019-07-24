/*
 * Copyright 2019 TNG Technology Consulting GmbH
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

using System;
using System.Collections;
using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.Fluent;
using Type = System.Type;

namespace ArchUnitNETTests.Dependencies.Attributes
{
    public static class AttributeTestsBuild
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.AttributeDependencyTestArchitecture;

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

        private static object[] BuildTypeAttributeTestData(Type classType, Type attributeType)
        {
            var targetClass = Architecture.GetTypeOfType(classType);
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
    }
}