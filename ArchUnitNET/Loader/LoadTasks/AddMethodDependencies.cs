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
using ArchUnitNET.Domain.Extensions;
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

            foreach (var calledMethodMember in calledMethodMembers.Where(method => !method.Member.IsCompilerGenerated)
                .Distinct())
            {
                yield return new MethodCallDependency(methodMember, calledMethodMember);
            }

            foreach (var bodyType in bodyTypes.Where(instance => !instance.Type.IsCompilerGenerated).Distinct())
            {
                yield return new BodyTypeMemberDependency(methodMember, bodyType);
            }

            foreach (var referencedType in referencedTypes.Where(instance => !instance.Type.IsCompilerGenerated)
                .Distinct())
            {
                yield return new MemberTypeDependency(methodMember, referencedType);
            }

            foreach (var fieldMember in accessedFieldMembers.Where(field => !field.Type.IsCompilerGenerated).Distinct())
            {
                yield return new AccessFieldDependency(methodMember, fieldMember);
            }
        }


        private IEnumerable<MethodMemberInstance> CreateMethodBodyDependenciesRecursive(MethodBody methodBody,
            ICollection<MethodReference> visitedMethodReferences, List<TypeInstance<IType>> bodyTypes,
            List<TypeInstance<IType>> referencedTypes, List<FieldMember> accessedFieldMembers)
        {
            var calledMethodReferences = methodBody.Instructions.Select(instruction => instruction.Operand)
                .OfType<MethodReference>();

            foreach (var calledMethodReference in calledMethodReferences.Except(visitedMethodReferences))
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

                    if (calledMethodDefinition == null)
                    {
                        //MethodReference to compiler generated type not resolvable, skip
                        continue;
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
                    var calledType =
                        _typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(calledMethodReference.DeclaringType);
                    var calledMethodMember =
                        _typeFactory.GetOrCreateMethodMemberFromMethodReference(calledType, calledMethodReference);
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