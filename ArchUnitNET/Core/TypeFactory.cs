/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Linq;
using ArchUnitNET.Core.LoadTasks;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNET.Core
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

        [NotNull]
        internal IType GetOrCreateTypeFromTypeReference(TypeReference typeReference)
        {
            return _typeRegistry.GetOrCreateTypeFromTypeReference(typeReference,
                s => CreateTypeFromTypeReference(typeReference, false));
        }

        [NotNull]
        internal IType GetOrCreateStubTypeFromTypeReference(TypeReference typeReference)
        {
            return _typeRegistry.GetOrCreateTypeFromTypeReference(typeReference,
                f => CreateTypeFromTypeReference(typeReference, true));
        }

        [NotNull]
        private IType CreateTypeFromTypeReference(TypeReference typeReference, bool isStub)
        {
            var type = SetupCreatedType(typeReference);

            var typeDefinition = typeReference.Resolve();
            if (typeDefinition == null)
            {
                return new Class(type);
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
                return createdType;
            }

            if (createdType is Class @class)
            {
                LoadBaseTask(@class, type, typeDefinition);
            }

            LoadNonBaseTasks(createdType, type, typeDefinition);

            return createdType;
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
        private Type SetupCreatedType(TypeReference typeReference)
        {
            var typeNamespaceName = typeReference.Namespace;
            var currentAssembly = _assemblyRegistry.GetOrCreateAssembly(typeReference.Module.Assembly.Name.FullName,
                typeReference.Module.Assembly.FullName, true);
            var currentNamespace = _namespaceRegistry.GetOrCreateNamespace(typeNamespaceName);
            var typeDefinition = typeReference.Resolve();
            var visibility = GetVisibilityFromTypeDefinition(typeDefinition);
            var isNested = typeReference.IsNested;
            var type = new Type(typeReference.FullName.Replace("/", "+"), typeReference.Name, currentAssembly,
                currentNamespace, visibility, isNested);
            AssignGenericProperties(typeReference, type, typeDefinition);
            return type;
        }

        private static Visibility GetVisibilityFromTypeDefinition(TypeDefinition typeDefinition)
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

        private void AssignGenericProperties(IGenericParameterProvider typeReference, Type type,
            TypeDefinition typeDefinition)
        {
            if (typeReference is GenericInstanceType genericInstanceType)
            {
                var elementTypeReference = genericInstanceType.ElementType;
                type.GenericType =
                    GetOrCreateStubTypeFromTypeReference(elementTypeReference);
                type.GenericTypeArguments = genericInstanceType.GenericArguments?
                    .AsEnumerable().Select(GetOrCreateStubTypeFromTypeReference).ToList();
            }

            if (typeReference.HasGenericParameters)
            {
                type.GenericTypeParameters = typeDefinition?.GenericParameters?
                    .Select(GetOrCreateStubTypeFromTypeReference).ToList();
            }
        }

        private void LoadBaseTask(Class cls, Type type, TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return;
            }

            _loadTaskRegistry.Add(typeof(AddBaseClassDependency),
                new AddBaseClassDependency(cls, type, typeDefinition, this));
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
            _loadTaskRegistry.Add(typeof(AddClassDependencies), new AddClassDependencies(createdType,
                typeDefinition, this,
                type.Dependencies));
            _loadTaskRegistry.Add(typeof(AddBackwardsDependencies), new AddBackwardsDependencies(createdType));
        }
    }
}