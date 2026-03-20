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

namespace ArchUnitNET.Loader.LoadTasks
{
    /// <summary>
    /// Resolves method signature and body dependencies for each method in the type,
    /// including method calls, body types, cast types, type checks, metadata types,
    /// and accessed fields. Also links getter/setter methods to their properties.
    /// </summary>
    internal static class AddMethodDependencies
    {
        internal static void Execute(
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
    }
}
