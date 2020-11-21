//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader.LoadTasks;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;
using GenericParameter = ArchUnitNET.Domain.GenericParameter;

namespace ArchUnitNET.Loader
{
    internal class TypeFactory
    {
        private readonly AssemblyRegistry _assemblyRegistry;
        private readonly LoadTaskRegistry _loadTaskRegistry;
        private readonly NamespaceRegistry _namespaceRegistry;
        private readonly TypeRegistry _typeRegistry;

        public TypeFactory(TypeRegistry typeRegistry, LoadTaskRegistry loadTaskRegistry,
            AssemblyRegistry assemblyRegistry,
            NamespaceRegistry namespaceRegistry)
        {
            _loadTaskRegistry = loadTaskRegistry;
            _assemblyRegistry = assemblyRegistry;
            _namespaceRegistry = namespaceRegistry;
            _typeRegistry = typeRegistry;
        }

        public IEnumerable<IType> GetAllTypes()
        {
            return _typeRegistry.GetAllTypes();
        }

        [NotNull]
        internal IType GetOrCreateTypeFromTypeReference(TypeReference typeReference)
        {
            return _typeRegistry.GetOrCreateTypeFromTypeReference(typeReference,
                s => CreateTypeFromTypeReference(typeReference, false)).Type;
        }

        [NotNull]
        internal TypeInstance<IType> GetOrCreateStubTypeInstanceFromTypeReference(TypeReference typeReference)
        {
            return _typeRegistry.GetOrCreateTypeFromTypeReference(typeReference,
                f => CreateTypeFromTypeReference(typeReference, true));
        }

        [NotNull]
        private TypeInstance<IType> CreateTypeFromTypeReference(TypeReference typeReference, bool isStub)
        {
            if (typeReference.IsGenericParameter)
            {
                var genericParameter = (Mono.Cecil.GenericParameter) typeReference;
                return new TypeInstance<IType>(CreateGenericParameter(genericParameter));
            }

            TypeDefinition typeDefinition;
            try
            {
                typeDefinition = typeReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                typeDefinition = null;
            }

            if (typeReference.IsGenericInstance)
            {
                var elementType = GetOrCreateStubTypeInstanceFromTypeReference(typeReference.GetElementType()).Type;
                var genericInstance = (GenericInstanceType) typeReference;
                var genericArguments = genericInstance.GenericArguments
                    .Select(CreateGenericArgumentFromTypeReference);
                return new TypeInstance<IType>(elementType, genericArguments);
            }


            var type = SetupCreatedType(typeReference, isStub);
            if (typeDefinition == null)
            {
                return new TypeInstance<IType>(new Class(type));
            }

            IType createdType;

            if (typeDefinition.IsInterface)
            {
                createdType = new Interface(type);
            }
            else
            {
                createdType = IsAttribute(typeDefinition)
                    ? new Attribute(type, typeDefinition.IsAbstract, typeDefinition.IsSealed)
                    : new Class(type, typeDefinition.IsAbstract, typeDefinition.IsSealed,
                        typeDefinition.IsValueType, typeDefinition.IsEnum);
            }

            if (isStub)
            {
                return new TypeInstance<IType>(createdType);
            }

            if (createdType is Class @class)
            {
                LoadBaseTask(@class, type, typeDefinition);
            }

            LoadNonBaseTasks(createdType, type, typeDefinition);

            return new TypeInstance<IType>(createdType);
        }

        public IEnumerable<GenericParameter> GetGenericParameters(IGenericParameterProvider genericParameterProvider)
        {
            return genericParameterProvider == null
                ? Enumerable.Empty<GenericParameter>()
                : genericParameterProvider.GenericParameters.Select(CreateGenericParameter);
        }

        private GenericParameter CreateGenericParameter(Mono.Cecil.GenericParameter genericParameter)
        {
            var variance = GetVarianceFromGenericParameter(genericParameter);
            var typeConstraints = GetTypeConstraintsFromGenericParameter(genericParameter);
            return new GenericParameter(genericParameter.Name, variance, typeConstraints,
                genericParameter.HasReferenceTypeConstraint, genericParameter.HasNotNullableValueTypeConstraint,
                genericParameter.HasDefaultConstructorConstraint);
        }

