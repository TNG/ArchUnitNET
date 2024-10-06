//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader.LoadTasks;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;
using Enum = ArchUnitNET.Domain.Enum;
using GenericParameter = ArchUnitNET.Domain.GenericParameter;

namespace ArchUnitNET.Loader
{
    internal class TypeFactory
    {
        private readonly AssemblyRegistry _assemblyRegistry;
        private readonly LoadTaskRegistry _loadTaskRegistry;
        private readonly MethodMemberRegistry _methodMemberRegistry;
        private readonly NamespaceRegistry _namespaceRegistry;
        private readonly TypeRegistry _typeRegistry;
        private readonly Dictionary<string, IType> _allTypes = new Dictionary<string, IType>();

        public TypeFactory(
            TypeRegistry typeRegistry,
            MethodMemberRegistry methodMemberRegistry,
            LoadTaskRegistry loadTaskRegistry,
            AssemblyRegistry assemblyRegistry,
            NamespaceRegistry namespaceRegistry
        )
        {
            _loadTaskRegistry = loadTaskRegistry;
            _assemblyRegistry = assemblyRegistry;
            _namespaceRegistry = namespaceRegistry;
            _typeRegistry = typeRegistry;
            _methodMemberRegistry = methodMemberRegistry;
        }

        public IEnumerable<IType> GetAllNonCompilerGeneratedTypes()
        {
            return _typeRegistry.GetAllTypes().Where(type => !type.IsCompilerGenerated);
        }

        [NotNull]
        internal IType GetOrCreateTypeFromTypeReference(TypeReference typeReference)
        {
            return GetOrCreateTypeInstanceFromTypeReference(typeReference, false).Type;
        }

        [NotNull]
        internal ITypeInstance<IType> GetOrCreateStubTypeInstanceFromTypeReference(
            TypeReference typeReference
        )
        {
            return GetOrCreateTypeInstanceFromTypeReference(typeReference, true);
        }

        private ITypeInstance<IType> GetOrCreateTypeInstanceFromTypeReference(
            TypeReference typeReference,
            bool isStub
        )
        {
            return _typeRegistry.GetOrCreateTypeFromTypeReference(
                typeReference,
                s => CreateTypeFromTypeReference(typeReference, isStub)
            );
        }

        [NotNull]
        internal MethodMemberInstance GetOrCreateMethodMemberFromMethodReference(
            [NotNull] IType type,
            [NotNull] MethodReference methodReference
        )
        {
            return _methodMemberRegistry.GetOrCreateMethodFromMethodReference(
                methodReference,
                s =>
                    CreateMethodMemberFromMethodReference(
                        new TypeInstance<IType>(type),
                        methodReference
                    )
            );
        }

        [NotNull]
        internal MethodMemberInstance GetOrCreateMethodMemberFromMethodReference(
            [NotNull] ITypeInstance<IType> typeInstance,
            [NotNull] MethodReference methodReference
        )
        {
            return _methodMemberRegistry.GetOrCreateMethodFromMethodReference(
                methodReference,
                s => CreateMethodMemberFromMethodReference(typeInstance, methodReference)
            );
        }

