//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ArchUnitNET.Domain;
using static ArchUnitNET.Domain.Visibility;
using Assembly = ArchUnitNET.Domain.Assembly;
using Type = ArchUnitNET.Loader.Type;

namespace ArchUnitNETTests.Fluent.Extensions
{
    public static class BuildMocksExtensions
    {
        private static Type CreateStubType(this System.Type type)
        {
            var assembly = type.Assembly.CreateStubAssembly();
            var namespc = type.Namespace.CreateStubNamespace();
            var visibility = type.GetVisibility();
            return new Type(type.FullName, type.Name, assembly, namespc, visibility, type.IsNested, type.IsGenericType,
                true, false);
        }

        private static Visibility GetVisibility(this System.Type type)
        {
            if (type == null)
            {
                return NotAccessible;
            }

            if (type.IsPublic || type.IsNestedPublic)
            {
                return Public;
            }

            if (type.IsNestedPrivate)
            {
                return Private;
            }

            if (type.IsNestedFamily)
            {
                return Protected;
            }

            if (type.IsNestedFamORAssem)
            {
                return ProtectedInternal;
            }

            if (type.IsNestedFamANDAssem)
            {
                return PrivateProtected;
            }

            if (type.IsNestedAssembly || type.IsNotPublic)
            {
                return Internal;
            }

            throw new ArgumentException("The provided type seems to have no visibility.");
        }

        public static IType CreateStubIType(this System.Type type)
        {
            var classType = type.CreateStubType();
            if (type.IsEnum)
            {
                return new ArchUnitNET.Domain.Enum(classType);
            }

            if (type.IsValueType)
            {
                return new Struct(classType);
            }

            return new Class(classType, type.IsAbstract, type.IsSealed);
        }

        public static Type CreateShallowStubType(this Class clazz)
        {
            var type = new Type(clazz.FullName, clazz.Name, clazz.Assembly, clazz.Namespace, clazz.Visibility,
                clazz.IsNested, clazz.IsGeneric, clazz.IsStub, clazz.IsCompilerGenerated);
            type.GenericParameters.AddRange(clazz.GenericParameters);
            return type;
        }

        private static Assembly CreateStubAssembly(this System.Reflection.Assembly assembly)
        {
            return new Assembly(assembly.FullName, assembly.FullName, false);
        }

        private static Namespace CreateStubNamespace(this string namespc)
        {
            return new Namespace(namespc, new List<IType>());
        }

        public static string BuildMethodMemberName(this string methodName, params System.Type[] parameterType)
        {
            var builder = new StringBuilder();

            builder.Append(methodName);
            builder.Append("(");

            for (var index = 0; index < parameterType.Length; index++)
            {
                if (index > 0)
                {
                    builder.Append(",");
                }

                builder.Append(parameterType[index].FullName);
            }

            builder.Append(")");
            return builder.ToString();
        }

        public static MethodMember CreateStubMethodMember(this MethodBase methodBase)
        {
            var visibility = methodBase.GetVisibility();

            var declaringType = methodBase.DeclaringType.CreateStubIType();
            var parameters = methodBase.CreateStubParameters().Select(parameter => new TypeInstance<IType>(parameter));
            var methodForm = methodBase.GetStubMethodForm();
            var isStatic = methodBase.IsStatic;

            var isGeneric = methodBase.IsGenericMethod;

            TypeInstance<IType> returnTypeInstance = null;
            string fullName = null;

            if (methodBase is ConstructorInfo constructor)
            {
                var voi = typeof(void).CreateStubIType();
                returnTypeInstance = new TypeInstance<IType>(voi);
                fullName = constructor.CreateStubFullName(voi);
            }

            if (methodBase is MethodInfo methodInfo)
            {
                var returnType = methodInfo.ReturnType.CreateStubIType();
                returnTypeInstance = new TypeInstance<IType>(returnType);
                fullName = methodInfo.CreateStubFullName();
            }

            var methodMember = new MethodMember(methodBase.BuildMockMethodName(), fullName, declaringType, visibility,
                returnTypeInstance, methodBase.IsVirtual, methodForm, isGeneric, false, false, false, isStatic);

            methodMember.ParameterInstances.AddRange(parameters);
            return methodMember;
        }

        private static string BuildMockMethodName(this MethodBase methodBase)
        {
            var stringBuilder = new StringBuilder(methodBase.Name);
            stringBuilder.Append("(");
            stringBuilder.Append(ConstructParameters(methodBase.CreateStubParameters(), methodBase.CallingConvention));
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        private static string ConstructParameters(IEnumerable<IType> parameterTypes,
            CallingConventions callingConvention)
        {
            var stringBuilder = new StringBuilder();
            var str1 = "";
            foreach (var parameterType in parameterTypes)
            {
                stringBuilder.Append(str1);
                var str2 = parameterType.Name;
                stringBuilder.Append(str2);
                str1 = ", ";
            }

            if ((callingConvention & CallingConventions.VarArgs) != CallingConventions.VarArgs)
            {
                return stringBuilder.ToString();
            }

            stringBuilder.Append(str1);
            stringBuilder.Append("...");
            return stringBuilder.ToString();
        }

        private static List<IType> CreateStubParameters(this MethodBase methodInfo)
        {
            return methodInfo.GetParameters().Select(info => (IType)CreateStubIType(info.ParameterType)).ToList();
        }

        private static string CreateStubFullName(this MethodInfo methodInfo)
        {
            var builder = new StringBuilder();
            builder.Append(methodInfo.ReturnType.FullName).Append(" ").Append(methodInfo.MemberFullName());
            methodInfo.MethodSignatureFullName(builder);
            return builder.ToString();
        }

        private static string CreateStubFullName(this ConstructorInfo constructorInfo, IType returnType)
        {
            var builder = new StringBuilder();
            builder.Append(returnType.FullName).Append(" ").Append(constructorInfo.MemberFullName());
            constructorInfo.MethodSignatureFullName(builder);
            return builder.ToString();
        }

        private static string MemberFullName<TMethod>(this TMethod methodInfo) where TMethod : MethodBase
        {
            if (methodInfo.DeclaringType == null)
            {
                return methodInfo.Name;
            }

            return methodInfo.DeclaringType.FullName + "::" + methodInfo.Name;
        }

        private static void MethodSignatureFullName(this MethodBase methodBase, StringBuilder stringBuilder)
        {
            stringBuilder.Append("(");
            if (methodBase.GetParameters().Length != 0)
            {
                var parameters = methodBase.GetParameters();
                for (var index = 0; index < parameters.Length; ++index)
                {
                    var parameterDefinition = parameters[index];
                    if (index > 0)
                    {
                        stringBuilder.Append(",");
                    }

                    stringBuilder.Append(parameterDefinition.ParameterType.FullName);
                }
            }

            stringBuilder.Append(")");
        }

        private static Visibility GetVisibility<TMethod>(this TMethod methodInfo) where TMethod : MethodBase
        {
            return methodInfo.IsPublic ? Public : Private;
        }

        private static MethodForm GetStubMethodForm<TMemberInfo>(this TMemberInfo methodInfo)
            where TMemberInfo : MemberInfo
        {
            var methodForm = MethodForm.Normal;
            if (methodInfo.MemberType == MemberTypes.Constructor)
            {
                methodForm = MethodForm.Constructor;
            }

            return methodForm;
        }
    }
}