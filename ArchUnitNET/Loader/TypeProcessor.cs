using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ArchUnitNET.Loader
{
    /// <summary>
    /// Contains all type-processing phases that populate the domain model with members,
    /// dependencies, and attributes. Each public method corresponds to a numbered phase
    /// and must be called in the required order (see <see cref="ArchBuilder.ProcessTypes"/>).
    /// </summary>
    internal static class TypeProcessor
    {
        // ── Phase 1: Base class dependency ──────────────────────────────────

        /// <summary>
        /// If the type has a base class, creates an <see cref="InheritsBaseClassDependency"/>
        /// and adds it to the type's dependency list. Skipped for interfaces and types
        /// whose base is not a <see cref="Class"/>.
        /// </summary>
        internal static void AddBaseClassDependency(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            var baseTypeRef = typeDefinition.BaseType;
            if (baseTypeRef == null)
            {
                return;
            }

            var baseType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(baseTypeRef);
            if (!(baseType.Type is Class baseClass))
            {
                return;
            }

            var dependency = new InheritsBaseClassDependency(
                type,
                new TypeInstance<Class>(
                    baseClass,
                    baseType.GenericArguments,
                    baseType.ArrayDimensions
                )
            );
            type.Dependencies.Add(dependency);
        }

        // ── Phase 2: Members ────────────────────────────────────────────────

        /// <summary>
        /// Creates field, property, and method members from the Cecil
        /// <see cref="TypeDefinition"/> and adds them to the domain type.
        /// Returns a <see cref="MemberData"/> containing method pairs and
        /// a mapping from getter/setter <see cref="MethodDefinition"/>s to
        /// their <see cref="PropertyMember"/>s.
        /// </summary>
        internal static MemberData AddMembers(
            ITypeInstance<IType> typeInstance,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            var methodPairs = new List<(MethodMember Member, MethodDefinition Definition)>();
            var propertyByAccessor = new Dictionary<MethodDefinition, PropertyMember>();
            var members = CreateMembers(
                typeInstance,
                typeDefinition,
                domainResolver,
                methodPairs,
                propertyByAccessor
            );
            typeInstance.Type.Members.AddRange(members);
            return new MemberData(methodPairs, propertyByAccessor);
        }

        [NotNull]
        private static IEnumerable<IMember> CreateMembers(
            ITypeInstance<IType> typeInstance,
            [NotNull] TypeDefinition typeDefinition,
            DomainResolver domainResolver,
            List<(MethodMember Member, MethodDefinition Definition)> methodPairs,
            Dictionary<MethodDefinition, PropertyMember> propertyByAccessor
        )
        {
            var fieldMembers = typeDefinition
                .Fields.Where(fieldDefinition => !fieldDefinition.IsBackingField())
                .Select(fieldDef =>
                    (IMember)domainResolver.GetOrCreateFieldMember(typeInstance.Type, fieldDef)
                );

            var propertyMembers = typeDefinition.Properties.Select(propDef =>
            {
                var propertyMember = CreatePropertyMember(typeInstance, propDef, domainResolver);
                if (propDef.GetMethod != null)
                {
                    propertyByAccessor[propDef.GetMethod] = propertyMember;
                }

                if (propDef.SetMethod != null)
                {
                    propertyByAccessor[propDef.SetMethod] = propertyMember;
                }

                return (IMember)propertyMember;
            });

            var methodMembers = typeDefinition.Methods.Select(method =>
            {
                var member = domainResolver
                    .GetOrCreateMethodMemberFromMethodReference(typeInstance, method)
                    .Member;
                methodPairs.Add((member, method));
                return (IMember)member;
            });

            return fieldMembers
                .Concat(propertyMembers)
                .Concat(methodMembers)
                .Where(member => !member.IsCompilerGenerated);
        }

        [NotNull]
        private static PropertyMember CreatePropertyMember(
            ITypeInstance<IType> typeInstance,
            PropertyDefinition propertyDefinition,
            DomainResolver domainResolver
        )
        {
            var typeReference = propertyDefinition.PropertyType;
            var propertyType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                typeReference
            );
            var isCompilerGenerated = propertyDefinition.IsCompilerGenerated();
            var isStatic =
                (propertyDefinition.SetMethod != null && propertyDefinition.SetMethod.IsStatic)
                || (propertyDefinition.GetMethod != null && propertyDefinition.GetMethod.IsStatic);
            var writeAccessor = GetPropertyWriteAccessor(propertyDefinition);
            return new PropertyMember(
                typeInstance.Type,
                propertyDefinition.Name,
                propertyDefinition.FullName,
                propertyType,
                isCompilerGenerated,
                isStatic,
                writeAccessor
            );
        }

        private static Writability GetPropertyWriteAccessor(
            [NotNull] PropertyDefinition propertyDefinition
        )
        {
            if (propertyDefinition.SetMethod == null)
            {
                return Writability.ReadOnly;
            }

            if (CheckPropertyHasInitSetterInNetStandardCompatibleWay(propertyDefinition))
            {
                return Writability.InitOnly;
            }

            return Writability.Writable;
        }

        private static bool CheckPropertyHasInitSetterInNetStandardCompatibleWay(
            PropertyDefinition propertyDefinition
        )
        {
            return propertyDefinition.SetMethod?.ReturnType.IsRequiredModifier == true
                && ((RequiredModifierType)propertyDefinition.SetMethod.ReturnType)
                    .ModifierType
                    .FullName == "System.Runtime.CompilerServices.IsExternalInit";
        }

        // ── Phase 3: Generic parameter dependencies ─────────────────────────

        /// <summary>
        /// Assigns declarers to generic parameters and creates type-constraint dependencies
        /// for both type-level and member-level generic parameters.
        /// </summary>
        internal static void AddGenericParameterDependencies(IType type)
        {
            // Type-level generic parameters
            foreach (var genericParameter in type.GenericParameters)
            {
                genericParameter.AssignDeclarer(type);
                foreach (var typeInstanceConstraint in genericParameter.TypeInstanceConstraints)
                {
                    var dependency = new TypeGenericParameterTypeConstraintDependency(
                        genericParameter,
                        typeInstanceConstraint
                    );
                    genericParameter.Dependencies.Add(dependency);
                }
            }

            // Member-level generic parameters
            foreach (var member in type.Members)
            {
                foreach (var genericParameter in member.GenericParameters)
                {
                    genericParameter.AssignDeclarer(member);
                    foreach (var typeInstanceConstraint in genericParameter.TypeInstanceConstraints)
                    {
                        var dependency = new MemberGenericParameterTypeConstraintDependency(
                            genericParameter,
                            typeInstanceConstraint
                        );
                        genericParameter.Dependencies.Add(dependency);
                    }
                }
            }
        }

        // ── Phase 4: Attributes and attribute dependencies ──────────────────

        /// <summary>
        /// Creates attribute instances from Cecil custom attributes on the type, its generic
        /// parameters, and its members, then adds corresponding attribute-type dependencies.
        /// </summary>
        internal static void AddAttributesAndAttributeDependencies(
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

        // ── Phase 5: Assembly-level attributes ──────────────────────────────

        /// <summary>
        /// Creates attribute instances from the assembly-level custom attributes of the
        /// given <see cref="AssemblyDefinition"/> and adds them to the domain
        /// <see cref="Assembly"/>.
        /// </summary>
        internal static void CollectAssemblyAttributes(
            Assembly assembly,
            AssemblyDefinition assemblyDefinition,
            DomainResolver domainResolver
        )
        {
            var attributeInstances = assemblyDefinition
                .CustomAttributes.Select(attr =>
                    attr.CreateAttributeFromCustomAttribute(domainResolver)
                )
                .ToList();
            assembly.AttributeInstances.AddRange(attributeInstances);
        }

        // ── Phase 6: Field and property type dependencies ───────────────────

        /// <summary>
        /// Creates <see cref="FieldTypeDependency"/> and <see cref="PropertyTypeDependency"/>
        /// instances for each field and property member of the type.
        /// </summary>
        internal static void AddFieldAndPropertyDependencies(IType type)
        {
            type.GetFieldMembers()
                .ForEach(field =>
                {
                    var dependency = new FieldTypeDependency(field);
                    if (!field.MemberDependencies.Contains(dependency))
                    {
                        field.MemberDependencies.Add(dependency);
                    }
                });

            type.GetPropertyMembers()
                .ForEach(property =>
                {
                    var dependency = new PropertyTypeDependency(property);
                    if (!property.MemberDependencies.Contains(dependency))
                    {
                        property.MemberDependencies.Add(dependency);
                    }
                });
        }

        // ── Phase 7: Method signature and body dependencies ─────────────────

        /// <summary>
        /// Resolves method signature and body dependencies for each method in the type,
        /// including method calls, body types, cast types, type checks, metadata types,
        /// and accessed fields. Also links getter/setter methods to their properties.
        /// </summary>
        internal static void AddMethodDependencies(
            IType type,
            DomainResolver domainResolver,
            IReadOnlyList<(MethodMember Member, MethodDefinition Definition)> methodPairs,
            IReadOnlyDictionary<MethodDefinition, PropertyMember> propertyByAccessor
        )
        {
            methodPairs
                .Where(pair => pair.Member != null && !pair.Member.IsCompilerGenerated)
                .Select(pair =>
                {
                    var dependencies = CreateMethodSignatureDependencies(
                            pair.Definition,
                            pair.Member,
                            domainResolver
                        )
                        .Concat(
                            CreateMethodBodyDependencies(
                                type,
                                pair.Definition,
                                pair.Member,
                                domainResolver
                            )
                        );
                    if (pair.Definition.IsSetter || pair.Definition.IsGetter)
                    {
                        AssignDependenciesToProperty(
                            pair.Member,
                            pair.Definition,
                            propertyByAccessor
                        );
                    }

                    return (pair.Member, dependencies);
                })
                .ForEach(tuple =>
                {
                    var (methodMember, dependencies) = tuple;
                    methodMember.MemberDependencies.AddRange(dependencies);
                });
        }

        private static void AssignDependenciesToProperty(
            MethodMember methodMember,
            MethodDefinition methodDefinition,
            IReadOnlyDictionary<MethodDefinition, PropertyMember> propertyByAccessor
        )
        {
            if (!propertyByAccessor.TryGetValue(methodDefinition, out var accessedProperty))
            {
                return;
            }

            accessedProperty.IsVirtual |= methodMember.IsVirtual;

            var methodForm = methodDefinition.GetMethodForm();
            switch (methodForm)
            {
                case MethodForm.Getter:
                    accessedProperty.Getter = methodMember;
                    break;
                case MethodForm.Setter:
                    accessedProperty.Setter = methodMember;
                    break;
            }

            var methodBody = methodDefinition.Body;

            if (methodBody == null)
            {
                return;
            }

            if (
                !methodBody
                    .Instructions.Select(instruction => instruction.Operand)
                    .OfType<FieldDefinition>()
                    .Any(definition => definition.IsBackingField())
            )
            {
                accessedProperty.IsAutoProperty = false;
            }
        }

        [NotNull]
        private static IEnumerable<MethodSignatureDependency> CreateMethodSignatureDependencies(
            MethodReference methodReference,
            MethodMember methodMember,
            DomainResolver domainResolver
        )
        {
            var returnType = methodReference.GetReturnType(domainResolver);
            return (returnType != null ? new[] { returnType } : Array.Empty<ITypeInstance<IType>>())
                .Concat(methodReference.GetParameters(domainResolver))
                .Concat(methodReference.GetGenericParameters(domainResolver))
                .Distinct()
                .Select(signatureType => new MethodSignatureDependency(
                    methodMember,
                    signatureType
                ));
        }

        [NotNull]
        private static IEnumerable<IMemberTypeDependency> CreateMethodBodyDependencies(
            IType type,
            MethodDefinition methodDefinition,
            MethodMember methodMember,
            DomainResolver domainResolver
        )
        {
            var methodBody = methodDefinition.Body;
            if (methodBody == null)
            {
                yield break;
            }

            var visitedMethodReferences = new List<MethodReference> { methodDefinition };
            var bodyTypes = new List<ITypeInstance<IType>>();

            if (methodDefinition.IsAsync())
            {
                HandleAsync(
                    out methodDefinition,
                    ref methodBody,
                    bodyTypes,
                    visitedMethodReferences,
                    domainResolver
                );
            }

            if (methodDefinition.IsIterator())
            {
                HandleIterator(
                    out methodDefinition,
                    ref methodBody,
                    bodyTypes,
                    visitedMethodReferences,
                    domainResolver
                );
            }

            var scan = methodDefinition.ScanMethodBody(domainResolver);
            bodyTypes.AddRange(scan.BodyTypes);

            var castTypes = scan.CastTypes;

            var typeCheckTypes = scan.TypeCheckTypes;

            var metaDataTypes = scan.MetaDataTypes;

            var accessedFieldMembers = scan.AccessedFieldMembers;

            var calledMethodMembers = CreateMethodBodyDependenciesRecursive(
                methodBody,
                visitedMethodReferences,
                bodyTypes,
                castTypes,
                typeCheckTypes,
                metaDataTypes,
                accessedFieldMembers,
                domainResolver
            );

            foreach (
                var calledMethodMember in calledMethodMembers
                    .Where(method => !method.Member.IsCompilerGenerated)
                    .Distinct()
            )
            {
                yield return new MethodCallDependency(methodMember, calledMethodMember);
            }

            foreach (
                var bodyType in bodyTypes
                    .Where(instance => !instance.Type.IsCompilerGenerated)
                    .Distinct()
            )
            {
                yield return new BodyTypeMemberDependency(methodMember, bodyType);
            }

            foreach (
                var castType in castTypes
                    .Where(instance => !instance.Type.IsCompilerGenerated)
                    .Distinct()
            )
            {
                yield return new CastTypeDependency(methodMember, castType);
            }

            foreach (
                var typeCheckType in typeCheckTypes
                    .Where(instance => !instance.Type.IsCompilerGenerated)
                    .Distinct()
            )
            {
                yield return new TypeCheckDependency(methodMember, typeCheckType);
            }

            foreach (
                var metaDataType in metaDataTypes
                    .Where(instance => !instance.Type.IsCompilerGenerated)
                    .Distinct()
            )
            {
                yield return new MetaDataDependency(methodMember, metaDataType);
            }

            foreach (
                var fieldMember in accessedFieldMembers
                    .Where(field => !field.IsCompilerGenerated)
                    .Distinct()
            )
            {
                yield return new AccessFieldDependency(methodMember, fieldMember);
            }
        }

        private static IEnumerable<MethodMemberInstance> CreateMethodBodyDependenciesRecursive(
            MethodBody methodBody,
            ICollection<MethodReference> visitedMethodReferences,
            List<ITypeInstance<IType>> bodyTypes,
            List<ITypeInstance<IType>> castTypes,
            List<ITypeInstance<IType>> typeCheckTypes,
            List<ITypeInstance<IType>> metaDataTypes,
            List<FieldMember> accessedFieldMembers,
            DomainResolver domainResolver
        )
        {
            var calledMethodReferences = methodBody
                .Instructions.Select(instruction => instruction.Operand)
                .OfType<MethodReference>();

            foreach (
                var calledMethodReference in calledMethodReferences.Except(visitedMethodReferences)
            )
            {
                visitedMethodReferences.Add(calledMethodReference);

                var calledType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                    calledMethodReference.DeclaringType
                );
                var calledMethodMember = domainResolver.GetOrCreateMethodMemberFromMethodReference(
                    calledType,
                    calledMethodReference
                );

                bodyTypes.AddRange(calledMethodMember.MemberGenericArguments);

                if (calledMethodReference.IsCompilerGenerated())
                {
                    MethodDefinition calledMethodDefinition;
                    try
                    {
                        calledMethodDefinition = calledMethodReference.Resolve();
                    }
                    catch (AssemblyResolutionException)
                    {
                        calledMethodDefinition = null;
                    }

                    if (calledMethodDefinition?.Body == null)
                    {
                        //MethodReference to compiler generated type not resolvable, skip
                        continue;
                    }

                    var calledMethodBody = calledMethodDefinition.Body;

                    if (calledMethodDefinition.IsIterator())
                    {
                        HandleIterator(
                            out calledMethodDefinition,
                            ref calledMethodBody,
                            bodyTypes,
                            visitedMethodReferences,
                            domainResolver
                        );
                    }

                    var calledScan = calledMethodDefinition.ScanMethodBody(domainResolver);
                    bodyTypes.AddRange(calledScan.BodyTypes);
                    castTypes.AddRange(calledScan.CastTypes);
                    typeCheckTypes.AddRange(calledScan.TypeCheckTypes);
                    metaDataTypes.AddRange(calledScan.MetaDataTypes);
                    accessedFieldMembers.AddRange(calledScan.AccessedFieldMembers);

                    foreach (
                        var dep in CreateMethodBodyDependenciesRecursive(
                            calledMethodBody,
                            visitedMethodReferences,
                            bodyTypes,
                            castTypes,
                            typeCheckTypes,
                            metaDataTypes,
                            accessedFieldMembers,
                            domainResolver
                        )
                    )
                    {
                        yield return dep;
                    }
                }
                else
                {
                    yield return calledMethodMember;
                }
            }
        }

        private static void HandleIterator(
            out MethodDefinition methodDefinition,
            ref MethodBody methodBody,
            List<ITypeInstance<IType>> bodyTypes,
            ICollection<MethodReference> visitedMethodReferences,
            DomainResolver domainResolver
        )
        {
            var compilerGeneratedGeneratorObject = (
                (MethodReference)methodBody.Instructions.First(inst => inst.IsNewObjectOp()).Operand
            ).DeclaringType.Resolve();
            methodDefinition = compilerGeneratedGeneratorObject.Methods.First(method =>
                method.Name == nameof(IEnumerator.MoveNext)
            );
            visitedMethodReferences.Add(methodDefinition);
            methodBody = methodDefinition.Body;

            var fieldsExceptGeneratorStateInfo = compilerGeneratedGeneratorObject.Fields.Where(
                field =>
                    !(
                        field.Name.EndsWith("__state")
                        || field.Name.EndsWith("__current")
                        || field.Name.EndsWith("__initialThreadId")
                        || field.Name.EndsWith("__this")
                    )
            );

            bodyTypes.AddRange(
                fieldsExceptGeneratorStateInfo.Select(bodyField =>
                    domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(bodyField.FieldType)
                )
            );
        }

        private static void HandleAsync(
            out MethodDefinition methodDefinition,
            ref MethodBody methodBody,
            List<ITypeInstance<IType>> bodyTypes,
            ICollection<MethodReference> visitedMethodReferences,
            DomainResolver domainResolver
        )
        {
            var compilerGeneratedGeneratorObject = (
                (MethodReference)
                    methodBody.Instructions.FirstOrDefault(inst => inst.IsNewObjectOp())?.Operand
            )?.DeclaringType.Resolve();

            if (compilerGeneratedGeneratorObject == null)
            {
                methodDefinition = methodBody.Method;
                return;
            }

            methodDefinition = compilerGeneratedGeneratorObject.Methods.First(method =>
                method.Name == nameof(IAsyncStateMachine.MoveNext)
            );

            visitedMethodReferences.Add(methodDefinition);
            methodBody = methodDefinition.Body;

            var fieldsExceptGeneratorStateInfo = compilerGeneratedGeneratorObject
                .Fields.Where(field =>
                    !(
                        field.Name.EndsWith("__state")
                        || field.Name.EndsWith("__builder")
                        || field.Name.EndsWith("__this")
                    )
                )
                .ToArray();

            bodyTypes.AddRange(
                fieldsExceptGeneratorStateInfo.Select(bodyField =>
                    domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(bodyField.FieldType)
                )
            );
        }

        // ── Phase 8: Generic argument dependencies ──────────────────────────

        /// <summary>
        /// Propagates generic-parameter dependencies to the owning type and member,
        /// then recursively discovers nested generic-argument dependencies in all
        /// existing type and member dependencies.
        /// </summary>
        internal static void AddGenericArgumentDependencies(IType type)
        {
            // Type-level generic argument dependencies
            foreach (var parameter in type.GenericParameters)
            {
                type.Dependencies.AddRange(parameter.Dependencies);
            }

            // Member-level generic argument dependencies
            foreach (var member in type.Members)
            {
                foreach (var parameter in member.GenericParameters)
                {
                    member.MemberDependencies.AddRange(
                        parameter.Dependencies.Cast<IMemberTypeDependency>()
                    );
                }
            }

            // Recursive generic arguments in type dependencies
            var typeDependencies = new List<GenericArgumentTypeDependency>();
            foreach (var dependency in type.Dependencies)
            {
                FindGenericArgumentsInTypeDependenciesRecursive(
                    type,
                    dependency.TargetGenericArguments,
                    typeDependencies
                );
            }

            type.Dependencies.AddRange(typeDependencies);

            // Recursive generic arguments in member dependencies
            foreach (var member in type.Members)
            {
                var memberDependencies = new List<GenericArgumentMemberDependency>();
                foreach (var dependency in member.Dependencies)
                {
                    FindGenericArgumentsInMemberDependenciesRecursive(
                        member,
                        dependency.TargetGenericArguments,
                        memberDependencies
                    );
                }

                member.MemberDependencies.AddRange(memberDependencies);
            }
        }

        private static void FindGenericArgumentsInTypeDependenciesRecursive(
            IType type,
            IEnumerable<GenericArgument> targetGenericArguments,
            ICollection<GenericArgumentTypeDependency> createdDependencies
        )
        {
            foreach (
                var genericArgument in targetGenericArguments.Where(argument =>
                    !argument.Type.IsGenericParameter
                )
            )
            {
                createdDependencies.Add(new GenericArgumentTypeDependency(type, genericArgument));
                FindGenericArgumentsInTypeDependenciesRecursive(
                    type,
                    genericArgument.GenericArguments,
                    createdDependencies
                );
            }
        }

        private static void FindGenericArgumentsInMemberDependenciesRecursive(
            IMember member,
            IEnumerable<GenericArgument> targetGenericArguments,
            ICollection<GenericArgumentMemberDependency> createdDependencies
        )
        {
            foreach (
                var genericArgument in targetGenericArguments.Where(argument =>
                    !argument.Type.IsGenericParameter
                )
            )
            {
                createdDependencies.Add(
                    new GenericArgumentMemberDependency(member, genericArgument)
                );
                FindGenericArgumentsInMemberDependenciesRecursive(
                    member,
                    genericArgument.GenericArguments,
                    createdDependencies
                );
            }
        }

        // ── Phase 9: Interface and member-to-type dependencies ──────────────

        /// <summary>
        /// Creates <see cref="ImplementsInterfaceDependency"/> for each interface the type
        /// implements (including inherited interfaces), then rolls up all member-level
        /// dependencies to the type's dependency list.
        /// </summary>
        internal static void AddClassDependencies(
            IType type,
            TypeDefinition typeDefinition,
            DomainResolver domainResolver
        )
        {
            // Interface dependencies
            GetInterfacesImplementedByClass(typeDefinition)
                .ForEach(target =>
                {
                    var targetType = domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                        target
                    );
                    type.Dependencies.Add(new ImplementsInterfaceDependency(type, targetType));
                });

            // Member dependencies rolled up to type level
            type.Members.ForEach(member =>
            {
                type.Dependencies.AddRange(member.Dependencies);
            });
        }

        private static IEnumerable<TypeReference> GetInterfacesImplementedByClass(
            TypeDefinition typeDefinition
        )
        {
            var baseType = typeDefinition.BaseType?.Resolve();
            var baseInterfaces =
                baseType != null
                    ? GetInterfacesImplementedByClass(baseType)
                    : new List<TypeReference>();

            return typeDefinition
                .Interfaces.Select(implementation => implementation.InterfaceType)
                .Concat(baseInterfaces);
        }

        // ── Phase 10: Backwards dependencies ────────────────────────────────

        /// <summary>
        /// Registers each of the type's dependencies as a backwards dependency on the
        /// target type, and each member-member dependency as a backwards dependency on
        /// the target member.
        /// </summary>
        internal static void AddBackwardsDependencies(IType type)
        {
            type.Dependencies.ForEach(dependency =>
                dependency.Target.BackwardsDependencies.Add(dependency)
            );

            var memberMemberDependencies = type
                .Members.SelectMany(member => member.MemberDependencies)
                .OfType<IMemberMemberDependency>();
            memberMemberDependencies.ForEach(memberDependency =>
                memberDependency.TargetMember.MemberBackwardsDependencies.Add(memberDependency)
            );
        }

        // ── Phase 11: Register types with namespaces ────────────────────────

        /// <summary>
        /// Adds each type to its namespace's type list. Must run after all types have been
        /// fully populated so that namespace queries return complete results.
        /// </summary>
        internal static void AddTypesToNamespaces(IEnumerable<IType> types)
        {
            foreach (var type in types)
            {
                ((List<IType>)type.Namespace.Types).Add(type);
            }
        }
    }

    /// <summary>
    /// Bundles the results of member creation: method (MethodMember, MethodDefinition) pairs
    /// and a mapping from getter/setter MethodDefinitions to their PropertyMembers.
    /// </summary>
    internal sealed class MemberData
    {
        public MemberData(
            IReadOnlyList<(MethodMember Member, MethodDefinition Definition)> methodPairs,
            IReadOnlyDictionary<MethodDefinition, PropertyMember> propertyByAccessor
        )
        {
            MethodPairs = methodPairs;
            PropertyByAccessor = propertyByAccessor;
        }

        public IReadOnlyList<(
            MethodMember Member,
            MethodDefinition Definition
        )> MethodPairs { get; }
        public IReadOnlyDictionary<MethodDefinition, PropertyMember> PropertyByAccessor { get; }
    }
}