        [NotNull]
        private ITypeInstance<IType> CreateTypeFromTypeReference(
            TypeReference typeReference,
            bool isStub
        )
        {
            if (typeReference is TypeDefinition t)
            {
                return CreateTypeInstanceFromTypeDefinition(t, isStub);
            }

            if (typeReference.IsGenericParameter)
            {
                var genericParameter = (Mono.Cecil.GenericParameter)typeReference;
                var declarerIsMethod = genericParameter.Type == GenericParameterType.Method;
                var declaringTypeFullName = declarerIsMethod
                    ? genericParameter.DeclaringMethod.BuildFullName()
                    : genericParameter.DeclaringType.BuildFullName();

                return new TypeInstance<GenericParameter>(
                    CreateGenericParameter(
                        genericParameter,
                        declaringTypeFullName,
                        declarerIsMethod
                    )
                );
            }

            if (typeReference.IsArray)
            {
                var dimensions = new List<int>();
                do
                {
                    var arrayType = (ArrayType)typeReference;
                    dimensions.Add(arrayType.Rank);
                    typeReference = arrayType.ElementType;
                } while (typeReference.IsArray);

                var elementTypeInstance = GetOrCreateStubTypeInstanceFromTypeReference(
                    typeReference
                );
                return CreateTypeInstance(
                    elementTypeInstance.Type,
                    elementTypeInstance.GenericArguments.ToList(),
                    dimensions
                );
            }

            if (typeReference.IsGenericInstance)
            {
                var elementType = GetOrCreateStubTypeInstanceFromTypeReference(
                    typeReference.GetElementType()
                ).Type;
                var genericInstance = (GenericInstanceType)typeReference;
                var genericArguments = genericInstance
                    .GenericArguments.Select(CreateGenericArgumentFromTypeReference)
                    .Where(argument => !argument.Type.IsCompilerGenerated)
                    .ToList();
                return CreateTypeInstance(elementType, genericArguments, new List<int>());
            }

            if (
                typeReference.IsByReference
                || typeReference.IsPointer
                || typeReference.IsPinned
                || typeReference.IsRequiredModifier
            )
            {
                return GetOrCreateTypeInstanceFromTypeReference(
                    typeReference.GetElementType(),
                    isStub
                );
            }

            if (typeReference is FunctionPointerType functionPointerType)
            {
                return GetOrCreateTypeInstance(functionPointerType);
            }

            TypeDefinition typeDefinition;
            try
            {
                typeDefinition = typeReference.Resolve();
            }
            catch (AssemblyResolutionException e)
            {
                throw new ArchLoaderException(
                    $"Could not resolve type {typeReference.FullName}",
                    e
                );
            }

            if (typeDefinition == null)
            {
                throw new ArchLoaderException($"Could not resolve type {typeReference.FullName}");
            }

            return _typeRegistry.GetOrCreateTypeFromTypeReference(
                typeDefinition,
                s => CreateTypeInstanceFromTypeDefinition(typeDefinition, isStub)
            );
        }

        [NotNull]
        private ITypeInstance<IType> CreateTypeInstanceFromTypeDefinition(
            TypeDefinition typeDefinition,
            bool isStub
        )
        {
            const string fixedElementField = "FixedElementField";
            if (
                typeDefinition.CustomAttributes.Any(att =>
                    att.AttributeType.FullName == typeof(UnsafeValueTypeAttribute).FullName
                ) && typeDefinition.Fields.Any(field => field.Name == fixedElementField)
            )
            {
                var arrayType = typeDefinition
                    .Fields.First(field => field.Name == fixedElementField)
                    .FieldType;
                var arrayTypeInstance = GetOrCreateStubTypeInstanceFromTypeReference(arrayType);
                var dimensions = new List<int> { 1 };
                return CreateTypeInstance(
                    arrayTypeInstance.Type,
                    arrayTypeInstance.GenericArguments.ToList(),
                    dimensions
                );
            }

            var assemblyFullName = typeDefinition.Module.Assembly.FullName;
            var fullName = typeDefinition.BuildFullName();
            // var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
            //     assemblyFullName,
            //     fullName
            // );
            // if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingType))
            // {
            //     return CreateTypeInstance(existingType, new List<GenericArgument>(), new List<int>());
            // }
            //
            var declaringTypeReference = typeDefinition;
            while (declaringTypeReference.IsNested)
            {
                declaringTypeReference = declaringTypeReference.DeclaringType;
            }
            var currentNamespace = _namespaceRegistry.GetOrCreateNamespace(
                declaringTypeReference.Namespace
            );
            var currentAssembly = _assemblyRegistry.GetOrCreateAssembly(
                assemblyFullName,
                assemblyFullName,
                true,
                null
            );
            var visibility = typeDefinition.GetVisibility();
            var isCompilerGenerated = typeDefinition.IsCompilerGenerated();
            var isNested = typeDefinition.IsNested;
            var isGeneric = typeDefinition.HasGenericParameters;
            var type = new Type(
                fullName,
                typeDefinition.Name,
                currentAssembly,
                currentNamespace,
                visibility,
                isNested,
                isGeneric,
                isStub,
                isCompilerGenerated
            );

            var genericParameters = GetGenericParameters(typeDefinition);
            type.GenericParameters.AddRange(genericParameters);

            ITypeInstance<IType> createdTypeInstance;

            if (typeDefinition.IsInterface)
            {
                createdTypeInstance = new TypeInstance<Interface>(new Interface(type));
            }
            else if (typeDefinition.IsAttribute())
            {
                createdTypeInstance = new TypeInstance<Attribute>(
                    new Attribute(type, typeDefinition.IsAbstract, typeDefinition.IsSealed)
                );
            }
            else if (typeDefinition.IsValueType)
            {
                if (typeDefinition.IsEnum)
                {
                    createdTypeInstance = new TypeInstance<Enum>(new Enum(type));
                }
                else
                {
                    createdTypeInstance = new TypeInstance<Struct>(new Struct(type));
                }
            }
            else
            {
                createdTypeInstance = new TypeInstance<Class>(
                    new Class(
                        type,
                        typeDefinition.IsAbstract,
                        typeDefinition.IsSealed,
                        typeDefinition.IsRecord()
                    )
                );
            }

            if (!isStub && !isCompilerGenerated)
            {
                if (!typeDefinition.IsInterface)
                {
                    LoadBaseTask(createdTypeInstance.Type, type, typeDefinition);
                }

                LoadNonBaseTasks(createdTypeInstance.Type, type, typeDefinition);
            }

            // _allTypes.Add(assemblyQualifiedName, createdTypeInstance.Type);
            return createdTypeInstance;
        }

