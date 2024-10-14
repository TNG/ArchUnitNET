//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddMethodDependencies : ILoadTask
    {
        private readonly IType _type;
        private readonly System.Type _systemType;
        private readonly TypeFactory _typeFactory;

        public AddMethodDependencies(
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
            _systemType.GetMethods()
                .Select(methodInfo =>
                    (
                        methodMember: _type.GetMethodMembers().SingleOrDefault(member => member.Name == methodInfo.Name),
                        methodInfo
                    )
                )
                .Select(tuple =>
                {
                    var (methodMember, methodInfo) = tuple;
                    var dependencies = CreateMethodSignatureDependencies(
                            methodInfo,
                            methodMember
                        )
                        .Concat(CreateMethodBodyDependencies(methodInfo, methodMember));
                    if (methodInfo.IsSetter || methodInfo.IsGetter)
                    {
                        AssignDependenciesToProperty(methodMember, methodInfo);
                    }
                    return (methodMember, dependencies);
                })
                .ForEach(tuple =>
                {
                    var (methodMember, dependencies) = tuple;
                    methodMember.MemberDependencies.AddRange(dependencies);
                });
        }

        private void AssignDependenciesToProperty(
            MethodMember methodMember,
            MethodDefinition methodDefinition
        )
        {
            var methodForm = methodDefinition.GetMethodForm();
            var matchFunction = GetMatchFunction(methodForm);
            matchFunction.RequiredNotNull();

            var accessedProperty = MatchToPropertyMember(
                methodMember.Name,
                methodMember.FullName,
                matchFunction
            );
            if (accessedProperty == null)
            {
                return;
            }

            accessedProperty.IsVirtual = accessedProperty.IsVirtual || methodMember.IsVirtual;

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
        private IEnumerable<MethodSignatureDependency> CreateMethodSignatureDependencies(
            MethodInfo methodInfo,
            MethodMember methodMember
        )
        {
            var parameterDependencies= methodInfo.GetParameters().Select(parameter => _typeFactory.GetOrCreateStubTypeFromSystemType(parameter.ParameterType))
                .Select(signatureType => new MethodSignatureDependency(
                    methodMember,
                    signatureType
                ));
            var returnTypeDependency = new MethodSignatureDependency(
                methodMember,
                _typeFactory.GetOrCreateStubTypeFromSystemType(methodInfo.ReturnType)
            );
            return parameterDependencies.Append(returnTypeDependency);
        }

        [NotNull]
        private IEnumerable<IMemberTypeDependency> CreateMethodBodyDependencies(
            MethodInfo methodInfo,
            MethodMember methodMember
        )
        {
            var methodBody = methodInfo.GetMethodBody();
            if (methodBody == null)
            {
                yield break;
            }

            var visitedMethodReferences = new List<MethodInfo> { methodInfo };
            var bodyTypes = new List<ITypeInstance<IType>>();
            if (methodInfo.IsAsync())
            {
                HandleAsync(
                    out methodInfo,
                    ref methodBody,
                    bodyTypes,
                    visitedMethodReferences
                );
            }

            if (methodInfo.IsIterator())
            {
                HandleIterator(
                    out methodInfo,
                    ref methodBody,
                    bodyTypes,
                    visitedMethodReferences
                );
            }

            bodyTypes.AddRange(methodInfo.GetBodyTypes(_typeFactory).ToList());

            var castTypes = methodInfo.GetCastTypes(_typeFactory).ToList();

            var typeCheckTypes = methodInfo.GetTypeCheckTypes(_typeFactory).ToList();

            var metaDataTypes = methodInfo.GetMetaDataTypes(_typeFactory).ToList();

            var accessedFieldMembers = methodInfo
                .GetAccessedFieldMembers(_typeFactory)
                .ToList();

            var calledMethodMembers = CreateMethodBodyDependenciesRecursive(
                methodBody,
                visitedMethodReferences,
                bodyTypes,
                castTypes,
                typeCheckTypes,
                metaDataTypes,
                accessedFieldMembers
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

        private IEnumerable<MethodMemberInstance> CreateMethodBodyDependenciesRecursive(
            MethodBody methodBody,
            ICollection<MethodReference> visitedMethodReferences,
            List<ITypeInstance<IType>> bodyTypes,
            List<ITypeInstance<IType>> castTypes,
            List<ITypeInstance<IType>> typeCheckTypes,
            List<ITypeInstance<IType>> metaDataTypes,
            List<FieldMember> accessedFieldMembers
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
                            visitedMethodReferences
                        );
                    }

                    bodyTypes.AddRange(calledMethodDefinition.GetBodyTypes(_typeFactory));
                    castTypes.AddRange(calledMethodDefinition.GetCastTypes(_typeFactory));
                    typeCheckTypes.AddRange(calledMethodDefinition.GetTypeCheckTypes(_typeFactory));
                    metaDataTypes.AddRange(calledMethodDefinition.GetMetaDataTypes(_typeFactory));
                    accessedFieldMembers.AddRange(
                        calledMethodDefinition.GetAccessedFieldMembers(_typeFactory)
                    );

                    foreach (
                        var dep in CreateMethodBodyDependenciesRecursive(
                            calledMethodBody,
                            visitedMethodReferences,
                            bodyTypes,
                            castTypes,
                            typeCheckTypes,
                            metaDataTypes,
                            accessedFieldMembers
                        )
                    )
                    {
                        yield return dep;
                    }
                }
                else
                {
                    var calledType = _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        calledMethodReference.DeclaringType
                    );
                    var calledMethodMember =
                        _typeFactory.GetOrCreateMethodMemberFromMethodReference(
                            calledType,
                            calledMethodReference
                        );
                    yield return calledMethodMember;
                }
            }
        }

        private void HandleIterator(
            out MethodDefinition methodDefinition,
            ref MethodBody methodBody,
            List<ITypeInstance<IType>> bodyTypes,
            ICollection<MethodReference> visitedMethodReferences
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
                    _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(bodyField.FieldType)
                )
            );
        }

        private void HandleAsync(
            out MethodDefinition methodDefinition,
            ref MethodBody methodBody,
            List<ITypeInstance<IType>> bodyTypes,
            ICollection<MethodReference> visitedMethodReferences
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
                    _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(bodyField.FieldType)
                )
            );
        }

        private static MatchFunction GetMatchFunction(MethodForm methodForm)
        {
            MatchFunction matchFunction;
            switch (methodForm)
            {
                case MethodForm.Getter:
                    matchFunction = new MatchFunction(RegexUtils.MatchGetPropertyName);
                    break;
                case MethodForm.Setter:
                    matchFunction = new MatchFunction(RegexUtils.MatchSetPropertyName);
                    break;
                default:
                    matchFunction = null;
                    break;
            }

            return matchFunction.RequiredNotNull();
        }

        private PropertyMember MatchToPropertyMember(
            string name,
            string fullName,
            MatchFunction matchFunction
        )
        {
            try
            {
                var accessedMemberName = matchFunction.MatchNameFunction(name);
                if (accessedMemberName != null)
                {
                    var foundNameMatches = _type
                        .GetPropertyMembersWithName(accessedMemberName)
                        .SingleOrDefault();
                    if (foundNameMatches != null)
                    {
                        return foundNameMatches;
                    }
                }
            }
            catch (InvalidOperationException) { }

            var accessedMemberFullName = matchFunction.MatchNameFunction(fullName);
            return accessedMemberFullName != null
                ? GetPropertyMemberWithFullNameEndingWith(_type, accessedMemberFullName)
                : null;
        }

        private PropertyMember GetPropertyMemberWithFullNameEndingWith(
            IType type,
            string detailedName
        )
        {
            return type
                .Members.OfType<PropertyMember>()
                .FirstOrDefault(propertyMember => propertyMember.FullName.EndsWith(detailedName));
        }
    }

    public class MatchFunction
    {
        public MatchFunction(Func<string, string> matchNameFunction)
        {
            MatchNameFunction = matchNameFunction;
        }

        public Func<string, string> MatchNameFunction { get; }
    }
}
