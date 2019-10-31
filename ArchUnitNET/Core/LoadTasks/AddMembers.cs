//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Core.LoadTasks
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
                    .Concat(typeDefinition.Methods.Select(CreateMethodMember)));
        }

        [NotNull]
        private IMember CreateFieldMember([NotNull] FieldDefinition fieldDefinition)
        {
            var typeReference = fieldDefinition.FieldType;
            var fieldType = _typeFactory.GetOrCreateStubTypeFromTypeReference(typeReference);
            var visibility = GetVisibilityFromFieldDefinition(fieldDefinition);

            return new FieldMember(_type, fieldDefinition.Name, fieldDefinition.FullName, visibility, fieldType);
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

        [NotNull]
        private IMember CreatePropertyMember(PropertyDefinition propertyDefinition)
        {
            var typeReference = propertyDefinition.PropertyType;
            var propertyType = _typeFactory.GetOrCreateStubTypeFromTypeReference(typeReference);

            MethodMember getter = null;
            var isVirtual = false;

            if (propertyDefinition.GetMethod != null)
            {
                isVirtual = propertyDefinition.GetMethod.IsVirtual;
                getter = CreateMethodMember(propertyDefinition.GetMethod);
            }

            MethodMember setter = null;

            if (propertyDefinition.SetMethod != null)
            {
                isVirtual = isVirtual || propertyDefinition.SetMethod.IsVirtual;
                setter = CreateMethodMember(propertyDefinition.SetMethod);
            }

            return new PropertyMember(_type, propertyDefinition.Name, propertyDefinition.FullName,
                propertyType, isVirtual, getter, setter);
        }

        [NotNull]
        private MethodMember CreateMethodMember(MethodDefinition methodDefinition)
        {
            var typeReference = methodDefinition.ReturnType;
            var returnType = _typeFactory.GetOrCreateStubTypeFromTypeReference(typeReference);
            var parameters = methodDefinition.GetParameters(_typeFactory).ToList();

            var visibility = methodDefinition.GetVisibility();
            var methodForm = methodDefinition.GetMethodForm();
            var methodName = methodDefinition.BuildMethodMemberName();
            var methodDefinitionFullName = methodDefinition.GetFullName();

            return new MethodMember(methodName, methodDefinitionFullName, _type, visibility,
                parameters, returnType, methodDefinition.IsVirtual, methodForm);
        }
    }
}