        [NotNull]
        private ITypeInstance<IType> CreateTypeInstance(
            IType type,
            List<GenericArgument> genericArguments,
            List<int> arrayDimensions
        )
        {
            switch (type)
            {
                case Interface intf:
                    return new TypeInstance<Interface>(intf, genericArguments, arrayDimensions);
                case Attribute att:
                    return new TypeInstance<Attribute>(att, genericArguments, arrayDimensions);
                case Class cls:
                    return new TypeInstance<Class>(cls, genericArguments, arrayDimensions);
                case Struct str:
                    return new TypeInstance<Struct>(str, genericArguments, arrayDimensions);
                case Enum en:
                    return new TypeInstance<Enum>(en, genericArguments, arrayDimensions);
                case GenericParameter gen:
                    return new TypeInstance<GenericParameter>(
                        gen,
                        genericArguments,
                        arrayDimensions
                    );
                default:
                    throw new ArgumentException("Subtype of IType not recognized");
            }
        }

        [NotNull]
        private ITypeInstance<FunctionPointer> GetOrCreateTypeInstance(
            FunctionPointerType functionPointerType
        )
        {
            var type = new Type(
                functionPointerType.FullName,
                functionPointerType.Name,
                null,
                null,
                Public,
                false,
                false,
                false,
                false
            );
            var returnTypeInstance = GetOrCreateStubTypeInstanceFromTypeReference(
                functionPointerType.ReturnType
            );
            var parameterTypeInstances = functionPointerType
                .Parameters.Select(parameter =>
                    GetOrCreateStubTypeInstanceFromTypeReference(parameter.ParameterType)
                )
                .ToList();
            return new TypeInstance<FunctionPointer>(
                new FunctionPointer(type, returnTypeInstance, parameterTypeInstances)
            );
        }

        [NotNull]
        private MethodMemberInstance CreateMethodMemberFromMethodReference(
            [NotNull] ITypeInstance<IType> typeInstance,
            [NotNull] MethodReference methodReference
        )
        {
            if (methodReference.IsGenericInstance)
            {
                var elementMethod = CreateMethodMemberFromMethodReference(
                    typeInstance,
                    methodReference.GetElementMethod()
                ).Member;

                var genericInstanceMethod = (GenericInstanceMethod)methodReference;
                var genericArguments = genericInstanceMethod
                    .GenericArguments.Select(CreateGenericArgumentFromTypeReference)
                    .Where(argument => !argument.Type.IsCompilerGenerated);

                return new MethodMemberInstance(
                    elementMethod,
                    typeInstance.GenericArguments,
                    genericArguments
                );
            }

            var returnTypeReference = methodReference.ReturnType;
            var returnType = GetOrCreateStubTypeInstanceFromTypeReference(returnTypeReference);

            var name = methodReference.BuildMethodMemberName();
            var fullName = methodReference.BuildFullName();
            var isGeneric = methodReference.HasGenericParameters;
            var isCompilerGenerated = methodReference.IsCompilerGenerated();
            MethodForm methodForm;
            Visibility visibility;
            bool isStub;
            bool? isIterator;
            bool? isStatic;

            MethodDefinition methodDefinition;
            try
            {
                methodDefinition = methodReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                methodDefinition = null;
            }

            if (methodDefinition == null)
            {
                visibility = Public;
                methodForm = methodReference.HasConstructorName()
                    ? MethodForm.Constructor
                    : MethodForm.Normal;
                isIterator = null;
                isStatic = null;
                isStub = true;
            }
            else
            {
                visibility = methodDefinition.GetVisibility();
                methodForm = methodDefinition.GetMethodForm();
                isIterator = methodDefinition.IsIterator();
                isStatic = methodDefinition.IsStatic;
                isStub = false;
            }

            var methodMember = new MethodMember(
                name,
                fullName,
                typeInstance.Type,
                visibility,
                returnType,
                false,
                methodForm,
                isGeneric,
                isStub,
                isCompilerGenerated,
                isIterator,
                isStatic
            );

            var parameters = methodReference.GetParameters(this).ToList();
            methodMember.ParameterInstances.AddRange(parameters);

            var genericParameters = GetGenericParameters(methodReference);
            methodMember.GenericParameters.AddRange(genericParameters);

            return new MethodMemberInstance(
                methodMember,
                typeInstance.GenericArguments,
                Enumerable.Empty<GenericArgument>()
            );
        }

