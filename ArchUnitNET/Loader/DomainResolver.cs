using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ArchUnitNET.Domain;
using JetBrains.Annotations;
using Mono.Cecil;
using static ArchUnitNET.Domain.Visibility;
using Attribute = ArchUnitNET.Domain.Attribute;
using Enum = ArchUnitNET.Domain.Enum;
using GenericParameter = ArchUnitNET.Domain.GenericParameter;

namespace ArchUnitNET.Loader
{
    /// <summary>
    /// Resolves Mono.Cecil type, method, and field references into cached domain objects.
    /// Creates and deduplicates <see cref="IType"/>, <see cref="MethodMember"/>,
    /// <see cref="FieldMember"/>, <see cref="Assembly"/>, and <see cref="Namespace"/>
    /// instances. Passed into every load-task phase as the single source of truth for
    /// domain object resolution.
    /// </summary>
    internal class DomainResolver
    {
        private readonly Dictionary<string, Assembly> _assemblies =
            new Dictionary<string, Assembly>();

        private readonly Dictionary<string, Namespace> _namespaces =
            new Dictionary<string, Namespace>();

        private readonly Dictionary<string, ITypeInstance<IType>> _allTypes =
            new Dictionary<string, ITypeInstance<IType>>();

        private readonly Dictionary<string, MethodMemberInstance> _allMethods =
            new Dictionary<string, MethodMemberInstance>();

        private readonly Dictionary<string, FieldMember> _allFields =
            new Dictionary<string, FieldMember>();

        /// <summary>
        /// All assemblies that have been created or cached.
        /// </summary>
        public IEnumerable<Assembly> Assemblies => _assemblies.Values;

        /// <summary>
        /// All namespaces that have been created or cached.
        /// </summary>
        public IEnumerable<Namespace> Namespaces => _namespaces.Values;

        /// <summary>
        /// All types that have been created or cached.
        /// </summary>
        public IEnumerable<ITypeInstance<IType>> Types => _allTypes.Values;

        /// <summary>
        /// Returns the existing <see cref="Assembly"/> for the given full name, or creates
        /// and caches a new one.
        /// </summary>
        internal Assembly GetOrCreateAssembly(
            string assemblyName,
            string assemblyFullName,
            bool isOnlyReferenced,
            List<string> assemblyReferences
        )
        {
            if (_assemblies.TryGetValue(assemblyFullName, out var existing))
            {
                return existing;
            }

            var assembly = new Assembly(
                assemblyName,
                assemblyFullName,
                isOnlyReferenced,
                assemblyReferences
            );
            _assemblies.Add(assemblyFullName, assembly);
            return assembly;
        }

        /// <summary>
        /// Returns the existing <see cref="Namespace"/> for the given name, or creates
        /// and caches a new one.
        /// </summary>
        internal Namespace GetOrCreateNamespace(string namespaceName)
        {
            if (_namespaces.TryGetValue(namespaceName, out var existing))
            {
                return existing;
            }

            var ns = new Namespace(namespaceName, new List<IType>());
            _namespaces.Add(namespaceName, ns);
            return ns;
        }

        /// <summary>
        /// Returns (or creates and caches) a type instance for the given type reference.
        /// Used during type discovery in <see cref="ArchBuilder.LoadTypesForModule"/>.
        /// </summary>
        [NotNull]
        internal ITypeInstance<IType> GetOrCreateTypeInstanceFromTypeReference(
            TypeReference typeReference
        )
        {
            return GetOrCreateTypeInstanceFromTypeReference(typeReference, false);
        }

        /// <summary>
        /// Returns (or creates and caches) a stub type instance for the given type reference.
        /// Used by load task phases when resolving dependency targets that may not have
        /// been discovered during module loading.
        /// </summary>
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

            TypeDefinition resolvedTypeDefinition;
            try
            {
                resolvedTypeDefinition = typeReference.Resolve();
            }
            catch (AssemblyResolutionException e)
            {
                throw new ArchLoaderException(
                    $"Could not resolve type {typeReference.FullName}",
                    e
                );
            }

            if (resolvedTypeDefinition == null)
            {
                // When assemblies are loaded by path, there are cases where a dependent type cannot be resolved because
                // the assembly dependency is not loaded in the current application domain. In this case, we create a
                // stub type.
                return GetOrCreateUnavailableTypeFromTypeReference(typeReference);
            }
            return GetOrCreateTypeInstanceFromTypeDefinition(resolvedTypeDefinition, isStub);
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
            var currentNamespace = GetOrCreateNamespace(declaringTypeReference.Namespace);
            var currentAssembly = GetOrCreateAssembly(
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
            var typeReferenceFullName = typeReference.BuildFullName();
            var assemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                typeReference.Scope.Name,
                typeReferenceFullName
            );
            if (_allTypes.TryGetValue(assemblyQualifiedName, out var existingTypeInstance))
            {
                return (ITypeInstance<UnavailableType>)existingTypeInstance;
            }

