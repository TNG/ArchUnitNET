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
                        AssignDependenciesToProperty(methodMember, methodDefinition);
                    }

                    return (methodMember, dependencies);
                })
                .ForEach(tuple =>
                {
                    var (methodMember, dependencies) = tuple;
                    methodMember.MemberDependencies.AddRange(dependencies);
                });
        }

        private void AssignDependenciesToProperty(MethodMember methodMember, MethodDefinition methodDefinition)
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

            accessedProperty.IsVirtual = accessedProperty.IsVirtual || methodMember.IsVirtual;

            if (methodForm == MethodForm.Getter)
            {
                accessedProperty.Getter = methodMember;
            }
            else if (methodForm == MethodForm.Setter)
            {
                accessedProperty.Setter = methodMember;
            }

            var methodBody = methodDefinition.Body;

            if (methodBody == null)
            {
                return;
            }

            if (!methodBody.Instructions
                .Select(instruction => instruction.Operand).OfType<FieldDefinition>()
                .Any(definition => definition.IsBackingField()))
            {
                accessedProperty.IsAutoProperty = false;
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

            var visitedMethodReferences = new List<MethodReference> {methodDefinition};

            var bodyTypes = methodDefinition.GetBodyTypes(_typeFactory).ToList();

            var referencedTypes = methodDefinition.GetReferencedTypes(_typeFactory).ToList();

            var accessedFieldMembers = methodDefinition.GetAccessedFieldMembers(_typeFactory).ToList();

            var calledMethodMembers = CreateMethodBodyDependenciesRecursive(methodBody, visitedMethodReferences,
                bodyTypes, referencedTypes, accessedFieldMembers);

            foreach (var calledMethodMember in calledMethodMembers.Distinct())
            {
                yield return new MethodCallDependency(methodMember, calledMethodMember);
            }

            foreach (var bodyType in bodyTypes.Distinct())
            {
                yield return new BodyTypeMemberDependency(methodMember, bodyType);
            }

            foreach (var referencedType in referencedTypes.Distinct())
            {
                yield return new MemberTypeDependency(methodMember, referencedType);
            }

            foreach (var fieldMember in accessedFieldMembers.Distinct())
            {
                yield return new AccessFieldDependency(methodMember, fieldMember);
            }
        }


        private IEnumerable<MethodMember> CreateMethodBodyDependenciesRecursive(MethodBody methodBody,
            ICollection<MethodReference> visitedMethodReferences, List<IType> bodyTypes,
            List<IType> referencedTypes, List<FieldMember> accessedFieldMembers)
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

                    bodyTypes.AddRange(calledMethodDefinition.GetBodyTypes(_typeFactory));

                    referencedTypes.AddRange(calledMethodDefinition.GetReferencedTypes(_typeFactory));

                    accessedFieldMembers.AddRange(calledMethodDefinition.GetAccessedFieldMembers(_typeFactory));

                    foreach (var dep in CreateMethodBodyDependenciesRecursive(calledMethodDefinition.Body,
                        visitedMethodReferences, bodyTypes, referencedTypes, accessedFieldMembers))
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