/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using Assembly = ArchUnitNET.Domain.Assembly;

namespace ArchUnitNETTests.Fluent
{
    public static class BuildMocksExtensions
    {
        private static Type CreateStubType(this System.Type type)
        {
            var assembly = type.Assembly.CreateStubAssembly();
            var namespc = type.Namespace.CreateStubNamespace();
            return new Type(type.FullName, type.Name, assembly, namespc);
        }

        public static Class CreateStubClass(this System.Type type)
        {
            var classType = type.CreateStubType();
            var isAbstract = type.IsAbstract;
            return new Class(classType, isAbstract);
        }

        public static Type CreateShallowStubType(this Class clazz)
        {
            return new Type(clazz.FullName, clazz.Name, clazz.Assembly, clazz.Namespace);
        }
        
        private static Assembly CreateStubAssembly(this System.Reflection.Assembly assembly)
        {
            return new Assembly(assembly.FullName, assembly.FullName);
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

            var declaringType = methodBase.DeclaringType.CreateStubClass();
            var parameters = methodBase.CreateStubParameters();
            var methodForm = methodBase.GetStubMethodForm();

            Class returnType = null;
            string fullName = null;

            if (methodBase is ConstructorInfo constructor)
            {
                returnType = typeof(void).CreateStubClass();
                fullName = constructor.CreateStubFullName(returnType);
            }
            if (methodBase is MethodInfo methodInfo)
            {
                returnType = methodInfo.ReturnType.CreateStubClass();
                fullName = methodInfo.CreateStubFullName();
            }

            return new MethodMember(methodBase.BuildMockMethodName(), fullName, declaringType, visibility, parameters,
                returnType, methodBase.IsVirtual, methodForm);
        }

        private static string BuildMockMethodName(this MethodBase methodBase)
        {
            StringBuilder stringBuilder = new StringBuilder(methodBase.Name);
            stringBuilder.Append("(");
            stringBuilder.Append(ConstructParameters(methodBase.CreateStubParameters(), methodBase.CallingConvention));
            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }

        //todo: update method below for new MethodMember naming convention
        private static string ConstructParameters(IEnumerable<IType> parameterTypes, CallingConventions callingConvention)
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
            return methodInfo.GetParameters().Select(info => (IType) info.ParameterType.CreateStubClass()).ToList();
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
            return methodInfo.IsPublic ? Visibility.Public : Visibility.Private;
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

        public static MethodCallDependency CreateStubMethodCallDependency(IMember originMember,
            MethodMember targetMember)
        {
            var methodCallDependency = new MethodCallDependency(originMember, targetMember);
            methodCallDependency.TargetMember.MemberBackwardsDependencies.Add(methodCallDependency);
            return methodCallDependency;
        }
    }
}