            var result = new TypeInstance<UnavailableType>(
                new UnavailableType(
                    new Type(
                        typeReferenceFullName,
                        typeReference.Name,
                        GetOrCreateAssembly(
                            typeReference.Scope.Name,
                            typeReference.Scope.ToString(),
                            true,
                            null
                        ),
                        GetOrCreateNamespace(typeReference.Namespace),
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
                true,
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

        /// <summary>
        /// Returns (or creates and caches) a <see cref="MethodMemberInstance"/> for the
        /// given method reference. Handles both generic and non-generic method references.
        /// </summary>
        [NotNull]
        public MethodMemberInstance GetOrCreateMethodMemberFromMethodReference(
            [NotNull] ITypeInstance<IType> typeInstance,
            [NotNull] MethodReference methodReference
        )
        {
            var methodReferenceFullName = methodReference.BuildFullName();
            if (methodReference.IsGenericInstance)
            {
                if (
                    _allMethods.TryGetValue(
                        methodReferenceFullName,
                        out var existingGenericInstance
                    )
                )
                {
                    return existingGenericInstance;
                }

                var genericResult = CreateGenericInstanceMethodMemberFromMethodReference(
                    typeInstance,
                    methodReference
                );
                _allMethods.Add(methodReferenceFullName, genericResult);
                return genericResult;
            }

            if (_allMethods.TryGetValue(methodReferenceFullName, out var existingMethodInstance))
            {
                return existingMethodInstance;
            }

            var name = methodReference.BuildMethodMemberName();
            var fullName = methodReferenceFullName;
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

        /// <summary>
        /// Returns (or creates and caches) a <see cref="FieldMember"/> for the given field
        /// reference. When the reference is a <see cref="FieldDefinition"/>, the field is
        /// created with full fidelity (visibility, exact writability). Otherwise a stub with
        /// default visibility (<see cref="Public"/>) is created.
        /// </summary>
        [NotNull]
        internal FieldMember GetOrCreateFieldMember(
            [NotNull] IType type,
            [NotNull] FieldReference fieldReference
        )
        {
            var fullName = fieldReference.FullName;
            if (_allFields.TryGetValue(fullName, out var existing))
            {
                return existing;
            }

            var typeReference = fieldReference.FieldType;
            var fieldType = GetOrCreateStubTypeInstanceFromTypeReference(typeReference);
            var isCompilerGenerated = fieldReference.IsCompilerGenerated();
            Visibility visibility;
            bool? isStatic;
            Writability writeAccessor;

            if (fieldReference is FieldDefinition fieldDefinition)
            {
                visibility = GetVisibilityFromFieldDefinition(fieldDefinition);
                isStatic = fieldDefinition.IsStatic;
                writeAccessor = fieldDefinition.IsInitOnly
                    ? Writability.ReadOnly
                    : Writability.Writable;
            }
            else
            {
                visibility = Public;
                isStatic = null;
                writeAccessor = Writability.Writable;
            }

            var fieldMember = new FieldMember(
                type,
                fieldReference.Name,
                fullName,
                visibility,
                fieldType,
                isCompilerGenerated,
                isStatic,
                writeAccessor
            );

            _allFields.Add(fullName, fieldMember);
            return fieldMember;
        }

        private static Visibility GetVisibilityFromFieldDefinition(
            [NotNull] FieldDefinition fieldDefinition
        )
        {
            if (fieldDefinition.IsPublic)
            {
                return Public;
            }

            if (fieldDefinition.IsPrivate)
            {
                return Private;
            }

            if (fieldDefinition.IsFamily)
            {
                return Protected;
            }

            if (fieldDefinition.IsAssembly)
            {
                return Internal;
            }

            if (fieldDefinition.IsFamilyOrAssembly)
            {
                return ProtectedInternal;
            }

            if (fieldDefinition.IsFamilyAndAssembly)
            {
                return PrivateProtected;
            }

            throw new ArgumentException("The field definition seems to have no visibility.");
        }

        /// <summary>
        /// Extracts generic parameters from a Cecil generic parameter provider (type or method)
        /// and returns them as domain <see cref="GenericParameter"/> instances.
        /// </summary>
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

        private GenericArgument CreateGenericArgumentFromTypeReference(TypeReference typeReference)
        {
            return new GenericArgument(GetOrCreateStubTypeInstanceFromTypeReference(typeReference));
        }
    }
}
