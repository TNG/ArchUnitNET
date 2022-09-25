//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddMembers : ILoadTask
    {
        private readonly MemberList _memberList;
        private readonly IType _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly TypeFactory _typeFactory;
        
        
        public AddMembers(IType type, TypeDefinition typeDefinition, TypeFactory typeFactory, MemberList memberList)
        {
            _type = type;
            _typeDefinition = typeDefinition;
            _typeFactory = typeFactory;
            _memberList = memberList;
        }

        public void Execute()
        {
            var members = CreateMembers(_typeDefinition);
            _memberList.AddRange(members);
        }

        [NotNull]
        private IEnumerable<IMember> CreateMembers([NotNull] TypeDefinition typeDefinition)
        {
            return typeDefinition.Fields.Where(fieldDefinition => !fieldDefinition.IsBackingField())
                .Select(CreateFieldMember).Concat(typeDefinition.Properties.Select(CreatePropertyMember)
                    .Concat(typeDefinition.Methods.Select(method =>
                        _typeFactory.GetOrCreateMethodMemberFromMethodReference(_type, method).Member)))
                .Where(member => !member.IsCompilerGenerated);
        }

        [NotNull]
        private IMember CreateFieldMember([NotNull] FieldDefinition fieldDefinition)
        {
            var typeReference = fieldDefinition.FieldType;
            var fieldType = _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            var visibility = GetVisibilityFromFieldDefinition(fieldDefinition);
            var isCompilerGenerated = fieldDefinition.IsCompilerGenerated();
            var writeAccessor = GetWriteAccessor(fieldDefinition);
            return new FieldMember(_type, fieldDefinition.Name, fieldDefinition.FullName, visibility, fieldType,
                isCompilerGenerated, fieldDefinition.IsStatic, writeAccessor);
        }

        [NotNull]
        private IMember CreatePropertyMember(PropertyDefinition propertyDefinition)
        {
            var typeReference = propertyDefinition.PropertyType;
            var propertyType = _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            var isCompilerGenerated = propertyDefinition.IsCompilerGenerated();
            var isStatic = (propertyDefinition.SetMethod != null && propertyDefinition.SetMethod.IsStatic) ||
                           (propertyDefinition.GetMethod != null && propertyDefinition.GetMethod.IsStatic);
            var writeAccessor = GetWriteAccessor(propertyDefinition);
            return new PropertyMember(_type, propertyDefinition.Name, propertyDefinition.FullName, propertyType,
                isCompilerGenerated, isStatic, writeAccessor);
        }

        private static Visibility GetVisibilityFromFieldDefinition([NotNull] FieldDefinition fieldDefinition)
        {
            if (fieldDefinition.IsPublic)
            {
                return Public;
            }

            if (fieldDefinition.IsPrivate)
            {
                return Private;
            }

            if (fieldDefinition.IsFamily)
            {
                return Protected;
            }

            if (fieldDefinition.IsAssembly)
            {
                return Internal;
            }

            if (fieldDefinition.IsFamilyOrAssembly)
            {
                return ProtectedInternal;
            }

            if (fieldDefinition.IsFamilyAndAssembly)
            {
                return PrivateProtected;
            }

            throw new ArgumentException("The field definition seems to have no visibility.");
        }

        private static Writability GetWriteAccessor([NotNull] FieldDefinition fieldDefinition)
        {
            return fieldDefinition.IsInitOnly ? Writability.ReadOnly : Writability.Writable;
        }

        private static Writability GetWriteAccessor([NotNull] PropertyDefinition propertyDefinition)
        {
            bool isReadOnly = propertyDefinition.SetMethod == null;

            if (isReadOnly)
            {
                return Writability.ReadOnly;
            }

            bool isInitSetter = CheckPropertyHasInitSetterInNetStandardCompatibleWay(propertyDefinition);
            return isInitSetter ? Writability.InitOnly : Writability.Writable;
        }

        private static bool CheckPropertyHasInitSetterInNetStandardCompatibleWay(PropertyDefinition propertyDefinition)
        {
            return propertyDefinition.SetMethod?.ReturnType.IsRequiredModifier == true
                && ((RequiredModifierType)propertyDefinition.SetMethod.ReturnType).ModifierType.FullName
                    == "System.Runtime.CompilerServices.IsExternalInit";
        }
    }
}