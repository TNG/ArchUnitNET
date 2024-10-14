//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddMembers : ILoadTask
    {
        private readonly MemberList _memberList;
        private readonly ITypeInstance<IType> _typeInstance;
        private readonly System.Type _systemType;
        private readonly TypeFactory _typeFactory;

        public AddMembers(
            ITypeInstance<IType> typeInstance,
            System.Type systemType,
            TypeFactory typeFactory,
            MemberList memberList
        )
        {
            _typeInstance = typeInstance;
            _systemType = systemType;
            _typeFactory = typeFactory;
            _memberList = memberList;
        }

        public void Execute()
        {
            var members = CreateMembers(_systemType);
            _memberList.AddRange(members);
        }

        [NotNull]
        private IEnumerable<IMember> CreateMembers([NotNull] System.Type systemType)
        {
            var fields = systemType
                .GetFields()
                .Where(field => !field.IsBackingField())
                .Select(CreateFieldMember);
            var properties = systemType
                .GetProperties()
                .Select(CreatePropertyMember);
            var methods = systemType
                .GetMethods()
                .Select(method =>
                    _typeFactory
                        .GetOrCreateMethodFromMethodInfo(
                            _typeInstance,
                            method
                        )
                        .Member
                );
            return fields
                .Concat(properties)
                .Concat(methods)
                .Where(member => !member.IsCompilerGenerated);
        }

        [NotNull]
        private IMember CreateFieldMember([NotNull] FieldInfo fieldInfo)
        {
            var fieldType = _typeFactory.GetOrCreateStubTypeFromSystemType(
                fieldInfo.FieldType
            );
            var visibility = GetVisibilityFromFieldDefinition(fieldInfo);
            var isCompilerGenerated = fieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>() != null;
            var writeAccessor = GetWriteAccessor(fieldInfo);
            return new FieldMember(
                _typeInstance.Type,
                fieldInfo.Name,
                fieldInfo.Name,
                visibility,
                fieldType,
                isCompilerGenerated,
                fieldInfo.IsStatic,
                writeAccessor
            );
        }

        [NotNull]
        private IMember CreatePropertyMember(PropertyInfo propertyInfo)
        {
            var propertyType = _typeFactory.GetOrCreateStubTypeFromSystemType(
                propertyInfo.PropertyType
            );
            var isCompilerGenerated = propertyInfo.GetCustomAttribute<CompilerGeneratedAttribute>() != null; 
            var isStatic =
                (propertyInfo.SetMethod != null && propertyInfo.SetMethod.IsStatic)
                || (propertyInfo.GetMethod != null && propertyInfo.GetMethod.IsStatic);
            var writeAccessor = GetWriteAccessor(propertyInfo);
            return new PropertyMember(
                _typeInstance.Type,
                propertyInfo.Name,
                propertyInfo.FullName,
                propertyType,
                isCompilerGenerated,
                isStatic,
                writeAccessor
            );
        }

        private static Visibility GetVisibilityFromFieldDefinition(
            [NotNull] FieldInfo fieldInfo
        )
        {
            if (fieldInfo.IsPublic)
            {
                return Public;
            }

            if (fieldInfo.IsPrivate)
            {
                return Private;
            }

            if (fieldInfo.IsFamily)
            {
                return Protected;
            }

            if (fieldInfo.IsAssembly)
            {
                return Internal;
            }

            if (fieldInfo.IsFamilyOrAssembly)
            {
                return ProtectedInternal;
            }

            if (fieldInfo.IsFamilyAndAssembly)
            {
                return PrivateProtected;
            }

            throw new ArgumentException("The field definition seems to have no visibility.");
        }

        private static Writability GetWriteAccessor([NotNull] FieldInfo fieldInfo)
        {
            return fieldInfo.IsInitOnly ? Writability.ReadOnly : Writability.Writable;
        }

        private static Writability GetWriteAccessor([NotNull] PropertyInfo propertyInfo)
        {
            bool isReadOnly = propertyInfo.SetMethod == null;

            if (isReadOnly)
            {
                return Writability.ReadOnly;
            }

            bool isInitSetter = CheckPropertyHasInitSetterInNetStandardCompatibleWay(
                propertyInfo
            );
            return isInitSetter ? Writability.InitOnly : Writability.Writable;
        }

        private static bool CheckPropertyHasInitSetterInNetStandardCompatibleWay(
            [NotNull] PropertyInfo propertyInfo
        )
        {
            return false;
            // TODO: Implement this
            // return propertyInfo.SetMethod?.ReturnType.IsRequiredModifier == true
            //     && ((RequiredModifierType)propertyInfo.SetMethod.ReturnType)
            //         .ModifierType
            //         .FullName == "System.Runtime.CompilerServices.IsExternalInit";
        }
    }
}
