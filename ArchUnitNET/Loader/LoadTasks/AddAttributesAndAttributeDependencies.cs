using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Creates attribute instances from Cecil custom attributes on the type, its generic
    /// parameters, and its members, then adds corresponding attribute-type dependencies.
    /// </summary>
    internal static class AddAttributesAndAttributeDependencies
    {
        internal static void Execute(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver,
            IReadOnlyList<(MethodMember Member, MethodDefinition Definition)> methodPairs
        )
        {
            typeDefinition.CustomAttributes.ForEach(attr =>
                AddAttributeArgumentReferenceDependencies(type, attr, domainResolver)
            );
            var typeAttributeInstances = CreateAttributesFromCustomAttributes(
                    typeDefinition.CustomAttributes,
                    domainResolver
                )
                .ToList();
            type.AttributeInstances.AddRange(typeAttributeInstances);
            var typeAttributeDependencies = typeAttributeInstances.Select(
                attributeInstance => new AttributeTypeDependency(type, attributeInstance)
            );
            type.Dependencies.AddRange(typeAttributeDependencies);

            SetUpAttributesForTypeGenericParameters(type, typeDefinition, domainResolver);
            CollectAttributesForMembers(type, typeDefinition, domainResolver, methodPairs);
        }

        private static void SetUpAttributesForTypeGenericParameters(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            foreach (var genericParameter in typeDefinition.GenericParameters)
            {
                var param = type.GenericParameters.First(parameter =>
                    parameter.Name == genericParameter.Name
                );
                var attributeInstances = CreateAttributesFromCustomAttributes(
                        genericParameter.CustomAttributes,
                        domainResolver
                    )
                    .ToList();
                type.AttributeInstances.AddRange(attributeInstances);
                param.AttributeInstances.AddRange(attributeInstances);
                var genericParameterAttributeDependencies = attributeInstances.Select(
                    attributeInstance => new AttributeTypeDependency(type, attributeInstance)
                );
                type.Dependencies.AddRange(genericParameterAttributeDependencies);
            }
        }

        private static void CollectAttributesForMembers(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver,
            IReadOnlyList<(MethodMember Member, MethodDefinition Definition)> methodPairs
        )
        {
            typeDefinition
                .Fields.Where(x => !x.IsBackingField() && !x.IsCompilerGenerated())
                .ForEach(fieldDef => SetUpAttributesForField(type, fieldDef, domainResolver));

            typeDefinition
                .Properties.Where(x => !x.IsCompilerGenerated())
                .ForEach(propDef => SetUpAttributesForProperty(type, propDef, domainResolver));

            // Build a lookup from method full-name -> MethodMember to avoid O(n) scans
            var methodMemberByFullName = new Dictionary<string, MethodMember>(methodPairs.Count);
            foreach (var pair in methodPairs)
            {
                var key = pair.Definition.BuildFullName();
                if (!methodMemberByFullName.ContainsKey(key))
                {
                    methodMemberByFullName[key] = pair.Member;
                }
            }

            typeDefinition
                .Methods.Where(x => !x.IsCompilerGenerated())
                .ForEach(methodDef =>
                {
                    MethodMember methodMember;
                    if (
                        !methodMemberByFullName.TryGetValue(
                            methodDef.BuildFullName(),
                            out methodMember
                        )
                    )
                    {
                        return;
                    }

                    SetUpAttributesForMethod(methodDef, methodMember, type, domainResolver);
                });
        }

        private static void SetUpAttributesForField(
            IType type,
            FieldDefinition fieldDefinition,
            DomainResolver domainResolver
        )
        {
            var fieldMember = type.GetFieldMembers()
                .WhereFullNameIs(fieldDefinition.FullName)
                .RequiredNotNull();
            CollectMemberAttributesAndDependencies(
                type,
                fieldMember,
                fieldDefinition.CustomAttributes.ToList(),
                fieldMember.MemberDependencies,
                domainResolver
            );
        }

        private static void SetUpAttributesForProperty(
            IType type,
            PropertyDefinition propertyDefinition,
            DomainResolver domainResolver
        )
        {
            var propertyMember = type.GetPropertyMembers()
                .WhereFullNameIs(propertyDefinition.FullName)
                .RequiredNotNull();
            CollectMemberAttributesAndDependencies(
                type,
                propertyMember,
                propertyDefinition.CustomAttributes.ToList(),
                propertyMember.AttributeDependencies,
                domainResolver
            );
        }

        private static void SetUpAttributesForMethod(
            MethodDefinition methodDefinition,
            MethodMember methodMember,
            IType type,
            DomainResolver domainResolver
        )
        {
            var memberCustomAttributes = methodDefinition.GetAllMethodCustomAttributes().ToList();
            SetUpAttributesForMethodGenericParameters(
                methodDefinition,
                methodMember,
                domainResolver
            );
            CollectMemberAttributesAndDependencies(
                type,
                methodMember,
                memberCustomAttributes,
                methodMember.MemberDependencies,
                domainResolver
            );
        }

        private static void SetUpAttributesForMethodGenericParameters(
            MethodDefinition methodDefinition,
            MethodMember methodMember,
            DomainResolver domainResolver
        )
        {
            foreach (var genericParameter in methodDefinition.GenericParameters)
            {
                var param = methodMember.GenericParameters.First(parameter =>
                    parameter.Name == genericParameter.Name
                );
                var customAttributes = genericParameter.CustomAttributes;
                customAttributes.ForEach(attr =>
                    AddAttributeArgumentReferenceDependencies(
                        methodMember.DeclaringType,
                        attr,
                        domainResolver
                    )
                );
                var attributeInstances = CreateAttributesFromCustomAttributes(
                        customAttributes,
                        domainResolver
                    )
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

        private static void CollectMemberAttributesAndDependencies(
            IType type,
            IMember member,
            List<CustomAttribute> memberCustomAttributes,
            List<IMemberTypeDependency> attributeDependencies,
            DomainResolver domainResolver
        )
        {
            memberCustomAttributes.ForEach(attr =>
                AddAttributeArgumentReferenceDependencies(type, attr, domainResolver)
            );
            var memberAttributeInstances = CreateAttributesFromCustomAttributes(
                    memberCustomAttributes,
                    domainResolver
                )
                .ToList();
            member.AttributeInstances.AddRange(memberAttributeInstances);
            var methodAttributeDependencies = memberAttributeInstances.Select(
                attributeInstance => new AttributeMemberDependency(member, attributeInstance)
            );
            attributeDependencies.AddRange(methodAttributeDependencies);
        }

        [NotNull]
        private static IEnumerable<AttributeInstance> CreateAttributesFromCustomAttributes(
            IEnumerable<CustomAttribute> customAttributes,
            DomainResolver domainResolver
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
                .Select(attr => attr.CreateAttributeFromCustomAttribute(domainResolver));
        }

        private static void AddAttributeArgumentReferenceDependencies(
            IType type,
            ICustomAttribute customAttribute,
            DomainResolver domainResolver
        )
        {
            if (!customAttribute.HasConstructorArguments)
            {
                return;
            }

            var attributeConstructorArgs = customAttribute.ConstructorArguments;
            attributeConstructorArgs
                .Where(attributeArgument =>
                    attributeArgument.Value is TypeReference typeReference
                    && !typeReference.IsCompilerGenerated()
                )
                .Select(attributeArgument =>
                    (typeReference: attributeArgument.Value as TypeReference, attributeArgument)
                )
                .ForEach(tuple =>
                {
                    var argumentType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                        tuple.typeReference
                    );
                    var dependency = new TypeReferenceDependency(type, argumentType);
                    type.Dependencies.Add(dependency);
                });
        }
    }
}
