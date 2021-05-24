//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader.LoadTasks;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;
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

        public TypeFactory(TypeRegistry typeRegistry, MethodMemberRegistry methodMemberRegistry,
            LoadTaskRegistry loadTaskRegistry, AssemblyRegistry assemblyRegistry, NamespaceRegistry namespaceRegistry)
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
            return _typeRegistry.GetOrCreateTypeFromTypeReference(typeReference,
                s => CreateTypeFromTypeReference(typeReference, false)).Type;
        }

        [NotNull]
        internal ITypeInstance<IType> GetOrCreateStubTypeInstanceFromTypeReference(TypeReference typeReference)
        {
            return _typeRegistry.GetOrCreateTypeFromTypeReference(typeReference,
                s => CreateTypeFromTypeReference(typeReference, true));
        }

        [NotNull]
        internal MethodMemberInstance GetOrCreateMethodMemberFromMethodReference([NotNull] IType type,
            [NotNull] MethodReference methodReference)
        {
            return _methodMemberRegistry.GetOrCreateMethodFromMethodReference(methodReference,
                s => CreateMethodMemberFromMethodReference(new TypeInstance<IType>(type), methodReference));
        }

        [NotNull]
        internal MethodMemberInstance GetOrCreateMethodMemberFromMethodReference(
            [NotNull] ITypeInstance<IType> typeInstance, [NotNull] MethodReference methodReference)
        {
            return _methodMemberRegistry.GetOrCreateMethodFromMethodReference(methodReference,
                s => CreateMethodMemberFromMethodReference(typeInstance, methodReference));
        }

        [NotNull]
        private ITypeInstance<IType> CreateTypeFromTypeReference(TypeReference typeReference, bool isStub)
        {
            if (typeReference.IsGenericParameter)
            {
                var genericParameter = (Mono.Cecil.GenericParameter) typeReference;
                var declarerIsMethod = genericParameter.Type == GenericParameterType.Method;
                var declaringTypeFullName = declarerIsMethod
                    ? genericParameter.DeclaringMethod.BuildFullName()
                    : genericParameter.DeclaringType.BuildFullName();

                return new TypeInstance<GenericParameter>(CreateGenericParameter(genericParameter,
                    declaringTypeFullName,
                    declarerIsMethod));
            }

            if (typeReference.IsArray)
            {
                var dimensions = new List<int>();
                do
                {
                    var arrayType = (ArrayType) typeReference;
                    dimensions.Add(arrayType.Rank);
                    typeReference = arrayType.ElementType;
                } while (typeReference.IsArray);

                var elementTypeInstance = GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
                switch (elementTypeInstance.Type)
                {
                    case Interface intf:
                        return new TypeInstance<Interface>(intf, elementTypeInstance.GenericArguments, dimensions);
                    case Attribute att:
                        return new TypeInstance<Attribute>(att, elementTypeInstance.GenericArguments, dimensions);
                    case Class cls:
                        return new TypeInstance<Class>(cls, elementTypeInstance.GenericArguments, dimensions);
                    default:
                        return new TypeInstance<IType>(elementTypeInstance.Type, elementTypeInstance.GenericArguments,
                            dimensions);
                }
            }

            if (typeReference.IsGenericInstance)
            {
                var elementType = GetOrCreateStubTypeInstanceFromTypeReference(typeReference.GetElementType()).Type;
                var genericInstance = (GenericInstanceType) typeReference;
                var genericArguments = genericInstance.GenericArguments
                    .Select(CreateGenericArgumentFromTypeReference)
                    .Where(argument => !argument.Type.IsCompilerGenerated);
                switch (elementType)
                {
                    case Interface intf:
                        return new TypeInstance<Interface>(intf, genericArguments);
                    case Attribute att:
                        return new TypeInstance<Attribute>(att, genericArguments);
                    case Class cls:
                        return new TypeInstance<Class>(cls, genericArguments);
                    default:
                        return new TypeInstance<IType>(elementType, genericArguments);
                }
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

            var typeName = typeReference.BuildFullName();
            var currentNamespace = _namespaceRegistry.GetOrCreateNamespace(typeReference.IsNested
                ? typeReference.DeclaringType.Namespace
                : typeReference.Namespace);
            var currentAssembly = _assemblyRegistry.GetOrCreateAssembly(typeReference.Module.Assembly.Name.FullName,
                typeReference.Module.Assembly.FullName, true);

            Type type;
            bool isCompilerGenerated, isNested, isGeneric;

            if (typeDefinition == null)
            {
                isCompilerGenerated = typeReference.IsCompilerGenerated();
                isNested = typeReference.IsNested;
                isGeneric = typeReference.HasGenericParameters;
                type = new Type(typeName, typeReference.Name, currentAssembly, currentNamespace, NotAccessible,
                    isNested, isGeneric, true, isCompilerGenerated);

                return new TypeInstance<IType>(type);
            }

            var visibility = typeDefinition.GetVisibility();
            isCompilerGenerated = typeDefinition.IsCompilerGenerated();
            isNested = typeDefinition.IsNested;
            isGeneric = typeDefinition.HasGenericParameters;
            type = new Type(typeName, typeReference.Name, currentAssembly, currentNamespace, visibility, isNested,
                isGeneric, isStub, isCompilerGenerated);

            var genericParameters = GetGenericParameters(typeDefinition);
            type.GenericParameters.AddRange(genericParameters);

            ITypeInstance<IType> createdTypeInstance;
            var isInterface = typeDefinition.IsInterface;

            if (typeDefinition.IsInterface)
            {
                createdTypeInstance = new TypeInstance<Interface>(new Interface(type));
            }
            else
            {
                if (typeDefinition.IsAttribute())
                {
                    createdTypeInstance =
                        new TypeInstance<Attribute>(new Attribute(type, typeDefinition.IsAbstract,
                            typeDefinition.IsSealed));
                }
                else
                {
                    createdTypeInstance = new TypeInstance<Class>(new Class(type, typeDefinition.IsAbstract,
                        typeDefinition.IsSealed, typeDefinition.IsValueType, typeDefinition.IsEnum));
                }
            }

            if (!isStub && !isCompilerGenerated)
            {
                if (!isInterface)
                {
                    LoadBaseTask((Class) createdTypeInstance.Type, type, typeDefinition);
                }

                LoadNonBaseTasks(createdTypeInstance.Type, type, typeDefinition);
            }

            return createdTypeInstance;
        }

        [NotNull]
        private MethodMemberInstance CreateMethodMemberFromMethodReference(
            [NotNull] ITypeInstance<IType> typeInstance, [NotNull] MethodReference methodReference)
        {
            if (methodReference.IsGenericInstance)
            {
                var elementMethod =
                    CreateMethodMemberFromMethodReference(typeInstance, methodReference.GetElementMethod()).Member;

                var genericInstanceMethod = (GenericInstanceMethod) methodReference;
                var genericArguments = genericInstanceMethod.GenericArguments
                    .Select(CreateGenericArgumentFromTypeReference)
                    .Where(argument => !argument.Type.IsCompilerGenerated);

                return new MethodMemberInstance(elementMethod, typeInstance.GenericArguments, genericArguments);
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
                methodForm = methodReference.HasConstructorName() ? MethodForm.Constructor : MethodForm.Normal;
                isStub = true;
            }
            else
            {
                visibility = methodDefinition.GetVisibility();
                methodForm = methodDefinition.GetMethodForm();
                isStub = false;
            }

            var methodMember = new MethodMember(name, fullName, typeInstance.Type, visibility, returnType,
                false, methodForm, isGeneric, isStub, isCompilerGenerated);

            var parameters = methodReference.GetParameters(this).ToList();
            methodMember.ParameterInstances.AddRange(parameters);

            var genericParameters = GetGenericParameters(methodReference);
            methodMember.GenericParameters.AddRange(genericParameters);

            return new MethodMemberInstance(methodMember, typeInstance.GenericArguments,
                Enumerable.Empty<GenericArgument>());
        }

        [NotNull]
        internal FieldMember CreateStubFieldMemberFromFieldReference([NotNull] IType type,
            [NotNull] FieldReference fieldReference)
        {
            var typeReference = fieldReference.FieldType;
            var fieldType = GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            var isCompilerGenerated = fieldReference.IsCompilerGenerated();

            return new FieldMember(type, fieldReference.Name, fieldReference.FullName, Public, fieldType,
                isCompilerGenerated);
        }

        public IEnumerable<GenericParameter> GetGenericParameters(IGenericParameterProvider genericParameterProvider)
        {
            return genericParameterProvider == null
                ? Enumerable.Empty<GenericParameter>()
                : genericParameterProvider.GenericParameters
                    .Select(param => GetOrCreateStubTypeInstanceFromTypeReference(param).Type).Cast<GenericParameter>();
        }


        private GenericParameter CreateGenericParameter(Mono.Cecil.GenericParameter genericParameter,
            [NotNull] string declarerFullName, bool declarerIsMethod)
        {
            var isCompilerGenerated = genericParameter.IsCompilerGenerated();
            var variance = genericParameter.GetVariance();
            var typeConstraints = genericParameter.Constraints.Select(con =>
                GetOrCreateStubTypeInstanceFromTypeReference(con.ConstraintType));
            return new GenericParameter(declarerFullName, genericParameter.Name, variance, typeConstraints,
                genericParameter.HasReferenceTypeConstraint, genericParameter.HasNotNullableValueTypeConstraint,
                genericParameter.HasDefaultConstructorConstraint, isCompilerGenerated, declarerIsMethod);
        }

        internal GenericArgument CreateGenericArgumentFromTypeReference(TypeReference typeReference)
        {
            return new GenericArgument(GetOrCreateStubTypeInstanceFromTypeReference(typeReference));
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
            _loadTaskRegistry.Add(typeof(AddGenericParameterDependencies),
                new AddGenericParameterDependencies(type));
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