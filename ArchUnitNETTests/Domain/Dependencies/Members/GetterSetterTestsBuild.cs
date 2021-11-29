//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNETTests.Fluent.Extensions;
using Type = System.Type;

namespace ArchUnitNETTests.Domain.Dependencies.Members
{
    public class GetterSetterTestsBuild
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(typeof(GetterMethodDependencyExamples).Assembly).Build();

        private static readonly Type GuidType = typeof(Guid);
        private static readonly IType MockGuidStruct = GuidType.CreateStubIType();
        private static readonly MethodInfo NewGuid = GuidType.GetMethods().First(method => method.Name == "NewGuid");
        private static readonly MethodMember MockNewGuid = NewGuid.CreateStubMethodMember();

        private static readonly Type[] ExpectedParameters = {typeof(string)};
        private static readonly ConstructorInfo ConstructGuid = GuidType.GetConstructor(ExpectedParameters);
        private static readonly MethodMember MockConstructorMember = ConstructGuid.CreateStubMethodMember();

        private static object[] BuildSetterTestData(Type classType, string backedPropertyName,
            Type expectedFieldDependencyTarget)
        {
            if (classType == null)
            {
                throw new ArgumentNullException(nameof(classType));
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
            var backedProperty = baseClass.GetPropertyMembersWithName(backedPropertyName).First();
            var expectedDependencyTargetClass = Architecture.GetClassOfType(expectedFieldDependencyTarget);

            return new object[] {backedProperty, expectedDependencyTargetClass};
        }

        private static object[] BuildGetterTestData(Type classType, string propertyName,
            IType expectedFieldDependencyTarget, MethodMember expectedTargetMember)
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
                throw new InvalidInputException(
                    $"Given MethodForm {nameof(methodForm)} is not valid for this test. Please give the form of Getter or Setter.");
            }

            var baseClass = Architecture.GetClassOfType(classType);
            var accessedProperty = baseClass.GetPropertyMembersWithName(propertyName).First();
            var accessorMethod = methodForm == MethodForm.Getter ? accessedProperty.Getter : accessedProperty.Setter;
            return new object[] {accessedProperty, accessorMethod};
        }

        private static MethodCallDependency CreateStubMethodCallDependency(IMember originMember,
            MethodMember targetMember)
        {
            var methodCallDependency = new MethodCallDependency(originMember,
                new MethodMemberInstance(targetMember, Enumerable.Empty<GenericArgument>(),
                    Enumerable.Empty<GenericArgument>()));
            methodCallDependency.TargetMember.MemberBackwardsDependencies.Add(methodCallDependency);
            return methodCallDependency;
        }

        public class SetterTestData : IEnumerable<object[]>
        {
            private readonly List<object[]> _setterTestData = new List<object[]>
            {
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.CustomProperty),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.LambdaPair),
                    typeof(ChildField)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.ConstructorPair),
                    typeof(PropertyType)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.ConstructorLambdaPair),
                    typeof(PropertyType)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.MethodPair),
                    typeof(PropertyType)),
                BuildSetterTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.MethodLambdaPair),
                    typeof(PropertyType))
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
                    MockGuidStruct, MockConstructorMember),
                BuildGetterTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.FirstUnacceptedCase),
                    MockGuidStruct, MockNewGuid),
                BuildGetterTestData(typeof(GetterMethodDependencyExamples),
                    nameof(GetterMethodDependencyExamples.SecondUnacceptedCase),
                    MockGuidStruct, MockNewGuid)
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
                    nameof(SetterMethodDependencyExamples.CustomProperty),
                    MethodForm.Setter),
                BuildAccessMethodTestData(typeof(SetterMethodDependencyExamples),
                    nameof(SetterMethodDependencyExamples.LambdaPair),
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
                    MethodForm.Getter)
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
    }
}