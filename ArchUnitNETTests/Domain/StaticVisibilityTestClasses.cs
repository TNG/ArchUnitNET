using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNETTests.Fluent.Extensions;

namespace ArchUnitNETTests.Domain
{
    public static class StaticVisibilityTestClasses
    {
        private static readonly Architecture Architecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
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
    }

    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global

    internal class TestClassForVisibilityTest
    {
        internal static readonly Type TypeOfNestedPrivateTestClass = typeof(NestedPrivateTestClass);
        internal static readonly Type TypeOfNestedProtectedTestClass = typeof(NestedProtectedTestClass);
        internal static readonly Type TypeOfNestedPrivateProtectedTestClass = typeof(NestedPrivateProtectedTestClass);


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
    }

    public class PublicTestClass
    {
    }

    internal class InternalTestClass
    {
    }
}