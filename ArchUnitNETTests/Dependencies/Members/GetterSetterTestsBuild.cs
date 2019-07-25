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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNETTests.ArchitectureTestExceptions;
using ArchUnitNETTests.Fluent;
using static ArchUnitNETTests.Fluent.BuildMocksExtensions;
using Type = System.Type;

namespace ArchUnitNETTests.Dependencies.Members
{
    public class GetterSetterTestsBuild
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(typeof(GetterMethodDependencyExamples).Assembly).Build();
        
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Class MockGuidClass = GuidType.CreateStubClass();
        private static readonly MethodInfo NewGuid = GuidType.GetMethods().First(method => method.Name == "NewGuid");
        private static readonly MethodMember MockNewGuid = NewGuid.CreateStubMethodMember();

        private static readonly Type[] ExpectedParameters = {typeof(string)};
        private static readonly ConstructorInfo ConstructGuid = GuidType.GetConstructor(ExpectedParameters);
        private static readonly MethodMember MockConstructorMember = ConstructGuid.CreateStubMethodMember();
        
        public class SetterTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _setterTestData = new List<object[]>
            {
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples._castingPairBacking),
                    nameof(SetterMethodDependencyExamples.CastingPair),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples._castingLambdaPairBacking),
                    nameof(SetterMethodDependencyExamples.CastingLambdaPair),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples._constructorPairBacking),
                    nameof(SetterMethodDependencyExamples.ConstructorPair),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples._constructorLambdaPairBacking),
                    nameof(SetterMethodDependencyExamples.ConstructorLambdaPair),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples._methodPairBacking),
                    nameof(SetterMethodDependencyExamples.MethodPair),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples._methodLambdaPairBacking),
                    nameof(SetterMethodDependencyExamples.MethodLambdaPair),
                    typeof(ChildField)),
            };
            public IEnumerator<object[]> GetEnumerator()
            {
                return _setterTestData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class GetterTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _getterTestData = new List<object[]>
            {
                BuildGetterTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.AcceptedCase),
                    MockGuidClass, MockConstructorMember),
                BuildGetterTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.FirstUnacceptedCase),
                    MockGuidClass, MockNewGuid),
                BuildGetterTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.SecondUnacceptedCase),
                    MockGuidClass, MockNewGuid)
            };
            
            public IEnumerator<object[]> GetEnumerator()
            {
                return _getterTestData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class AccessMethodDependenciesByPropertyTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _accessMethodTestData = new List<object[]>
            {
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.CastingPair),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.CastingLambdaPair),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.ConstructorPair),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.ConstructorLambdaPair),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.MethodPair),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.MethodLambdaPair),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.AcceptedCase),
                    MethodForm.Getter),
                BuildAccessMethodTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.FirstUnacceptedCase),
                    MethodForm.Getter),
                BuildAccessMethodTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.SecondUnacceptedCase),
                    MethodForm.Getter),
            };

            public IEnumerator<object[]> GetEnumerator()
            {
                return _accessMethodTestData.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private static object[] BuildSetterTestData(Type classType, string backingFieldName,
            string backedPropertyName, Type expectedFieldDependencyTarget)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (backingFieldName == null)
            {
                throw new ArgumentNullException(nameof(backingFieldName));
            }

            if (backedPropertyName == null)
            {
                throw new ArgumentNullException(nameof(backedPropertyName));
            }

            if (expectedFieldDependencyTarget == null)
            {
                throw new ArgumentNullException(nameof(expectedFieldDependencyTarget));
            }
            
            var baseClass = Architecture.GetClassOfType(classType);
            var backingField = baseClass.GetFieldMembersWithName(backingFieldName).First();
            var backedProperty = baseClass.GetPropertyMembersWithName(backedPropertyName).First();
            var expectedDependencyTargetClass = Architecture.GetClassOfType(expectedFieldDependencyTarget);

            return new object[] {backingField, backedProperty, expectedDependencyTargetClass};
        }
        
        private static object[] BuildGetterTestData(Type classType, string propertyName,
            Class expectedFieldDependencyTarget, MethodMember expectedTargetMember)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (expectedFieldDependencyTarget == null)
            {
                throw new ArgumentNullException(nameof(expectedFieldDependencyTarget));
            }

            var baseClass = Architecture.GetClassOfType(classType);
            var accessedProperty = baseClass.GetPropertyMembersWithName(propertyName).First();
            var expectedDependency = CreateStubMethodCallDependency(accessedProperty, expectedTargetMember);
            return new object[] {accessedProperty, expectedFieldDependencyTarget, expectedDependency};
        }

        private static object[] BuildAccessMethodTestData(Type classType, string propertyName, MethodForm methodForm)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (methodForm != MethodForm.Getter && methodForm != MethodForm.Setter)
            {
                throw new InvalidInputException($"Given MethodForm {nameof(methodForm)} is not valid for this test. Please give the form of Getter or Setter.");
            }            
            var baseClass = Architecture.GetClassOfType(classType);
            var accessedProperty = baseClass.GetPropertyMembersWithName(propertyName).First();
            var accessorMethod = methodForm == MethodForm.Getter ? accessedProperty.Getter : accessedProperty.Setter;
            return new object[] {accessedProperty, accessorMethod};
        }


    }
}