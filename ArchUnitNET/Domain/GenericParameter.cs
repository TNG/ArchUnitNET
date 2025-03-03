using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class GenericParameter : IType
    {
        internal readonly IEnumerable<ITypeInstance<IType>> TypeInstanceConstraints;

        public GenericParameter(
            ITypeInstance<IType> declaringTypeInstance,
            [CanBeNull] MethodMemberInstance declaringMethodInstance,
            string fullName,
            string name,
            GenericParameterVariance variance,
            IEnumerable<ITypeInstance<IType>> typeConstraints,
            bool hasReferenceTypeConstraint,
            bool hasNotNullableValueTypeConstraint,
            bool hasDefaultConstructorConstraint,
            bool isCompilerGenerated
        )
        {
            DeclaringTypeInstance = declaringTypeInstance;
            DeclaringMethodInstance = declaringMethodInstance;
            FullName = fullName;
            Name = name;
            Variance = variance;
            TypeInstanceConstraints = typeConstraints;
            HasReferenceTypeConstraint = hasReferenceTypeConstraint;
            HasNotNullableValueTypeConstraint = hasNotNullableValueTypeConstraint;
            HasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
            IsCompilerGenerated = isCompilerGenerated;
        }

        public ITypeInstance<IType> DeclaringTypeInstance { get; }
        public IType DeclaringType => DeclaringTypeInstance.Type;

        [CanBeNull]
        public MethodMemberInstance DeclaringMethodInstance { get; }

        [CanBeNull]
        public IMember DeclaringMethod => DeclaringMethodInstance?.Member;
        public bool DeclarerIsMethod => DeclaringMethodInstance != null;
        public GenericParameterVariance Variance { get; }
        public IEnumerable<IType> TypeConstraints =>
            TypeInstanceConstraints.Select(instance => instance.Type);
        public bool HasReferenceTypeConstraint { get; }
        public bool HasNotNullableValueTypeConstraint { get; }
        public bool HasDefaultConstructorConstraint { get; }

        public bool HasConstraints =>
            HasReferenceTypeConstraint
            || HasNotNullableValueTypeConstraint
            || HasDefaultConstructorConstraint
            || TypeConstraints.Any();

        public string Name { get; }
        public string FullName { get; }
        public string AssemblyQualifiedName =>
            System.Reflection.Assembly.CreateQualifiedName(
                DeclaringType.Assembly.FullName,
                FullName
            );
        public bool IsCompilerGenerated { get; }
        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances { get; } = new List<AttributeInstance>();

        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();
        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();
        public Visibility Visibility => Visibility.NotAccessible;
        public bool IsGeneric => false;
        public bool IsGenericParameter => true;
        public List<GenericParameter> GenericParameters => new List<GenericParameter>();
        public Namespace Namespace => DeclaringType?.Namespace;
        public Assembly Assembly => DeclaringType?.Assembly;
        public MemberList Members => new MemberList();
        public IEnumerable<IType> ImplementedInterfaces => Enumerable.Empty<IType>();

        public bool IsNested => true;
        public bool IsStub => true;

        public bool Equals(GenericParameter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(FullName, other.FullName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((GenericParameter)obj);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override string ToString()
        {
            return FullName;
        }
    }

    public enum GenericParameterVariance
    {
        NonVariant,
        Covariant,
        Contravariant,
    }
}
