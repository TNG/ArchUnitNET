//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddAttributesAndAttributeDependencies : ILoadTask
    {
        private readonly IType _type;
        private readonly System.Type _systemType;
        private readonly TypeFactory _typeFactory;

        public AddAttributesAndAttributeDependencies(
            IType type,
            System.Type systemType,
            TypeFactory typeFactory
        )
        {
            _type = type;
            _systemType = systemType;
            _typeFactory = typeFactory;
        }

        public void Execute()
        {
            _systemType.CustomAttributes.ForEach(
                AddAttributeArgumentReferenceDependenciesToOriginType
            );
            var typeAttributeInstances = CreateAttributesFromCustomAttributes(
                    _systemType.CustomAttributes
                )
                .ToList();
            _type.AttributeInstances.AddRange(typeAttributeInstances);
            var typeAttributeDependencies = typeAttributeInstances.Select(
                attributeInstance => new AttributeTypeDependency(_type, attributeInstance)
            );
            _type.Dependencies.AddRange(typeAttributeDependencies);
            SetUpAttributesForTypeGenericParameters();
            CollectAttributesForMembers();
        }

        private void SetUpAttributesForTypeGenericParameters()
        {
            foreach (var genericParameter in _systemType.GenericParameters)
            {
                var param = _type.GenericParameters.First(parameter =>
                    parameter.Name == genericParameter.Name
                );
                var attributeInstances = CreateAttributesFromCustomAttributes(
                        genericParameter.CustomAttributes
                    )
                    .ToList();
                _type.AttributeInstances.AddRange(attributeInstances);
                param.AttributeInstances.AddRange(attributeInstances);
                var genericParameterAttributeDependencies = attributeInstances.Select(
                    attributeInstance => new AttributeTypeDependency(_type, attributeInstance)
                );
                _type.Dependencies.AddRange(genericParameterAttributeDependencies);
            }
        }

        private void CollectAttributesForMembers()
        {
            _systemType
                .Fields.Where(x => !x.IsBackingField() && !x.IsCompilerGenerated())
                .ForEach(SetUpAttributesForFields);

            _systemType
                .Properties.Where(x => !x.IsCompilerGenerated())
                .ForEach(SetUpAttributesForProperties);

            _systemType
                .Methods.Where(x => !x.IsCompilerGenerated())
                .ForEach(SetUpAttributesForMethods);
        }

        private void SetUpAttributesForFields(FieldDefinition fieldDefinition)
        {
            var fieldMember = _type
                .GetFieldMembers()
                .WhereFullNameIs(fieldDefinition.FullName)
                .RequiredNotNull();
            CollectMemberAttributesAndDependencies(
                fieldMember,
                fieldDefinition.CustomAttributes.ToList(),
                fieldMember.MemberDependencies
            );
        }

        private void SetUpAttributesForProperties(PropertyDefinition propertyDefinition)
        {
            var propertyMember = _type
                .GetPropertyMembers()
                .WhereFullNameIs(propertyDefinition.FullName)
                .RequiredNotNull();
            CollectMemberAttributesAndDependencies(
                propertyMember,
                propertyDefinition.CustomAttributes.ToList(),
                propertyMember.AttributeDependencies
            );
        }

        private void SetUpAttributesForMethods(MethodDefinition methodDefinition)
        {
            var methodMember = _type
                .GetMethodMembers()
                .WhereFullNameIs(methodDefinition.BuildFullName())
                .RequiredNotNull();
            var memberCustomAttributes = methodDefinition.GetAllMethodCustomAttributes().ToList();
            SetUpAttributesForMethodGenericParameters(methodDefinition, methodMember);
            CollectMemberAttributesAndDependencies(
                methodMember,
                memberCustomAttributes,
                methodMember.MemberDependencies
            );
        }

        private void SetUpAttributesForMethodGenericParameters(
            MethodDefinition methodDefinition,
            MethodMember methodMember
        )
        {
            foreach (var genericParameter in methodDefinition.GenericParameters)
            {
                var param = methodMember.GenericParameters.First(parameter =>
                    parameter.Name == genericParameter.Name
                );
                var customAttributes = genericParameter.CustomAttributes;
                customAttributes.ForEach(AddAttributeArgumentReferenceDependenciesToOriginType);
                var attributeInstances = CreateAttributesFromCustomAttributes(customAttributes)
                    .ToList();
                methodMember.AttributeInstances.AddRange(attributeInstances);
                param.AttributeInstances.AddRange(attributeInstances);
                var genericParameterAttributeDependencies = attributeInstances.Select(
                    attributeInstance => new AttributeMemberDependency(
                        methodMember,
                        attributeInstance
                    )
                );
                methodMember.MemberDependencies.AddRange(genericParameterAttributeDependencies);
            }
        }

        private void CollectMemberAttributesAndDependencies(
            IMember methodMember,
            List<CustomAttributeData> memberCustomAttributes,
            List<IMemberTypeDependency> attributeDependencies
        )
        {
            memberCustomAttributes.ForEach(AddAttributeArgumentReferenceDependenciesToOriginType);
            var memberAttributeInstances = CreateAttributesFromCustomAttributes(
                    memberCustomAttributes
                )
                .ToList();
            methodMember.AttributeInstances.AddRange(memberAttributeInstances);
            var methodAttributeDependencies = CreateMemberAttributeDependencies(
                methodMember,
                memberAttributeInstances
            );
            attributeDependencies.AddRange(methodAttributeDependencies);
        }

        [NotNull]
        public IEnumerable<AttributeInstance> CreateAttributesFromCustomAttributes(
            IEnumerable<CustomAttributeData> customAttributes
        )
        {
            return customAttributes
                .Where(customAttribute =>
                    customAttribute.AttributeType.FullName
                        != "Microsoft.CodeAnalysis.EmbeddedAttribute"
                    && customAttribute.AttributeType.FullName
                        != "System.Runtime.CompilerServices.NullableAttribute"
                    && customAttribute.AttributeType.FullName
                        != "System.Runtime.CompilerServices.NullableContextAttribute"
                )
                .Select(attr => attr.CreateAttributeFromCustomAttributeData(_typeFactory));
        }

        [NotNull]
        private static IEnumerable<AttributeMemberDependency> CreateMemberAttributeDependencies(
            IMember member,
            IEnumerable<AttributeInstance> attributes
        )
        {
            return attributes.Select(attributeInstance => new AttributeMemberDependency(
                member,
                attributeInstance
            ));
        }

        private void AddAttributeArgumentReferenceDependenciesToOriginType(
            CustomAttributeData customAttributeData
        )
        {
            if (!customAttributeData.ConstructorArguments.Any())
            {
                return;
            }

            var attributeConstructorArgs = customAttributeData.ConstructorArguments;
            attributeConstructorArgs
                .Where(attributeArgument =>
                    attributeArgument.Value is System.Type systemType
                    && systemType.GetCustomAttribute<CompilerGeneratedAttribute>() == null
                )
                .Select(attributeArgument =>
                    (systemType: attributeArgument.Value as System.Type, attributeArgument)
                )
                .ForEach(tuple =>
                {
                    var argumentType = _typeFactory.GetOrCreateStubTypeFromSystemType(
                        tuple.systemType
                    );
                    var dependency = new TypeReferenceDependency(_type, argumentType);
                    _type.Dependencies.Add(dependency);
                });
        }
    }
}