        [NotNull]
        internal FieldMember CreateStubFieldMemberFromFieldReference(
            [NotNull] IType type,
            [NotNull] FieldReference fieldReference
        )
        {
            var typeReference = fieldReference.FieldType;
            var fieldType = GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            var isCompilerGenerated = fieldReference.IsCompilerGenerated();
            bool? isStatic = null;
            var isReadOnly = false;

            if (fieldReference is FieldDefinition fieldDefinition)
            {
                isStatic = fieldDefinition.IsStatic;
                isReadOnly = fieldDefinition.IsInitOnly;
            }

            return new FieldMember(
                type,
                fieldReference.Name,
                fieldReference.FullName,
                Public,
                fieldType,
                isCompilerGenerated,
                isStatic,
                isReadOnly ? Writability.ReadOnly : Writability.Writable
            );
        }

        public IEnumerable<GenericParameter> GetGenericParameters(
            IGenericParameterProvider genericParameterProvider
        )
        {
            return genericParameterProvider == null
                ? Enumerable.Empty<GenericParameter>()
                : genericParameterProvider
                    .GenericParameters.Select(param =>
                        GetOrCreateStubTypeInstanceFromTypeReference(param).Type
                    )
                    .Cast<GenericParameter>();
        }

        private GenericParameter CreateGenericParameter(
            Mono.Cecil.GenericParameter genericParameter,
            [NotNull] string declarerFullName,
            bool declarerIsMethod
        )
        {
            if (
                declarerFullName == "System.Collections.Generic.List`1"
                && genericParameter.Name == "!0"
            )
            {
                ;
            }
            var isCompilerGenerated = genericParameter.IsCompilerGenerated();
            var variance = genericParameter.GetVariance();
            var typeConstraints = genericParameter.Constraints.Select(con =>
                GetOrCreateStubTypeInstanceFromTypeReference(con.ConstraintType)
            );
            return new GenericParameter(
                declarerFullName,
                genericParameter.Name,
                variance,
                typeConstraints,
                genericParameter.HasReferenceTypeConstraint,
                genericParameter.HasNotNullableValueTypeConstraint,
                genericParameter.HasDefaultConstructorConstraint,
                isCompilerGenerated,
                declarerIsMethod
            );
        }

        internal GenericArgument CreateGenericArgumentFromTypeReference(TypeReference typeReference)
        {
            return new GenericArgument(GetOrCreateStubTypeInstanceFromTypeReference(typeReference));
        }

        private void LoadBaseTask(IType cls, Type type, TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return;
            }

            _loadTaskRegistry.Add(
                typeof(AddBaseClassDependency),
                new AddBaseClassDependency(cls, type, typeDefinition, this)
            );
        }

        private void LoadNonBaseTasks(IType createdType, Type type, TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return;
            }

            _loadTaskRegistry.Add(
                typeof(AddMembers),
                new AddMembers(createdType, typeDefinition, this, type.Members)
            );
            _loadTaskRegistry.Add(
                typeof(AddGenericParameterDependencies),
                new AddGenericParameterDependencies(type)
            );
            _loadTaskRegistry.Add(
                typeof(AddAttributesAndAttributeDependencies),
                new AddAttributesAndAttributeDependencies(createdType, typeDefinition, this)
            );
            _loadTaskRegistry.Add(
                typeof(AddFieldAndPropertyDependencies),
                new AddFieldAndPropertyDependencies(createdType)
            );
            _loadTaskRegistry.Add(
                typeof(AddMethodDependencies),
                new AddMethodDependencies(createdType, typeDefinition, this)
            );
            _loadTaskRegistry.Add(
                typeof(AddGenericArgumentDependencies),
                new AddGenericArgumentDependencies(type)
            );
            _loadTaskRegistry.Add(
                typeof(AddClassDependencies),
                new AddClassDependencies(createdType, typeDefinition, this, type.Dependencies)
            );
            _loadTaskRegistry.Add(
                typeof(AddBackwardsDependencies),
                new AddBackwardsDependencies(createdType)
            );
        }
    }
}
