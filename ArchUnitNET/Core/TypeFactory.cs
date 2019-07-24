/*
 * Copyright 2019 TNG Technology Consulting GmbH
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

using System.Linq;
using ArchUnitNET.Core.LoadTasks;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;

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
                return new Class(type, false);
            }

            IType createdType;

            if (typeDefinition.IsInterface)
            {
                createdType = new Interface(type);
            }
            else
            {
                var cls = new Class(type, typeDefinition.IsAbstract);
                createdType = cls;
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

        [NotNull]
        private Type SetupCreatedType(TypeReference typeReference)
        {
            var typeNamespaceName = typeReference.Namespace;
            var currentAssembly = _assemblyRegistry.GetOrCreateAssembly(typeReference.Module.Assembly.Name.FullName,
                typeReference.Module.Assembly.FullName);
            var currentNamespace = _namespaceRegistry.GetOrCreateNamespace(typeNamespaceName);
            var typeDefinition = typeReference.Resolve();
            var type = new Type(typeReference.FullName.Replace("/", "+"), typeReference.Name, currentAssembly,
                currentNamespace);
            AssignGenericProperties(typeReference, type, typeDefinition);
            return type;
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