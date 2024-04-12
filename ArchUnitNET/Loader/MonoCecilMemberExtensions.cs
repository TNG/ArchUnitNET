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
    internal static class MonoCecilMemberExtensions
    {
        private static readonly OpCode[] BodyTypeOpCodes =
        {
            OpCodes.Box,
            OpCodes.Newarr,
            OpCodes.Initobj,
            OpCodes.Unbox,
            OpCodes.Unbox_Any,
            OpCodes.Ldelem_Any,
            OpCodes.Ldobj,
            OpCodes.Stelem_Any,
            OpCodes.Ldelema,
            OpCodes.Stobj
        }; //maybe not complete

        internal static string BuildFullName(this MethodReference methodReference)
        {
            return methodReference.FullName
                + methodReference.GenericParameters.Aggregate(
                    string.Empty,
                    (current, newElement) => current + "<" + newElement.Name + ">"
                );
        }

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
        internal static IEnumerable<CustomAttribute> GetAllMethodCustomAttributes(
            this MethodDefinition methodDefinition
        )
        {
            return methodDefinition
                .CustomAttributes.Concat(
                    methodDefinition.Parameters.SelectMany(parameterDefinition =>
                        parameterDefinition.CustomAttributes
                    )
                )
                .Concat(methodDefinition.MethodReturnType.CustomAttributes);
        }

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetSignatureTypes(
            this MethodReference methodReference,
            TypeFactory typeFactory
        )
        {
            var parameters = GetAllParameters(methodReference, typeFactory).ToList();
            var returnType = GetReturnType(methodReference, typeFactory);
            if (returnType != null)
            {
                parameters.Insert(0, returnType);
            }

            return parameters;
        }

        private static ITypeInstance<IType> GetReturnType(
            this MethodReference methodReference,
            TypeFactory typeFactory
        )
        {
            return ReturnsVoid(methodReference)
                ? null
                : typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                    methodReference.MethodReturnType.ReturnType
                );
        }

        [NotNull]
        private static IEnumerable<ITypeInstance<IType>> GetAllParameters(
            this MethodReference methodReference,
            TypeFactory typeFactory
        )
        {
            var parameters = methodReference.GetParameters(typeFactory).ToList();
            var genericParameters = methodReference.GetGenericParameters(typeFactory).ToList();
            parameters.AddRange(genericParameters);
            return parameters;
        }

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetParameters(
            this MethodReference method,
            TypeFactory typeFactory
        )
        {
            return method
                .Parameters.Select(parameter =>
                {
                    var typeReference = parameter.ParameterType;
                    return typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
                })
                .Distinct();
        }

        [NotNull]
        private static IEnumerable<ITypeInstance<IType>> GetGenericParameters(
            this MethodReference method,
            TypeFactory typeFactory
        )
        {
            return method
                .GenericParameters.Select(parameter =>
                {
                    var typeReference = parameter.GetElementType();
                    return typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
                })
                .Distinct();
        }

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetBodyTypes(
            this MethodDefinition methodDefinition,
            TypeFactory typeFactory
        )
        {
            var instructions =
                methodDefinition.Body?.Instructions ?? Enumerable.Empty<Instruction>();

            var bodyTypes = instructions
                .Where(inst =>
                    BodyTypeOpCodes.Contains(inst.OpCode) && inst.Operand is TypeReference
                )
                .Select(inst =>
                    typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        (TypeReference)inst.Operand
                    )
                );

            //OpCodes.Ldstr should create a dependency to string, but it does not have a TypeReference as Operand so no Type can be created

            bodyTypes = bodyTypes
                .Union(
                    methodDefinition.Body?.Variables.Select(variableDefinition =>
                    {
                        var variableTypeReference = variableDefinition.VariableType;
                        return typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                            variableTypeReference
                        );
                    }) ?? Enumerable.Empty<TypeInstance<IType>>()
                )
                .Distinct();

            return bodyTypes;
        }

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetCastTypes(
            this MethodDefinition methodDefinition,
            TypeFactory typeFactory
        )
        {
            var instructions =
                methodDefinition.Body?.Instructions ?? Enumerable.Empty<Instruction>();

            return instructions
                .Where(inst => inst.OpCode == OpCodes.Castclass && inst.Operand is TypeReference)
                .Select(inst =>
                    typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        (TypeReference)inst.Operand
                    )
                );
        }

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetMetaDataTypes(
            this MethodDefinition methodDefinition,
            TypeFactory typeFactory
        )
        {
            var instructions =
                methodDefinition.Body?.Instructions ?? Enumerable.Empty<Instruction>();

            return instructions
                .Where(inst => inst.OpCode == OpCodes.Ldtoken && inst.Operand is TypeReference)
                .Select(inst =>
                    typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        (TypeReference)inst.Operand
                    )
                );
        }

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetTypeCheckTypes(
            this MethodDefinition methodDefinition,
            TypeFactory typeFactory
        )
        {
            var instructions =
                methodDefinition.Body?.Instructions ?? Enumerable.Empty<Instruction>();

            return instructions
                .Where(inst => inst.OpCode == OpCodes.Isinst && inst.Operand is TypeReference)
                .Select(inst =>
                    typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                        (TypeReference)inst.Operand
                    )
                );
        }

        internal static bool IsIterator(this MethodDefinition methodDefinition)
        {
            return methodDefinition.CustomAttributes.Any(att =>
                att.AttributeType.FullName
                == typeof(System.Runtime.CompilerServices.IteratorStateMachineAttribute).FullName
            );
        }

        internal static bool IsAsync(this MethodDefinition methodDefinition)
        {
            return methodDefinition.CustomAttributes.Any(att =>
                att.AttributeType.FullName
                == typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute).FullName
            );
        }

        [NotNull]
        internal static IEnumerable<FieldMember> GetAccessedFieldMembers(
            this MethodDefinition methodDefinition,
            TypeFactory typeFactory
        )
        {
            var accessedFieldMembers = new List<FieldMember>();
            var instructions =
                methodDefinition.Body?.Instructions.ToList() ?? new List<Instruction>();
            var accessedFieldReferences = instructions
                .Select(inst => inst.Operand)
                .OfType<FieldReference>()
                .Distinct();

            foreach (var fieldReference in accessedFieldReferences)
            {
                var declaringType = typeFactory.GetOrCreateStubTypeInstanceFromTypeReference(
                    fieldReference.DeclaringType
                );
                var matchingFieldMembers = declaringType
                    .Type.GetFieldMembers()
                    .Where(member => member.Name == fieldReference.Name)
                    .ToList();

                switch (matchingFieldMembers.Count)
                {
                    case 0:
                        var stubFieldMember = typeFactory.CreateStubFieldMemberFromFieldReference(
                            declaringType.Type,
                            fieldReference
                        );
                        accessedFieldMembers.Add(stubFieldMember);
                        break;
                    case 1:
                        accessedFieldMembers.Add(matchingFieldMembers.First());
                        break;
                    default:
                        throw new MultipleOccurrencesInSequenceException(
                            $"Multiple Fields matching {fieldReference.FullName} found in provided type."
                        );
                }
            }

            return accessedFieldMembers.Distinct();
        }

        internal static bool IsCompilerGenerated(this MemberReference memberReference)
        {
            if (memberReference.Name.HasCompilerGeneratedName())
            {
                return true;
            }
            var declaringType =
                memberReference.Resolve()?.DeclaringType ?? memberReference.DeclaringType;
            return declaringType != null && declaringType.Name.HasCompilerGeneratedName();
        }

        internal static bool HasCompilerGeneratedName(this string name)
        {
            return name[0] == '<' || name[0] == '!';
        }

        internal static MethodForm GetMethodForm(this MethodDefinition methodDefinition)
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

        internal static bool HasConstructorName(this MethodReference methodReference)
        {
            return methodReference.Name == ".ctor" || methodReference.Name == ".cctor";
        }

        internal static bool IsBackingField(this FieldReference fieldReference)
        {
            return fieldReference.FullName.Contains(StaticConstants.BackingField);
        }

        internal static Visibility GetVisibility([CanBeNull] this MethodDefinition methodDefinition)
        {
            if (methodDefinition == null)
            {
                return NotAccessible;
            }

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
    }
}
