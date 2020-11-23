//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;

namespace ArchUnitNET.Loader
{
    public static class CompilerGeneratedExtensions
    {
        public static bool IsCompilerGenerated(this MethodMemberInstance instance)
        {
            return IsCompilerGenerated(instance.Member) ||
                   instance.DeclaringTypeGenericArguments.Any(IsCompilerGenerated) ||
                   instance.MemberGenericArguments.Any(IsCompilerGenerated);
        }

        public static bool IsCompilerGenerated(this IMember member)
        {
            return IsCompilerGenerated(member.DeclaringType) || member.NameContains("<") || member.NameStartsWith("!");
        }

        public static bool IsCompilerGenerated<T>(this TypeInstance<T> type) where T : IType
        {
            return IsCompilerGenerated(type.Type) || type.GenericArguments.Any(IsCompilerGenerated);
        }

        public static bool IsCompilerGenerated(this IType type)
        {
            return type.NameContains("<") || type.NameStartsWith("!");
        }

        public static bool IsCompilerGenerated(this GenericArgument genericArgument)
        {
            return IsCompilerGenerated(genericArgument.Type) ||
                   genericArgument.GenericArguments.Any(IsCompilerGenerated);
        }
    }
}