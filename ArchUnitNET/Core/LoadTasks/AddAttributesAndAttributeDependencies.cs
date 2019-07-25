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

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Core.LoadTasks
{
    internal class AddAttributesAndAttributeDependencies : ILoadTask
    {
        private readonly IType _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly TypeFactory _typeFactory;

        public AddAttributesAndAttributeDependencies(IType type, TypeDefinition typeDefinition, TypeFactory typeFactory)
        {
            _type = type;
            _typeDefinition = typeDefinition;
            _typeFactory = typeFactory;
        }

        public void Execute()
        {
            _typeDefinition.CustomAttributes.ForEach(AddAttributeArgumentReferenceDependenciesToOriginType);
            var typeAttributes = CreateAttributesFromCustomAttributes(_typeDefinition.CustomAttributes).ToList();
            _type.Attributes.AddRange(typeAttributes);
            var typeAttributeDependencies = typeAttributes.Select(attribute => new AttributeTypeDependency(_type, attribute));
            _type.Dependencies.AddRange(typeAttributeDependencies);
            CollectAttributesForMembers();
        }

        private void CollectAttributesForMembers()
        {
            _typeDefinition.Fields.Where(x => !x.IsBackingField()).ForEach(SetUpAttributesForFields);

            _typeDefinition.Properties.ForEach(SetUpAttributesForProperties);
            
            _typeDefinition.Methods.ForEach(SetUpAttributesForMethods);
        }

        private void SetUpAttributesForFields(FieldDefinition fieldDefinition)
        {
            var fieldMember = _type.GetFieldMembers().WhereFullNameIs(fieldDefinition.FullName)
                .RequiredNotNull();
            CollectMemberAttributesAndDependencies(fieldMember, fieldDefinition.CustomAttributes.ToList());
        }

        private void SetUpAttributesForProperties(PropertyDefinition propertyDefinition)
        {
            var propertyMember = _type.GetPropertyMembers().WhereFullNameIs(propertyDefinition.FullName)
                .RequiredNotNull();
            CollectMemberAttributesAndDependencies(propertyMember, propertyDefinition.CustomAttributes.ToList());
        }

        private void SetUpAttributesForMethods(MethodDefinition methodDefinition)
        {
            var methodMember = _type.GetMethodMembers().WhereFullNameIs(methodDefinition.FullName)
                .RequiredNotNull();
            var memberCustomAttributes = methodDefinition.GetAllMethodCustomAttributes().ToList();
            CollectMemberAttributesAndDependencies(methodMember, memberCustomAttributes);
        }

        private void CollectMemberAttributesAndDependencies(IMember methodMember, List<CustomAttribute> memberCustomAttributes)
        {
            memberCustomAttributes.ForEach(AddAttributeArgumentReferenceDependenciesToOriginType);
            var memberAttributes = CreateAttributesFromCustomAttributes(memberCustomAttributes).ToList();
            methodMember.Attributes.AddRange(memberAttributes);
            var methodAttributeDependencies = CreateMemberAttributeDependencies(methodMember, memberAttributes);
            methodMember.Dependencies.AddRange(methodAttributeDependencies);
        }

        [NotNull]
        private IEnumerable<Attribute> CreateAttributesFromCustomAttributes(
            IEnumerable<CustomAttribute> customAttributes)
        {
            return customAttributes.Select(customAttribute =>
            {
                var attributeTypeReference = customAttribute.AttributeType;
                var attributeType = _typeFactory.GetOrCreateStubTypeFromTypeReference(attributeTypeReference);
                return new Attribute(attributeType as Class);
            });
        }
        
        [NotNull]
        private static IEnumerable<AttributeMemberDependency> CreateMemberAttributeDependencies(IMember member, IEnumerable<Attribute> attributes)
        {
            return attributes.Select(attribute => new AttributeMemberDependency(member, attribute));
        }

        private void AddAttributeArgumentReferenceDependenciesToOriginType(ICustomAttribute customAttribute)
        {
            if (!customAttribute.HasConstructorArguments)
            {
                return;
            }
            
            var attributeConstructorArgs = customAttribute.ConstructorArguments;
            attributeConstructorArgs
                .Where(attributeArgument => attributeArgument.Value is TypeReference)
                .Select(attributeArgument => (typeReference: attributeArgument.Value as TypeReference,
                    attributeArgument))
                .ForEach(tuple =>
                {
                    var argumentType = _typeFactory.GetOrCreateStubTypeFromTypeReference(tuple.typeReference);
                    var dependency = new TypeReferenceDependency(_type, argumentType);
                    _type.Dependencies.Add(dependency);
                });
        }
    }
}