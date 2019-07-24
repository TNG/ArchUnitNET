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

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using JetBrains.Annotations;
using Mono.Cecil;

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

            var visibility = fieldDefinition.GetVisibility();
            return new FieldMember(_type, fieldDefinition.Name, fieldDefinition.FullName, fieldType,
                visibility);
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
            
            return new MethodMember(methodName, methodDefinition.FullName, _type, visibility,
                parameters, returnType, methodDefinition.IsVirtual, methodForm);
        }
    }
}