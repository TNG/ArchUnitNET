//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Cil;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Loader
{
    public static class MonoCecilMemberExtensions
    {
        [NotNull]
        internal static string BuildMethodMemberName(this MethodReference methodReference)
        {
            var builder = new StringBuilder();

            builder.Append(methodReference.Name);
            builder.Append("(");
            if (methodReference.HasParameters)
            {
                var parameters = methodReference.Parameters;
                for (var index = 0; index < parameters.Count; ++index)
                {
                    var parameterDefinition = parameters[index];
                    if (index > 0)
                    {
                        builder.Append(",");
                    }

                    if (parameterDefinition.ParameterType.IsSentinel)
                    {
                        builder.Append("...,");
                    }

                    builder.Append(parameterDefinition.ParameterType.FullName);
                }
            }

            builder.Append(")");
            return builder.ToString();
        }

        [NotNull]
        internal static MethodMemberInstance GetMethodMemberInstanceWithMethodReference(this TypeFactory typeFactory,
            [NotNull] TypeInstance<IType> type, [NotNull] MethodReference methodReference)
        {
            var matchingMethods = type.Type.GetMethodMembers()
                .Where(member => member.FullName == methodReference.GetElementMethod().FullName).ToList();

            if (!matchingMethods.Any())
            {
                var stubMethod = typeFactory.CreateStubMethodMemberInstanceFromMethodReference(type, methodReference);
                return stubMethod;
            }

            if (matchingMethods.Count > 1)
            {
                throw new MultipleOccurrencesInSequenceException(
                    $"Multiple Methods matching {methodReference.FullName} found in provided type.");
            }

            var methodGenericArguments = new List<GenericArgument>();

            if (methodReference.IsGenericInstance)
            {
                var methodInstance = (GenericInstanceMethod) methodReference;
                methodGenericArguments.AddRange(
                    methodInstance.GenericArguments.Select(typeFactory.CreateGenericArgumentFromTypeReference));
            }

            return new MethodMemberInstance(matchingMethods.First(), type.GenericArguments, methodGenericArguments);
        }

        [NotNull]
        internal static MethodMemberInstance CreateStubMethodMemberInstanceFromMethodReference(
            this TypeFactory typeFactory, [NotNull] TypeInstance<IType> typeInstance,
            [NotNull] MethodReference methodReference)
        {
            if (methodReference.IsGenericInstance)
            {
                var elementMethod = typeFactory
                    .CreateStubMethodMemberInstanceFromMethodReference(typeInstance, methodReference.GetElementMethod())
                    .Member;

                var genericInstanceMethod = (GenericInstanceMethod) methodReference;
                var genericArguments = genericInstanceMethod.GenericArguments
                    .Select(typeFactory.CreateGenericArgumentFromTypeReference);

                return new MethodMemberInstance(elementMethod, typeInstance.GenericArguments, genericArguments);
            }

            var name = methodReference.BuildMethodMemberName();
            var typeReference = methodReference.ReturnType;
            var returnType = typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            var parameters = methodReference.GetParameters(typeFactory);
            var isGeneric = methodReference.HasGenericParameters;
            var methodForm = methodReference.HasConstructorName() ? MethodForm.Constructor : MethodForm.Normal;

            var methodMember = new MethodMember(name, methodReference.FullName, typeInstance.Type, Public, parameters,
                returnType, false, methodForm, isGeneric);

            var genericParameters = typeFactory.GetGenericParameters(methodReference);
            methodMember.GenericParameters.AddRange(genericParameters);

            return new MethodMemberInstance(methodMember, typeInstance.GenericArguments,
                Enumerable.Empty<GenericArgument>());
        }

        [NotNull]
        internal static FieldMember CreateStubFieldMemberFromFieldReference(this TypeFactory typeFactory,
            [NotNull] IType type, [NotNull] FieldReference fieldReference)
        {
            var typeReference = fieldReference.FieldType;
            var fieldType = typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);

            return new FieldMember(type, fieldReference.Name, fieldReference.FullName, Public, fieldType);
        }

        [NotNull]
        internal static IEnumerable<CustomAttribute> GetAllMethodCustomAttributes(
            this MethodDefinition methodDefinition)
        {
            return methodDefinition.CustomAttributes
                .Concat(methodDefinition.Parameters.SelectMany(parameterDefinition =>
                    parameterDefinition.CustomAttributes))
                .Concat(methodDefinition.MethodReturnType.CustomAttributes);
        }

        [NotNull]
        internal static IEnumerable<TypeInstance<IType>> GetSignatureTypes(this MethodReference methodReference,
            TypeFactory typeFactory)
        {
            var parameters = GetAllParameters(methodReference, typeFactory).ToList();
            var returnType = GetReturnType(methodReference, typeFactory);
            if (returnType != null)
            {
                parameters.Insert(0, returnType);
            }

            return parameters;
        }

        private static TypeInstance<IType> GetReturnType(this MethodReference methodReference, TypeFactory typeFactory)
        {
            return ReturnsVoid(methodReference)
                ? null
                : typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(methodReference.MethodReturnType.ReturnType);
        }

        [NotNull]
        private static IEnumerable<TypeInstance<IType>> GetAllParameters(this MethodReference methodReference,
            TypeFactory typeFactory)
        {
            var parameters = methodReference.GetParameters(typeFactory).ToList();
            var genericParameters = methodReference.GetGenericParameters(typeFactory).ToList();
            parameters.AddRange(genericParameters);
            return parameters;
        }

        [NotNull]
        internal static IEnumerable<TypeInstance<IType>> GetParameters(this MethodReference method,
            TypeFactory typeFactory)
        {
            return method.Parameters.Select(parameter =>
            {
                var typeReference = parameter.ParameterType;
                return typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            }).Distinct();
        }

        [NotNull]
        private static IEnumerable<TypeInstance<IType>> GetGenericParameters(this MethodReference method,
            TypeFactory typeFactory)
        {
            return method.GenericParameters.Select(parameter =>
            {
                var typeReference = parameter.GetElementType();
                return typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            }).Distinct();
        }

        [NotNull]
        internal static IEnumerable<TypeInstance<IType>> GetBodyTypes(this MethodDefinition methodDefinition,
            TypeFactory typeFactory)
        {
            var instructions = methodDefinition.Body?.Instructions.ToList() ?? new List<Instruction>();

            var bodyTypes = instructions.Where(inst => inst.OpCode == OpCodes.Box && inst.Operand is TypeReference)
                .Select(inst => typeFactory.GetOrCreateStubTypeInstanceFromTypeReference((TypeReference) inst.Operand));

            if (instructions.Any(inst => inst.OpCode == OpCodes.Ldstr))
            {
                //TODO find typeReference to string and add to body types (could be done like CreateStubType() in BuildMocksExtensions)
                //bodyTypes.Add(typeFactory.GetOrCreateStubTypeFromTypeReference();
            }

            bodyTypes = bodyTypes.Union(methodDefinition.Body?.Variables.Select(variableDefinition =>
            {
                var variableTypeReference = variableDefinition.VariableType;
                return typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(variableTypeReference);
            }) ?? Enumerable.Empty<TypeInstance<IType>>()).Distinct();

            return bodyTypes;
        }

        [NotNull]
        internal static IEnumerable<TypeInstance<IType>> GetReferencedTypes(this MethodDefinition methodDefinition,
            TypeFactory typeFactory)
        {
            var instructions = methodDefinition.Body?.Instructions.ToList() ?? new List<Instruction>();

            var codes = new List<OpCode>
                {OpCodes.Castclass, OpCodes.Isinst, OpCodes.Ldtoken}; //TODO probably not all necessary OpCodes

            return instructions.Where(inst =>
                    codes.Contains(inst.OpCode) && inst.Operand is TypeReference)
                .Select(inst => typeFactory.GetOrCreateStubTypeInstanceFromTypeReference((TypeReference) inst.Operand));
        }

        [NotNull]
        internal static IEnumerable<FieldMember> GetAccessedFieldMembers(this MethodDefinition methodDefinition,
            TypeFactory typeFactory)
        {
            var accessedFieldMembers = new List<FieldMember>();
            var instructions = methodDefinition.Body?.Instructions.ToList() ?? new List<Instruction>();
            var accessedFieldReferences =
                instructions.Select(inst => inst.Operand).OfType<FieldReference>().Distinct();

            foreach (var fieldReference in accessedFieldReferences)
            {
                var declaringType =
                    typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(fieldReference.DeclaringType);
                var matchingFieldMembers = declaringType.Type.GetFieldMembers()
                    .Where(member => member.Name == fieldReference.Name).ToList();

                switch (matchingFieldMembers.Count)
                {
                    case 0:
                        var stubFieldMember =
                            typeFactory.CreateStubFieldMemberFromFieldReference(declaringType.Type, fieldReference);
                        accessedFieldMembers.Add(stubFieldMember);
                        break;
                    case 1:
                        accessedFieldMembers.Add(matchingFieldMembers.First());
                        break;
                    default:
                        throw new MultipleOccurrencesInSequenceException(
                            $"Multiple Fields matching {fieldReference.FullName} found in provided type.");
                }
            }

            return accessedFieldMembers.Distinct();
        }

        public static bool IsCompilerGenerated(this MemberReference memberReference)
        {
            return memberReference.Name.Contains("<") || (memberReference.DeclaringType?.Name.Contains("<") ?? false);
        }

        public static MethodForm GetMethodForm(this MethodDefinition methodDefinition)
        {
            if (methodDefinition.IsConstructor)
            {
                return MethodForm.Constructor;
            }

            if (methodDefinition.IsGetter)
            {
                return MethodForm.Getter;
            }

            return methodDefinition.IsSetter ? MethodForm.Setter : MethodForm.Normal;
        }

        private static bool ReturnsVoid(this IMethodSignature methodSignature)
        {
            return methodSignature.MethodReturnType.ReturnType.FullName.Equals("System.Void");
        }

        public static bool HasConstructorName(this MethodReference methodReference)
        {
            return methodReference.Name == ".ctor" || methodReference.Name == ".cctor";
        }

        public static bool IsBackingField(this FieldReference fieldReference)
        {
            return fieldReference.FullName.Contains(StaticConstants.BackingField);
        }

        public static Visibility GetVisibility(this MethodDefinition methodDefinition)
        {
            if (methodDefinition.IsPublic)
            {
                return Public;
            }

            if (methodDefinition.IsPrivate)
            {
                return Private;
            }

            if (methodDefinition.IsFamily)
            {
                return Protected;
            }

            if (methodDefinition.IsAssembly)
            {
                return Internal;
            }

            if (methodDefinition.IsFamilyOrAssembly)
            {
                return ProtectedInternal;
            }

            if (methodDefinition.IsFamilyAndAssembly)
            {
                return PrivateProtected;
            }

            throw new ArgumentException("The method definition seems to have no visibility.");
        }

        public static string GetFullName(this MethodReference methodReference)
        {
            return methodReference.FullName + methodReference.GenericParameters.Aggregate(string.Empty,
                (current, newElement) => current + "<" + newElement.Name + ">");
        }
    }
}