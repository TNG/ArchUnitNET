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
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;
using GenericParameter = ArchUnitNET.Domain.GenericParameter;

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
                        .Concat(CreateMethodBodyDependencies(methodDefinition, methodMember));
                    if (methodDefinition.IsSetter || methodDefinition.IsGetter)
                    {
                        AssignDependenciesToProperty(methodMember, methodDefinition, dependencies);
                    }

                    return (methodMember, dependencies);
                })
                .ForEach(tuple =>
                {
                    var (methodMember, dependencies) = tuple;
                    methodMember.MemberDependencies.AddRange(dependencies);
                });
        }

        private void AssignDependenciesToProperty(MethodMember methodMember, MethodDefinition methodDefinition,
            IEnumerable<IMemberTypeDependency> dependencies)
        {
            var methodForm = methodDefinition.GetMethodForm();
            var matchFunction = GetMatchFunction(methodForm);
            matchFunction.RequiredNotNull();

            var accessedProperty =
                MatchToPropertyMember(methodMember.Name, methodMember.FullName, matchFunction);
            if (accessedProperty == null)
            {
                return;
            }

            //TODO add dependency to backing field
            if (methodForm == MethodForm.Getter)
            {
                accessedProperty.Getter = methodMember;
            }
            else if (methodForm == MethodForm.Setter)
            {
                accessedProperty.Setter = methodMember;
            }
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
        private IEnumerable<IMemberTypeDependency> CreateMethodBodyDependencies(MethodDefinition methodDefinition,
            MethodMember methodMember)
        {
            var methodBody = methodDefinition.Resolve().Body;
            if (methodBody == null)
            {
                yield break;
            }

            var bodyTypes = methodDefinition.GetBodyTypes(_typeFactory).ToList();

            var calledMethodMembers = CreateMethodBodyDependenciesRecursive(methodMember, methodBody,
                new List<MethodReference>(),
                bodyTypes);

            //HandlePropertyBackingFieldDependencies(methodBody);

            // if (methodDefinition.IsSetter || methodDefinition.IsGetter)
            // {
            //     AssignDependenciesToAccessedProperty(methodMember, methodBody, methodDefinition.GetMethodForm());
            // }
            // else
            // {
            foreach (var calledMethodMember in calledMethodMembers)
            {
                yield return new MethodCallDependency(methodMember, calledMethodMember);
            }

            foreach (var bodyType in bodyTypes)
            {
                yield return new BodyTypeMemberDependency(methodMember, bodyType);
            }

            // }
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

                    var calledMember = GetMethodMemberWithMethodReference(calledType, methodReference);

                    var dependency = new MethodCallDependency(backedProperty, calledMember);
                    backedProperty.MemberDependencies.Add(dependency);
                });
        }

        private IEnumerable<MethodMember> CreateMethodBodyDependenciesRecursive(MethodMember methodMember,
            MethodBody methodBody, ICollection<MethodReference> visitedMethodReferences, ICollection<IType> bodyTypes)
        {
            var calledMethodReferences = methodBody.Instructions.Select(instruction => instruction.Operand)
                .OfType<MethodReference>();

            foreach (var calledMethodReference in calledMethodReferences.Except(visitedMethodReferences))
            {
                visitedMethodReferences.Add(calledMethodReference);
                var calledType =
                    _typeFactory.GetOrCreateStubTypeFromTypeReference(calledMethodReference.DeclaringType);

                if (calledType.NameContains("<")) //compilerGenerated
                {
                    if (!(calledMethodReference is MethodDefinition calledMethodDefinition))
                    {
                        calledMethodDefinition = calledMethodReference.Resolve();
                        if (calledMethodDefinition == null)
                        {
                            //MethodReference to compiler generated type not resolvable, skip
                            continue;
                        }
                    }

                    bodyTypes = bodyTypes.Union(calledMethodDefinition.GetBodyTypes(_typeFactory)).ToList();

                    foreach (var dep in CreateMethodBodyDependenciesRecursive(methodMember, calledMethodDefinition.Body,
                        visitedMethodReferences, bodyTypes))
                    {
                        yield return dep;
                    }
                }
                else
                {
                    var calledMethodMember = GetMethodMemberWithMethodReference(calledType, calledMethodReference);
                    yield return calledMethodMember;
                }
            }
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

                    var calledMember = GetMethodMemberWithMethodReference(calledType, methodReference);

                    return new MethodCallDependency(accessedProperty, calledMember);
                });
        }

        [NotNull]
        private MethodMember GetMethodMemberWithMethodReference([NotNull] IType type,
            [NotNull] MethodReference methodReference)
        {
            var matchingMethods = type.GetMethodMembers().Where(member => MatchesGeneric(member, methodReference))
                .ToList();

            if (!matchingMethods.Any())
            {
                var stubMethod = _typeFactory.CreateStubMethodMemberFromMethodReference(type, methodReference);
                return stubMethod;
            }

            if (matchingMethods.Count > 1)
            {
                throw new MultipleOccurrencesInSequenceException(
                    $"Multiple Methods matching {methodReference.FullName} found in provided type.");
            }

            return matchingMethods.First();
        }

        private bool MatchesGeneric(MethodMember methodMember, MethodReference methodReference)
        {
            var referenceFullName = methodReference.GetElementMethod().GetFullName();
            var memberFullName = methodMember.FullName;
            var count = methodReference.GetElementMethod().GenericParameters.Count;
            if (methodMember.GenericParameters.Count != count)
            {
                return false;
            }

            var parameters = new List<GenericParameter[]>();
            for (var i = 0; i < count; i++)
            {
                parameters.Add(new[]
                {
                    new GenericParameter(methodReference.GetElementMethod().GenericParameters[i].Name),
                    methodMember.GenericParameters[i]
                });
            }

            parameters = parameters.OrderByDescending(genericParameters => genericParameters[0].Name.Length).ToList();

            foreach (var genericParameters in parameters.Where(genericParameters => genericParameters[0] != null)
            )
            {
                referenceFullName = referenceFullName.Replace(genericParameters[0].Name, genericParameters[1].Name);
                memberFullName = memberFullName.Replace(genericParameters[0].Name, genericParameters[1].Name);
            }

            return memberFullName.Equals(referenceFullName);
        }

        private FieldMember FindMatchingField(FieldDefinition fieldDefinition)
        {
            return _type.GetFieldMembersWithName(fieldDefinition.Name).SingleOrDefault();
        }

        private PropertyMember
            MatchToPropertyMember(string name, string fullName,
                MatchFunction matchFunction) //TODO get_Item for list enumerator func is not matched correctly
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