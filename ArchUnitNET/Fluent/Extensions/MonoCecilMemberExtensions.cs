//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;

namespace ArchUnitNET.Fluent.Extensions
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

        internal static MethodMember CreateStubMethodMemberFromMethodReference(this TypeFactory typeFactory, IType type,
            MethodReference methodReference)
        {
            if (type == null || methodReference == null)
            {
                return null;
            }

            var typeReference = methodReference.ReturnType;
            var returnType = typeFactory.GetOrCreateStubTypeFromTypeReference(typeReference);
            var parameters = methodReference.GetParameters(typeFactory).ToList();

            var methodForm = methodReference.HasConstructorName() ? MethodForm.Constructor : MethodForm.Normal;

            return new MethodMember(methodReference.BuildMethodMemberName(), methodReference.FullName, type,
                Public, parameters, returnType, false, methodForm);
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
        internal static IEnumerable<IType> GetSignatureTypes(this MethodReference methodReference,
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

        private static IType GetReturnType(this MethodReference methodReference, TypeFactory typeFactory)
        {
            return ReturnsVoid(methodReference)
                ? null
                : typeFactory.GetOrCreateStubTypeFromTypeReference(methodReference.MethodReturnType.ReturnType);
        }

        [NotNull]
        private static IEnumerable<IType> GetAllParameters(this MethodReference methodReference,
            TypeFactory typeFactory)
        {
            var parameters = methodReference.GetParameters(typeFactory).ToList();
            var genericParameters = methodReference.GetGenericParameters(typeFactory).ToList();
            parameters.AddRange(genericParameters);
            return parameters;
        }

        [NotNull]
        internal static IEnumerable<IType> GetParameters(this MethodReference method, TypeFactory typeFactory)
        {
            return method.Parameters.Select(parameter =>
            {
                var typeReference = parameter.ParameterType;
                return typeFactory.GetOrCreateStubTypeFromTypeReference(typeReference);
            }).Distinct();
        }

        [NotNull]
        private static IEnumerable<IType> GetGenericParameters(this MethodReference method, TypeFactory typeFactory)
        {
            return method.GenericParameters.Select(parameter =>
            {
                var typeReference = parameter.GetElementType();
                return typeFactory.GetOrCreateStubTypeFromTypeReference(typeReference);
            }).Distinct();
        }

        [NotNull]
        internal static IEnumerable<IType> GetBodyTypes(this MethodDefinition methodDefinition, TypeFactory typeFactory)
        {
            return methodDefinition.Body?.Variables.Select(variableDefinition =>
            {
                var variableTypeReference = variableDefinition.VariableType;
                return typeFactory.GetOrCreateStubTypeFromTypeReference(variableTypeReference);
            }).Distinct() ?? Enumerable.Empty<IType>();
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

        public static string GetFullName(this MethodDefinition methodDefinition)
        {
            return methodDefinition.FullName + methodDefinition.GenericParameters.Aggregate(string.Empty,
                       (current, newElement) => current + "<" + newElement.Name + ">");
        }
    }
}