//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Fluent.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace ArchUnitNET.Loader.LoadTasks
{
    internal class AddMethodDependencies : ILoadTask
    {
        private readonly IType _type;
        private readonly TypeDefinition _typeDefinition;
        private readonly TypeFactory _typeFactory;

        public AddMethodDependencies(IType type, TypeDefinition typeDefinition, TypeFactory typeFactory)
        {
            _type = type;
            _typeDefinition = typeDefinition;
            _typeFactory = typeFactory;
        }

        public void Execute()
        {
            _typeDefinition.Methods
                .Where(methodDefinition => _type.GetMemberWithFullName(methodDefinition.FullName) is MethodMember)
                .Select(definition => (methodMember: _type.GetMemberWithFullName(definition.FullName) as MethodMember,
                    methodDefinition: definition))
                .Select(tuple =>
                {
                    var (methodMember, methodDefinition) = tuple;
                    var dependencies = CreateMethodSignatureDependencies(methodDefinition, methodMember)
                        .Concat<IMemberTypeDependency>(CreateMethodBodyTypeDependencies(methodDefinition, methodMember))
                        .Concat(CreateMethodCallDependencies(methodDefinition, methodMember));
                    return (methodMember, dependencies);
                })
                .ForEach(tuple =>
                {
                    var (methodMember, dependencies) = tuple;
                    methodMember.MemberDependencies.AddRange(dependencies);
                });
        }

        [NotNull]
        private IEnumerable<MethodSignatureDependency> CreateMethodSignatureDependencies(
            MethodReference methodReference, MethodMember methodMember)
        {
            return methodReference
                .GetSignatureTypes(_typeFactory)
                .Select(signatureType => new MethodSignatureDependency(methodMember, signatureType));
        }

        [NotNull]
        private IEnumerable<BodyTypeMemberDependency> CreateMethodBodyTypeDependencies(
            MethodDefinition methodDefinition,
            MethodMember methodMember)
        {
            return methodDefinition
                .GetBodyTypes(_typeFactory)
                .Select(bodyType => new BodyTypeMemberDependency(methodMember, bodyType));
        }

        [NotNull]
        private IEnumerable<MethodCallDependency> CreateMethodCallDependencies(MethodDefinition methodDefinition,
            MethodMember methodMember)
        {
            var methodBody = methodDefinition.Resolve().Body;
            if (methodBody == null)
            {
                return Enumerable.Empty<MethodCallDependency>();
            }

            if (methodDefinition.IsSetter || methodDefinition.IsGetter)
            {
                AssignDependenciesToAccessedProperty(methodMember, methodBody, methodDefinition.GetMethodForm());
                return Enumerable.Empty<MethodCallDependency>();
            }

            HandlePropertyBackingFieldDependencies(methodBody);

            return CreateMethodCallDependenciesFromBody(methodMember, methodBody);
        }

        private void AssignDependenciesToAccessedProperty(MethodMember methodMember,
            MethodBody methodBody, MethodForm methodForm)
        {
            var matchFunction = GetMatchFunction(methodForm);
            matchFunction.RequiredNotNull();

            var accessedProperty =
                MatchToPropertyMember(methodMember.Name, methodMember.FullName, matchFunction);
            if (accessedProperty == null)
            {
                return;
            }

            var memberDependenciesToAdd = CreateMethodCallDependenciesForProperty(accessedProperty, methodBody)
                .ToList();

            methodBody.Instructions
                .Select(instruction => instruction.Operand)
                .OfType<FieldDefinition>()
                .ForEach(fieldDefinition =>
                {
                    var backingField = FindMatchingField(fieldDefinition);
                    accessedProperty.BackingField = backingField;
                });
            if (accessedProperty.BackingField != null && accessedProperty.BackingField.MemberDependencies.Count != 0)
            {
                memberDependenciesToAdd.AddRange(accessedProperty.BackingField.MemberDependencies);
            }

            if (methodForm == MethodForm.Getter)
            {
                accessedProperty.Getter?.MemberDependencies.AddRange(memberDependenciesToAdd);
            }
            else if (methodForm == MethodForm.Setter)
            {
                accessedProperty.Setter?.MemberDependencies.AddRange(memberDependenciesToAdd);
            }

            accessedProperty.MemberDependencies.AddRange(memberDependenciesToAdd);
        }

        private void HandlePropertyBackingFieldDependencies(MethodBody methodBody)
        {
            methodBody.Instructions
                .Where(instruction => instruction.Operand is MethodReference
                                      && instruction.IsOperationForBackedProperty())
                .Select(instruction => (methodReference: instruction.Operand as MethodReference,
                    methodBodyInstruction: instruction))
                .ForEach(tuple =>
                {
                    var (methodReference, methodBodyInstruction) = tuple;
                    var fieldDefinitionOp = methodBodyInstruction.GetAssigneeFieldDefinition();
                    if (fieldDefinitionOp == null)
                    {
                        return;
                    }

                    var backedProperty =
                        MatchToPropertyMember(fieldDefinitionOp.Name, fieldDefinitionOp.FullName,
                            GetFieldMatchFunctions());
                    if (backedProperty == null)
                    {
                        return;
                    }

                    var calledType =
                        _typeFactory.GetOrCreateStubTypeFromTypeReference(methodReference.DeclaringType);

                    var dependency =
                        CreateStubMethodCallDependencyForProperty(calledType, methodReference, backedProperty);
                    backedProperty.MemberDependencies.Add(dependency);
                });
        }

        private IEnumerable<MethodCallDependency> CreateMethodCallDependenciesFromBody(MethodMember methodMember,
            MethodBody methodBody)
        {
            return methodBody.Instructions
                .Select(instruction => instruction.Operand)
                .OfType<MethodReference>()
                .Select(methodReference =>
                {
                    var calledType =
                        _typeFactory.GetOrCreateStubTypeFromTypeReference(methodReference.DeclaringType);

                    return calledType.GetMethodMemberWithMethodReference(methodReference);
                })
                .Where(calledMethodMember => calledMethodMember != null)
                .Select(calledMethodMember => new MethodCallDependency(methodMember, calledMethodMember));
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

        private static MatchFunction GetFieldMatchFunctions()
        {
            return new MatchFunction(RegexUtils.MatchFieldName);
        }

        private IEnumerable<IMemberTypeDependency> CreateMethodCallDependenciesForProperty(
            PropertyMember accessedProperty, MethodBody methodBody)
        {
            return methodBody.Instructions
                .Where(instruction => instruction.Operand is MethodReference)
                .Where(instruction => instruction.IsMethodCallAssignment())
                .Select(instruction => (methodReference: instruction.Operand as MethodReference,
                    instruction))
                .Select<(MethodReference, Instruction), IMemberTypeDependency>(tuple =>
                {
                    var (methodReference, _) = tuple;
                    var calledType = _typeFactory.GetOrCreateStubTypeFromTypeReference(methodReference.DeclaringType);

                    return CreateStubMethodCallDependencyForProperty(calledType, methodReference, accessedProperty);
                });
        }

        private FieldMember FindMatchingField(FieldDefinition fieldDefinition)
        {
            return _type.GetFieldMembersWithName(fieldDefinition.Name).SingleOrDefault();
        }

        private PropertyMember MatchToPropertyMember(string name, string fullName, MatchFunction matchFunction)
        {
            try
            {
                var accessedMemberName = matchFunction.MatchNameFunction(name);
                if (accessedMemberName != null)
                {
                    var foundNameMatches = _type.GetPropertyMembersWithName(accessedMemberName).SingleOrDefault();
                    if (foundNameMatches != null)
                    {
                        return foundNameMatches;
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }

            var accessedMemberFullName = matchFunction.MatchNameFunction(fullName);
            return accessedMemberFullName != null
                ? GetPropertyMemberWithFullNameEndingWith(_type, accessedMemberFullName)
                : null;
        }

        private MethodCallDependency CreateStubMethodCallDependencyForProperty(IType calledType,
            MethodReference methodReference,
            PropertyMember backedProperty)
        {
            var calledMethodMember =
                _typeFactory.CreateStubMethodMemberFromMethodReference(calledType, methodReference);
            var dependency = new MethodCallDependency(backedProperty, calledMethodMember);
            return dependency;
        }

        private PropertyMember GetPropertyMemberWithFullNameEndingWith(IType type, string detailedName)
        {
            return type.Members.OfType<PropertyMember>().FirstOrDefault(propertyMember =>
                propertyMember.FullName.EndsWith(detailedName));
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