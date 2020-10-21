//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Dependencies
{
    public class AccessFieldDependenciesTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(AccessFieldDependenciesTests).Assembly).Build();

        private readonly Class _accessingClass;

        private readonly Class _classWithNonStaticFields;
        private readonly Class _classWithStaticFields;
        private readonly FieldMember _constFieldMember;

        private readonly FieldMember _nonStaticFieldMember;
        private readonly FieldMember _staticFieldMember;

        public AccessFieldDependenciesTests()
        {
            _classWithNonStaticFields = Architecture.GetClassOfType(typeof(ClassWithNonStaticFields));
            _classWithStaticFields = Architecture.GetClassOfType(typeof(ClassWithStaticFields));
            _accessingClass = Architecture.GetClassOfType(typeof(ClassAccessingFields));


            _nonStaticFieldMember = _classWithNonStaticFields.GetFieldMembersWithName("NonStaticField").First();
            _constFieldMember = _classWithStaticFields.GetFieldMembersWithName("ConstField").First();
            _staticFieldMember = _classWithStaticFields.GetFieldMembersWithName("StaticField").First();
        }

        [Fact]
        public void PropertyAccessToConstFieldFound()
        {
            var property = _accessingClass.GetPropertyMembers()
                .First(member => member.FullNameContains("PropertyAccessingConstField"));
            var propertyTypeDependencies = property.GetTypeDependencies().ToList();
            var propertyFieldDependencies = property.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticFields, propertyTypeDependencies);
            Assert.Contains(_constFieldMember, propertyFieldDependencies);
        }

        [Fact]
        public void PropertyAccessToStaticFieldFound()
        {
            var property = _accessingClass.GetPropertyMembers()
                .First(member => member.FullNameContains("PropertyAccessingStaticField"));
            var propertyTypeDependencies = property.GetTypeDependencies().ToList();
            var propertyFieldDependencies = property.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticFields, propertyTypeDependencies);
            Assert.Contains(_staticFieldMember, propertyFieldDependencies);
        }

        [Fact]
        public void PropertyAccessToNonStaticFieldFound()
        {
            var property = _accessingClass.GetPropertyMembers()
                .First(member => member.FullNameContains("PropertyAccessingNonStaticField"));
            var propertyTypeDependencies = property.GetTypeDependencies().ToList();
            var propertyFieldDependencies = property.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithNonStaticFields, propertyTypeDependencies);
            Assert.Contains(_nonStaticFieldMember, propertyFieldDependencies);
        }

        [Fact]
        public void MethodAccessToConstFieldFound()
        {
            var method = _accessingClass.GetMethodMembers()
                .First(member => member.FullNameContains("MethodAccessingConstField"));
            var methodTypeDependencies = method.GetTypeDependencies().ToList();
            var methodFieldDependencies = method.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticFields, methodTypeDependencies);
            Assert.Contains(_constFieldMember, methodFieldDependencies);
        }

        [Fact]
        public void SettingStaticFieldDependencyFound()
        {
            var method = _accessingClass.GetMethodMembers()
                .First(member => member.FullNameContains("MethodSettingStaticField"));
            var methodTypeDependencies = method.GetTypeDependencies().ToList();
            var methodFieldDependencies = method.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticFields, methodTypeDependencies);
            Assert.Contains(_staticFieldMember, methodFieldDependencies);
        }

        [Fact]
        public void GettingStaticFieldDependencyFound()
        {
            var method = _accessingClass.GetMethodMembers()
                .First(member => member.FullNameContains("MethodGettingStaticField"));
            var methodTypeDependencies = method.GetTypeDependencies().ToList();
            var methodFieldDependencies = method.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithStaticFields, methodTypeDependencies);
            Assert.Contains(_staticFieldMember, methodFieldDependencies);
        }

        [Fact]
        public void SettingNonStaticFieldDependencyFound()
        {
            var method = _accessingClass.GetMethodMembers()
                .First(member => member.FullNameContains("MethodSettingNonStaticField"));
            var methodTypeDependencies = method.GetTypeDependencies().ToList();
            var methodFieldDependencies = method.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithNonStaticFields, methodTypeDependencies);
            Assert.Contains(_nonStaticFieldMember, methodFieldDependencies);
        }

        [Fact]
        public void GettingNonStaticFieldDependencyFound()
        {
            var method = _accessingClass.GetMethodMembers()
                .First(member => member.FullNameContains("MethodGettingNonStaticField"));
            var methodTypeDependencies = method.GetTypeDependencies().ToList();
            var methodFieldDependencies = method.GetAccessedFieldMembers().ToList();

            Assert.Contains(_classWithNonStaticFields, methodTypeDependencies);
            Assert.Contains(_nonStaticFieldMember, methodFieldDependencies);
        }
    }

    internal class ClassWithNonStaticFields
    {
        public ClassAccessingFields NonStaticField;
    }

    internal static class ClassWithStaticFields
    {
        public const string ConstField = "ConstField";
        public static ClassAccessingFields StaticField;
    }

    internal class ClassAccessingFields
    {
        public string PropertyAccessingConstField => ClassWithStaticFields.ConstField;
        public ClassAccessingFields PropertyAccessingStaticField => ClassWithStaticFields.StaticField;
        public ClassAccessingFields PropertyAccessingNonStaticField => new ClassWithNonStaticFields().NonStaticField;

        public void MethodAccessingConstField()
        {
            var a = ClassWithStaticFields.ConstField;
        }

        public void MethodGettingStaticField()
        {
            var b = ClassWithStaticFields.StaticField;
        }

        public void MethodSettingStaticField()
        {
            ClassWithStaticFields.StaticField = new ClassAccessingFields();
        }

        public void MethodGettingNonStaticField()
        {
            var cls = new ClassWithNonStaticFields();
            var c = cls.NonStaticField;
        }

        public void MethodSettingNonStaticField()
        {
            var cls = new ClassWithNonStaticFields {NonStaticField = new ClassAccessingFields()};
        }
    }
}