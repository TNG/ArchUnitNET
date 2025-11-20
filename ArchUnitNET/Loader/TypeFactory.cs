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
        private readonly NamespaceRegistry _namespaceRegistry;

        private readonly Dictionary<string, ITypeInstance<IType>> _allTypes =
            new Dictionary<string, ITypeInstance<IType>>();

        private readonly Dictionary<string, MethodMemberInstance> _allMethods =
            new Dictionary<string, MethodMemberInstance>();

        public TypeFactory(
            LoadTaskRegistry loadTaskRegistry,
            AssemblyRegistry assemblyRegistry,
            NamespaceRegistry namespaceRegistry
        )
        {
            _loadTaskRegistry = loadTaskRegistry;
            _assemblyRegistry = assemblyRegistry;
            _namespaceRegistry = namespaceRegistry;
        }

        public IEnumerable<IType> GetAllNonCompilerGeneratedTypes()
        {
            return _allTypes
                .Values.Select(instance => instance.Type)
                .Distinct()
                .Where(type => !type.IsCompilerGenerated);
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
            if (typeReference.IsGenericParameter)
            {
                return GetOrCreateGenericParameterTypeInstanceFromTypeReference(typeReference);
            }
            if (typeReference.IsArray)
            {
                return GetOrCreateArrayTypeInstanceFromTypeReference(typeReference);
            }
            if (typeReference.IsGenericInstance)
            {
                return GetOrCreateGenericInstanceTypeInstanceFromTypeReference(typeReference);
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
                return GetOrCreateFunctionPointerTypeInstanceFromTypeReference(functionPointerType);
            }
            if (typeReference is TypeDefinition typeDefinition)
            {
                return GetOrCreateTypeInstanceFromTypeDefinition(typeDefinition, isStub);
            }

            var resolvedTypeDefinition = ResolveTypeReferenceToTypeDefinition(typeReference);
            if (resolvedTypeDefinition == null)
            {
                // When assemblies are loaded by path, there are cases where a dependent type cannot be resolved because
                // the assembly dependency is not loaded in the current application domain. In this case, we create a
                // stub type.
                return GetOrCreateUnavailableTypeFromTypeReference(typeReference);
            }
            return GetOrCreateTypeInstanceFromTypeDefinition(resolvedTypeDefinition, isStub);
        }

        private TypeDefinition ResolveTypeReferenceToTypeDefinition(TypeReference typeReference)
        {
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
            return typeDefinition;
        }

        private ITypeInstance<IType> GetOrCreateGenericParameterTypeInstanceFromTypeReference(
            TypeReference typeReference
        )
        {
            var genericParameter = (Mono.Cecil.GenericParameter)typeReference;
            var declarerIsMethod = genericParameter.Type == GenericParameterType.Method;
            var declarerFullName = declarerIsMethod
                ? genericParameter.DeclaringMethod.BuildFullName()
                : genericParameter.DeclaringType.BuildFullName();
            var declaringTypeAssemblyName = declarerIsMethod
                ? genericParameter.DeclaringMethod.DeclaringType.Module.Assembly.FullName
                : genericParameter.DeclaringType.Module.Assembly.FullName;
            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                declaringTypeAssemblyName,
                $"{declarerFullName}+<{genericParameter.Name}>"
            );
            if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingTypeInstance))
            {
                return existingTypeInstance;
            }
            var isCompilerGenerated = genericParameter.IsCompilerGenerated();
            var variance = genericParameter.GetVariance();
            var typeConstraints = genericParameter.Constraints.Select(con =>
                GetOrCreateStubTypeInstanceFromTypeReference(con.ConstraintType)
            );
            var result = new TypeInstance<GenericParameter>(
                new GenericParameter(
                    declarerFullName,
                    genericParameter.Name,
                    variance,
                    typeConstraints,
                    genericParameter.HasReferenceTypeConstraint,
                    genericParameter.HasNotNullableValueTypeConstraint,
                    genericParameter.HasDefaultConstructorConstraint,
                    isCompilerGenerated,
                    declarerIsMethod
                )
            );
            _allTypes.Add(assemblyQualifiedName, result);
            return result;
        }

        private ITypeInstance<IType> GetOrCreateGenericInstanceTypeInstanceFromTypeReference(
            TypeReference typeReference
        )
        {
            var elementType = GetOrCreateStubTypeInstanceFromTypeReference(
                typeReference.GetElementType()
            ).Type;
            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                elementType.Assembly.FullName,
                typeReference.BuildFullName()
            );
            if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingTypeInstance))
            {
                return existingTypeInstance;
            }
            var genericInstance = (GenericInstanceType)typeReference;
            var genericArguments = genericInstance
                .GenericArguments.Select(CreateGenericArgumentFromTypeReference)
                .Where(argument => !argument.Type.IsCompilerGenerated)
                .ToList();
            var result = CreateTypeInstance(elementType, genericArguments, new List<int>());
            _allTypes.Add(assemblyQualifiedName, result);
            return result;
        }

        private ITypeInstance<IType> GetOrCreateArrayTypeInstanceFromTypeReference(
            TypeReference typeReference
        )
        {
            var dimensions = new List<int>();
            var elementType = typeReference;
            do
            {
                var arrayType = (ArrayType)elementType;
                dimensions.Add(arrayType.Rank);
                elementType = arrayType.ElementType;
            } while (elementType.IsArray);
            var elementTypeInstance = GetOrCreateStubTypeInstanceFromTypeReference(elementType);
            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                elementTypeInstance.Type.Assembly?.FullName ?? "",
                typeReference.BuildFullName()
            );
            if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingTypeInstance))
            {
                return existingTypeInstance;
            }
            var result = CreateTypeInstance(
                elementTypeInstance.Type,
                elementTypeInstance.GenericArguments,
                dimensions
            );
            _allTypes.Add(assemblyQualifiedName, result);
            return result;
        }

        [NotNull]
        private ITypeInstance<IType> GetOrCreateTypeInstanceFromTypeDefinition(
            TypeDefinition typeDefinition,
            bool isStub
        )
        {
            var assemblyFullName = typeDefinition.Module.Assembly.FullName;
            var fullName = typeDefinition.BuildFullName();

            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                assemblyFullName,
                fullName
            );
            if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingTypeInstance))
            {
                return existingTypeInstance;
            }

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
                    arrayTypeInstance.GenericArguments,
                    dimensions
                );
            }

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

                LoadNonBaseTasks(createdTypeInstance, type, typeDefinition);
            }

            _allTypes.Add(assemblyQualifiedName, createdTypeInstance);

            var genericParameters = GetGenericParameters(typeDefinition);
            type.GenericParameters.AddRange(genericParameters);

            return createdTypeInstance;
        }

        [NotNull]
        private ITypeInstance<UnavailableType> GetOrCreateUnavailableTypeFromTypeReference(
            TypeReference typeReference
        )
        {
            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                typeReference.Scope.Name,
                typeReference.BuildFullName()
            );
            if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingTypeInstance))
            {
                return (ITypeInstance<UnavailableType>)existingTypeInstance;
            }

            var result = new TypeInstance<UnavailableType>(
                new UnavailableType(
                    new Type(
                        typeReference.BuildFullName(),
                        typeReference.Name,
                        _assemblyRegistry.GetOrCreateAssembly(
                            typeReference.Scope.Name,
                            typeReference.Scope.ToString(),
                            true,
                            null
                        ),
                        _namespaceRegistry.GetOrCreateNamespace(typeReference.Namespace),
                        NotAccessible,
                        typeReference.IsNested,
                        typeReference.HasGenericParameters,
                        true,
                        typeReference.IsCompilerGenerated()
                    )
                )
            );
            _allTypes.Add(assemblyQualifiedName, result);
            return result;
        }

        [NotNull]
        private ITypeInstance<IType> CreateTypeInstance(
            IType type,
            IEnumerable<GenericArgument> genericArguments,
            IEnumerable<int> arrayDimensions
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
                case UnavailableType unavailableType:
                    return new TypeInstance<UnavailableType>(
                        unavailableType,
                        genericArguments,
                        arrayDimensions
                    );
                default:
                    throw new ArgumentException("Subtype of IType not recognized");
            }
        }

        [NotNull]
        private ITypeInstance<IType> GetOrCreateFunctionPointerTypeInstanceFromTypeReference(
            FunctionPointerType functionPointerType
        )
        {
            if (_allTypes.TryGetValue(functionPointerType.FullName, out var existingTypeInstance))
            {
                return existingTypeInstance;
            }
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
            var result = new TypeInstance<FunctionPointer>(
                new FunctionPointer(type, returnTypeInstance, parameterTypeInstances)
            );
            _allTypes.Add(functionPointerType.FullName, result);
            return result;
        }

        [NotNull]
        public MethodMemberInstance GetOrCreateMethodMemberFromMethodReference(
            [NotNull] ITypeInstance<IType> typeInstance,
            [NotNull] MethodReference methodReference
        )
        {
            var methodReferenceFullName = methodReference.BuildFullName();
            if (methodReference.IsGenericInstance)
            {
                return RegistryUtils.GetFromDictOrCreateAndAdd(
                    methodReferenceFullName,
                    _allMethods,
                    _ =>
                        CreateGenericInstanceMethodMemberFromMethodReference(
                            typeInstance,
                            methodReference
                        )
                );
            }

            if (_allMethods.TryGetValue(methodReferenceFullName, out var existingMethodInstance))
            {
                return existingMethodInstance;
            }

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
                false,
                methodForm,
                isGeneric,
                isStub,
                isCompilerGenerated,
                isIterator,
                isStatic
            );

            var result = new MethodMemberInstance(
                methodMember,
                typeInstance.GenericArguments,
                Enumerable.Empty<GenericArgument>()
            );

            _allMethods.Add(methodReferenceFullName, result);

            var genericParameters = GetGenericParameters(methodReference);
            methodMember.GenericParameters.AddRange(genericParameters);

            var returnTypeReference = methodReference.ReturnType;
            methodMember.ReturnTypeInstance = GetOrCreateStubTypeInstanceFromTypeReference(
                returnTypeReference
            );

            var parameters = methodReference.GetParameters(this).ToList();
            methodMember.ParameterInstances.AddRange(parameters);

            return result;
        }

        private MethodMemberInstance CreateGenericInstanceMethodMemberFromMethodReference(
            ITypeInstance<IType> typeInstance,
            MethodReference methodReference
        )
        {
            var elementMethod = GetOrCreateMethodMemberFromMethodReference(
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

        private void LoadNonBaseTasks(
            ITypeInstance<IType> createdTypeInstance,
            Type type,
            TypeDefinition typeDefinition
        )
        {
            if (typeDefinition == null)
            {
                return;
            }

            _loadTaskRegistry.Add(
                typeof(AddMembers),
                new AddMembers(createdTypeInstance, typeDefinition, this, type.Members)
            );
            _loadTaskRegistry.Add(
                typeof(AddGenericParameterDependencies),
                new AddGenericParameterDependencies(type)
            );
            _loadTaskRegistry.Add(
                typeof(AddAttributesAndAttributeDependencies),
                new AddAttributesAndAttributeDependencies(
                    createdTypeInstance.Type,
                    typeDefinition,
                    this
                )
            );
            _loadTaskRegistry.Add(
                typeof(AddFieldAndPropertyDependencies),
                new AddFieldAndPropertyDependencies(createdTypeInstance.Type)
            );
            _loadTaskRegistry.Add(
                typeof(AddMethodDependencies),
                new AddMethodDependencies(createdTypeInstance.Type, typeDefinition, this)
            );
            _loadTaskRegistry.Add(
                typeof(AddGenericArgumentDependencies),
                new AddGenericArgumentDependencies(type)
            );
            _loadTaskRegistry.Add(
                typeof(AddClassDependencies),
                new AddClassDependencies(
                    createdTypeInstance.Type,
                    typeDefinition,
                    this,
                    type.Dependencies
                )
            );
            _loadTaskRegistry.Add(
                typeof(AddBackwardsDependencies),
                new AddBackwardsDependencies(createdTypeInstance.Type)
            );
        }
    }
}
