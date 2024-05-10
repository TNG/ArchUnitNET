//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

using System;
using System.Linq;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using static ArchUnitNET.Domain.Visibility;
using GenericParameter = Mono.Cecil.GenericParameter;

namespace ArchUnitNET.Loader
{
    internal static class MonoCecilTypeExtensions
    {
        private const string RecordCloneMethodName = "<Clone>$";

        internal static string BuildFullName(this TypeReference typeReference)
        {
            if (typeReference.IsGenericParameter)
            {
                var genericParameter = (GenericParameter)typeReference;

                return (
                        genericParameter.Type == GenericParameterType.Type
                            ? genericParameter.DeclaringType.BuildFullName()
                            : genericParameter.DeclaringMethod.BuildFullName()
                    )
                    + "+<"
                    + genericParameter.Name
                    + ">";
            }

            return typeReference.FullName.Replace("/", "+");
        }

        internal static GenericParameterVariance GetVariance(this GenericParameter genericParameter)
        {
            if (genericParameter.IsCovariant)
            {
                return GenericParameterVariance.Covariant;
            }

            if (genericParameter.IsContravariant)
            {
                return GenericParameterVariance.Contravariant;
            }

            return GenericParameterVariance.NonVariant;
        }

        internal static bool IsAttribute([CanBeNull] this TypeDefinition typeDefinition)
        {
            if (typeDefinition?.BaseType != null)
            {
                return typeDefinition.BaseType.FullName == "System.Attribute"
                    || IsAttribute(typeDefinition.BaseType.Resolve());
            }

            return false;
        }

        internal static Visibility GetVisibility([CanBeNull] this TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return NotAccessible;
            }

            if (typeDefinition.IsPublic || typeDefinition.IsNestedPublic)
            {
                return Public;
            }

            if (typeDefinition.IsNestedPrivate)
            {
                return Private;
            }

            if (typeDefinition.IsNestedFamily)
            {
                return Protected;
            }

            if (typeDefinition.IsNestedFamilyOrAssembly)
            {
                return ProtectedInternal;
            }

            if (typeDefinition.IsNestedFamilyAndAssembly)
            {
                return PrivateProtected;
            }

            if (typeDefinition.IsNestedAssembly || typeDefinition.IsNotPublic)
            {
                return Internal;
            }

            throw new ArgumentException(
                "The provided type definition seems to have no visibility."
            );
        }

        internal static bool IsRecord(this TypeDefinition typeDefinition)
        {
            return typeDefinition.IsClass
                && typeDefinition.GetMethods().Any(x => x.Name == RecordCloneMethodName);
        }
    }
}
