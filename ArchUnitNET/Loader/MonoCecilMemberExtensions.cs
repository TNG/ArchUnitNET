using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchUnitNET.Domain;
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
            OpCodes.Stobj,
        }; //maybe not complete

        internal static string BuildFullName(this MethodReference methodReference)
        {
            if (!methodReference.HasGenericParameters)
            {
                return methodReference.FullName;
            }

            var sb = new StringBuilder(methodReference.FullName);
            foreach (var p in methodReference.GenericParameters)
            {
                sb.Append('<').Append(p.Name).Append('>');
            }

            return sb.ToString();
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

        internal static ITypeInstance<IType> GetReturnType(
            this MethodReference methodReference,
            DomainResolver domainResolver
        ) =>
            ReturnsVoid(methodReference)
                ? null
                : domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                    methodReference.MethodReturnType.ReturnType
                );

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetParameters(
            this MethodReference method,
            DomainResolver domainResolver
        ) =>
            method.Parameters.Select(parameter =>
                domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(parameter.ParameterType)
            );

        [NotNull]
        internal static IEnumerable<ITypeInstance<IType>> GetGenericParameters(
            this MethodReference method,
            DomainResolver domainResolver
        ) =>
            method.GenericParameters.Select(
                domainResolver.GetOrCreateStubTypeInstanceFromTypeReference
            );

        /// <summary>
        /// Result of a single-pass scan of a method body.
        /// </summary>
        internal struct MethodBodyScanResult
        {
            internal readonly List<ITypeInstance<IType>> BodyTypes;
            internal readonly List<ITypeInstance<IType>> CastTypes;
            internal readonly List<ITypeInstance<IType>> MetaDataTypes;
            internal readonly List<ITypeInstance<IType>> TypeCheckTypes;
            internal readonly List<FieldMember> AccessedFieldMembers;

            internal MethodBodyScanResult(
                List<ITypeInstance<IType>> bodyTypes,
                List<ITypeInstance<IType>> castTypes,
                List<ITypeInstance<IType>> metaDataTypes,
                List<ITypeInstance<IType>> typeCheckTypes,
                List<FieldMember> accessedFieldMembers
            )
            {
                BodyTypes = bodyTypes;
                CastTypes = castTypes;
                MetaDataTypes = metaDataTypes;
                TypeCheckTypes = typeCheckTypes;
                AccessedFieldMembers = accessedFieldMembers;
            }
        }

        /// <summary>
        /// Scans the method body in a single pass, collecting body types, cast types,
        /// metadata types, type-check types, accessed field members, and local variable types.
        /// </summary>
        [NotNull]
        internal static MethodBodyScanResult ScanMethodBody(
            this MethodDefinition methodDefinition,
            DomainResolver domainResolver
        )
        {
            var bodyTypes = new List<ITypeInstance<IType>>();
            var castTypes = new List<ITypeInstance<IType>>();
            var metaDataTypes = new List<ITypeInstance<IType>>();
            var typeCheckTypes = new List<ITypeInstance<IType>>();
            var accessedFieldMembers = new List<FieldMember>();

            var body = methodDefinition.Body;
            if (body != null)
            {
                // Collect local variable types
                var seenBodyTypes = new HashSet<ITypeInstance<IType>>();
                bodyTypes.AddRange(
                    body.Variables.Select(variableDefinition =>
                            domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                                variableDefinition.VariableType
                            )
                        )
                        .Where(typeInstance => seenBodyTypes.Add(typeInstance))
                );

                // Single pass over instructions
                var seenFieldRefs = new HashSet<FieldReference>(
                    FieldReferenceNameComparer.Instance
                );
                foreach (var instruction in body.Instructions)
                {
                    var opCode = instruction.OpCode;
                    var operand = instruction.Operand;

                    switch (operand)
                    {
                        case TypeReference typeReference when opCode == OpCodes.Castclass:
                            castTypes.Add(
                                domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                                    typeReference
                                )
                            );
                            break;
                        case TypeReference typeReference when opCode == OpCodes.Ldtoken:
                            metaDataTypes.Add(
                                domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                                    typeReference
                                )
                            );
                            break;
                        case TypeReference typeReference when opCode == OpCodes.Isinst:
                            typeCheckTypes.Add(
                                domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                                    typeReference
                                )
                            );
                            break;
                        case TypeReference typeReference:
                        {
                            if (BodyTypeOpCodes.Contains(opCode))
                            {
                                var bodyTypeInstance =
                                    domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                                        typeReference
                                    );
                                if (seenBodyTypes.Add(bodyTypeInstance))
                                {
                                    bodyTypes.Add(bodyTypeInstance);
                                }
                            }

                            break;
                        }
                        case FieldReference fieldReference when !seenFieldRefs.Add(fieldReference):
                            continue;
                        case FieldReference fieldReference:
                        {
                            var declaringType =
                                domainResolver.GetOrCreateStubTypeInstanceFromTypeReference(
                                    fieldReference.DeclaringType
                                );
                            accessedFieldMembers.Add(
                                domainResolver.GetOrCreateFieldMember(
                                    declaringType.Type,
                                    fieldReference
                                )
                            );
                            break;
                        }
                    }
                }
            }

            return new MethodBodyScanResult(
                bodyTypes,
                castTypes,
                metaDataTypes,
                typeCheckTypes,
                accessedFieldMembers
            );
        }

        /// <summary>
        /// Comparer that deduplicates FieldReference by full name, avoiding repeated field lookups.
        /// </summary>
        private sealed class FieldReferenceNameComparer : IEqualityComparer<FieldReference>
        {
            internal static readonly FieldReferenceNameComparer Instance =
                new FieldReferenceNameComparer();

            public bool Equals(FieldReference x, FieldReference y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;
                return x.FullName == y.FullName;
            }

            public int GetHashCode(FieldReference obj)
            {
                return obj?.FullName?.GetHashCode() ?? 0;
            }
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
