/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using ArchUnitNETTests.Dependencies.Members;
using TestAssembly;
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedMember.Local

// ReSharper disable UnassignedField.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedVariable
// ReSharper disable UnusedTypeParameter

namespace ArchUnitNETTests.Dependencies.Attributes
{
    public class TestAttributes
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ExampleAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ExampleClassAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Interface)]
    public class ExampleInterfaceAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ExampleMethodAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ExampleFieldAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExamplePropertyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class ExampleParameterAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Struct)]
    public class ExampleStructAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class ExampleEnumAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Delegate)]
    public class ExampleDelegateAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.ReturnValue)]
    public class ExampleReturnAttribute : Attribute
    {
    }

    public abstract class ExampleAbstractAttribute : Attribute
    {
        private FieldType _fieldType;
    }

    public class ChildOfAbstractAttribute : ExampleAbstractAttribute
    {
        public PropertyType Property { get; }
    }

    public class ConstructorAttribute : Attribute
    {
        private FieldType _insideField;
        
        ConstructorAttribute()
        {
            _insideField = new FieldType();
        }

        ConstructorAttribute(FieldType insideField)
        {
            _insideField = insideField;
        }
    }

    public class ForbiddenAttribute : Attribute
    {

    }

    public class InterfaceImplementingAttribute : Attribute, IAttribute
    {
    }

    public interface IAttribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class CountryAttributeWithParameters : Attribute
    {
        private PropertyType _propertyTypeA;

        public CountryAttributeWithParameters(string country, int population)
        {
            Country = country;
            Population = population;
            Visited = false;
        }

        private string Country { get; }

        private int Population { get; }

        public bool Visited { get; set; }

        public PropertyType PropertyTypeA { get; private set; }
    }

    [ExampleClass]
    public class ClassWithExampleAttribute
    {
        private DelegateWithAttribute _delegateWithAttribute = MethodForDelegate;

        private string _parameterProperty;

        [ExampleField]
        public FieldType FieldA;

        [ChildOfAbstract]
        public string FieldWithAbstractAttributeImplemented;

        public string ParameterProperty;

        [ExampleProperty]
        public string PropertyA { get; private set; }

        public string get_ParameterProperty()
        {
            return _parameterProperty;
        }

        public void set_ParameterProperty([ExampleParameter] string value)
        {
            _parameterProperty = value;
        }

        private static void MethodForDelegate(FieldType fieldType)
        {
        }

        [ExampleMethod]
        public void MethodWithAttribute()
        {
        }

        [CountryAttributeWithParameters("Germany", 83000000, Visited = true)]
        public void MethodWithCountryAttribute()
        {
        }

        public void MethodWithParameterAttribute([ExampleParameter] string example)
        {
            var doubleExample = example + example;
        }

        [return: ExampleReturn]
        public string MethodWithReturnAttribute()
        {
            return "Return from MethodWithReturnAttribute";
        }
    }

    [ExampleDelegate]
    public delegate void DelegateWithAttribute(FieldType fieldType);

    [ExampleEnum]
    public enum EnumWithAttribute
    {
        Enum1,
        Enum2,
        Enum3,
        Enum4
    }

    [ExampleInterface]
    public interface IInterfaceWithExampleAttribute
    {
        string Name { get; set; }
    }

    [ExampleStruct]
    public struct StructWithAttribute
    {
        public int Field1;
        public FieldType Field2;
        private string Field3;
    }

    [TypeDependent(typeof(Hello))]
    [TypeDependent(typeof(HelloEvent))]
    public class ClassWithInnerAttributeDependency
    {
        [TypeDependent(typeof(ClassWithExampleAttribute))]
        private FieldType _fieldType;

        [TypeDependent(typeof(ClassWithBodyTypeA))]
        public FieldType FieldType => _fieldType;

        [TypeDependent(typeof(Class1))]
        [TypeDependent(typeof(Class2))]
        public void Method()
        {
        }
    }

    [TypeDependent(typeof(Hello))]
    [TypeDependent(typeof(HelloEvent))]
    public class AttributeWithAttributes : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TypeDependentAttribute : Attribute
    {
        public TypeDependentAttribute(Type type)
        {
            Type = type;
        }

        private Type Type { get; }
    }

    public class Hello : IEventHandler<HelloEvent>
    {
//        [Forbidden]
        public void Handle(HelloEvent helloEvent)
        {
        }

        public static int StaticMethod()
        {
            return 1;
        }
    }

    public class HelloEvent : IPersistentEvent
    {
    }

    public interface IEventHandler<TEvent>
    {
    }

    public interface IPersistentEvent
    {
    }
}