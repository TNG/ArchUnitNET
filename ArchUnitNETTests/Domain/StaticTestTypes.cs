//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNETTests.Domain
{
    public static class StaticTestTypes
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;

        public static readonly Class TestEnum = Architecture.GetClassOfType(typeof(TestEnum));
        public static readonly Class TestStruct = Architecture.GetClassOfType(typeof(TestStruct));
        public static readonly Class SealedTestClass = Architecture.GetClassOfType(typeof(SealedTestClass));

        public static readonly Attribute TestAttribute =
            Architecture.GetAttributeOfType(typeof(TestAttribute));

        public static readonly Attribute ChildTestAttribute =
            Architecture.GetAttributeOfType(typeof(ChildTestAttribute));

        public static readonly Attribute SealedTestAttribute =
            Architecture.GetAttributeOfType(typeof(SealedTestAttribute));

        public static readonly Class PublicTestClass = Architecture.GetClassOfType(typeof(PublicTestClass));
        public static readonly Class InternalTestClass = Architecture.GetClassOfType(typeof(InternalTestClass));

        public static readonly Class NestedPublicTestClass =
            Architecture.GetClassOfType(typeof(TestClassForVisibilityTest.NestedPublicTestClass));

        public static readonly Class NestedPrivateTestClass =
            Architecture.GetClassOfType(TestClassForVisibilityTest.TypeOfNestedPrivateTestClass);

        public static readonly Class NestedProtectedTestClass =
            Architecture.GetClassOfType(TestClassForVisibilityTest.TypeOfNestedProtectedTestClass);

        public static readonly Class NestedInternalTestClass =
            Architecture.GetClassOfType(typeof(TestClassForVisibilityTest.NestedInternalTestClass));

        public static readonly Class NestedProtectedInternalTestClass =
            Architecture.GetClassOfType(typeof(TestClassForVisibilityTest.NestedProtectedInternalTestClass));

        public static readonly Class NestedPrivateProtectedTestClass =
            Architecture.GetClassOfType(TestClassForVisibilityTest.TypeOfNestedPrivateProtectedTestClass);


        public static readonly Interface PublicTestInterface =
            Architecture.GetInterfaceOfType(typeof(IPublicTestInterface));

        public static readonly Interface InternalTestInterface =
            Architecture.GetInterfaceOfType(typeof(IInternalTestInterface));

        public static readonly Interface NestedPublicTestInterface =
            Architecture.GetInterfaceOfType(typeof(TestClassForVisibilityTest.INestedPublicTestInterface));

        public static readonly Interface NestedPrivateTestInterface =
            Architecture.GetInterfaceOfType(TestClassForVisibilityTest.TypeOfNestedPrivateTestInterface);

        public static readonly Interface NestedProtectedTestInterface =
            Architecture.GetInterfaceOfType(TestClassForVisibilityTest.TypeOfNestedProtectedTestInterface);

        public static readonly Interface NestedInternalTestInterface =
            Architecture.GetInterfaceOfType(typeof(TestClassForVisibilityTest.INestedInternalTestInterface));

        public static readonly Interface NestedProtectedInternalTestInterface =
            Architecture.GetInterfaceOfType(typeof(TestClassForVisibilityTest.INestedProtectedInternalTestInterface));

        public static readonly Interface NestedPrivateProtectedTestInterface =
            Architecture.GetInterfaceOfType(TestClassForVisibilityTest.TypeOfNestedPrivateProtectedTestInterface);


        public static readonly Interface InheritedTestInterface =
            Architecture.GetInterfaceOfType(typeof(IInheritedTestInterface));

        public static readonly Interface InheritingInterface =
            Architecture.GetInterfaceOfType(typeof(IInheritingInterface));

        public static readonly Class InheritedType = Architecture.GetClassOfType(typeof(InheritedType));
        public static readonly Class InheritingType = Architecture.GetClassOfType(typeof(InheritingType));

        public static readonly Interface TestInterface1 =
            Architecture.GetInterfaceOfType(typeof(ITestInterface1));

        public static readonly Interface InheritedFromTestInterface12 =
            Architecture.GetInterfaceOfType(typeof(IInheritedFromTestInterface12));
    }

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global

    internal class TestClassForVisibilityTest
    {
        internal static readonly Type TypeOfNestedPrivateTestClass = typeof(NestedPrivateTestClass);
        internal static readonly Type TypeOfNestedProtectedTestClass = typeof(NestedProtectedTestClass);
        internal static readonly Type TypeOfNestedPrivateProtectedTestClass = typeof(NestedPrivateProtectedTestClass);

        internal static readonly Type TypeOfNestedPrivateTestInterface = typeof(INestedPrivateTestInterface);
        internal static readonly Type TypeOfNestedProtectedTestInterface = typeof(INestedProtectedTestInterface);

        internal static readonly Type TypeOfNestedPrivateProtectedTestInterface =
            typeof(INestedPrivateProtectedTestInterface);


        public class NestedPublicTestClass
        {
        }

        private class NestedPrivateTestClass
        {
        }

        protected class NestedProtectedTestClass
        {
        }

        internal class NestedInternalTestClass
        {
        }

        protected internal class NestedProtectedInternalTestClass
        {
        }

        private protected class NestedPrivateProtectedTestClass
        {
        }

        public interface INestedPublicTestInterface
        {
        }

        private interface INestedPrivateTestInterface
        {
        }

        protected interface INestedProtectedTestInterface
        {
        }

        internal interface INestedInternalTestInterface
        {
        }

        protected internal interface INestedProtectedInternalTestInterface
        {
        }

        private protected interface INestedPrivateProtectedTestInterface
        {
        }
    }

    public enum TestEnum
    {
    }

    public struct TestStruct
    {
    }

    public sealed class SealedTestClass
    {
    }

    public class TestAttribute : System.Attribute
    {
    }

    public class ChildTestAttribute : TestAttribute
    {
    }

    public sealed class SealedTestAttribute : System.Attribute
    {
    }

    public class PublicTestClass
    {
    }

    internal class InternalTestClass
    {
    }

    public interface IPublicTestInterface
    {
    }

    internal interface IInternalTestInterface
    {
    }

    internal interface IInheritedTestInterface
    {
    }

    internal interface IInheritingInterface : IInheritedTestInterface
    {
    }

    internal abstract class InheritedType : IInheritingInterface
    {
    }

    internal class InheritingType : InheritedType
    {
    }

    internal interface ITestInterface1
    {
    }

    internal interface ITestInterface12
    {
    }

    internal interface IInheritedFromTestInterface12 : ITestInterface12
    {
    }
}