        private IEnumerable<TypeInstance<IType>> GetTypeConstraintsFromGenericParameter(
            Mono.Cecil.GenericParameter genericParameter)
        {
            return genericParameter.Constraints.Select(con =>
                GetOrCreateStubTypeInstanceFromTypeReference(con.ConstraintType));
        }

        private static GenericParameterVariance GetVarianceFromGenericParameter(
            Mono.Cecil.GenericParameter genericParameter)
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

        public static string GetTypeFullNameForTypeReference(TypeReference typeReference)
        {
            return typeReference.FullName.Replace("/", "+");
        }

        private static bool IsAttribute([CanBeNull] TypeDefinition typeDefinition)
        {
            if (typeDefinition?.BaseType != null)
            {
                return typeDefinition.BaseType.FullName == "System.Attribute" ||
                       IsAttribute(typeDefinition.BaseType.Resolve());
            }

            return false;
        }

        [NotNull]
        private Type SetupCreatedType(TypeReference typeReference, bool isStub)
        {
            var typeNamespaceName = typeReference.Namespace;
            var currentAssembly = _assemblyRegistry.GetOrCreateAssembly(typeReference.Module.Assembly.Name.FullName,
                typeReference.Module.Assembly.FullName, true);
            var currentNamespace = _namespaceRegistry.GetOrCreateNamespace(typeNamespaceName);
            TypeDefinition typeDefinition;
            try
            {
                typeDefinition = typeReference.Resolve();
            }
            catch (AssemblyResolutionException)
            {
                typeDefinition = null;
            }

            var typeName = GetTypeFullNameForTypeReference(typeReference);
            var visibility = GetVisibilityFromTypeDefinition(typeDefinition);
            var isNested = typeReference.IsNested;
            var isGeneric = typeReference.HasGenericParameters;

            var type = new Type(typeName, typeReference.Name, currentAssembly, currentNamespace, visibility, isNested,
                isGeneric, isStub);

            var genericParameters = GetGenericParameters(typeDefinition);
            type.GenericParameters.AddRange(genericParameters);

            return type;
        }

        internal GenericArgument CreateGenericArgumentFromTypeReference(TypeReference typeReference)
        {
            return new GenericArgument(GetOrCreateStubTypeInstanceFromTypeReference(typeReference));
        }

        private static Visibility GetVisibilityFromTypeDefinition([CanBeNull] TypeDefinition typeDefinition)
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

            throw new ArgumentException("The provided type definition seems to have no visibility.");
        }

        private void LoadBaseTask(Class cls, Type type, TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return;
            }

            _loadTaskRegistry.Add(typeof(AddBaseClassDependency),
                new AddBaseClassDependency(cls, type, typeDefinition, this));
            _loadTaskRegistry.Add(typeof(AddGenericParameterDependencies),
                new AddGenericParameterDependencies(type));
        }

        private void LoadNonBaseTasks(IType createdType, Type type, TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return;
            }

            _loadTaskRegistry.Add(typeof(AddMembers),
                new AddMembers(createdType, typeDefinition, this, type.Members));
            _loadTaskRegistry.Add(typeof(AddAttributesAndAttributeDependencies),
                new AddAttributesAndAttributeDependencies(createdType, typeDefinition, this));
            _loadTaskRegistry.Add(typeof(AddFieldAndPropertyDependencies),
                new AddFieldAndPropertyDependencies(createdType));
            _loadTaskRegistry.Add(typeof(AddMethodDependencies),
                new AddMethodDependencies(createdType, typeDefinition, this));
            _loadTaskRegistry.Add(typeof(AddGenericArgumentDependencies),
                new AddGenericArgumentDependencies(type));
            _loadTaskRegistry.Add(typeof(AddClassDependencies),
                new AddClassDependencies(createdType, typeDefinition, this, type.Dependencies));
            _loadTaskRegistry.Add(typeof(AddBackwardsDependencies), new AddBackwardsDependencies(createdType));
        }
    }
}