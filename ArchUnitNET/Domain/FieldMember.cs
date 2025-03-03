using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class FieldMember : IMember, ITypeInstance<IType>
    {
        private readonly ITypeInstance<IType> _typeInstance;

        public FieldMember(
            IType declaringType,
            string name,
            string fullName,
            Visibility visibility,
            ITypeInstance<IType> typeInstance,
            bool isCompilerGenerated,
            bool? isStatic,
            Writability writability
        )
        {
            DeclaringType = declaringType;
            Name = name;
            FullName = fullName;
            AssemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                declaringType.Assembly.FullName,
                fullName
            );
            Visibility = visibility;
            IsCompilerGenerated = isCompilerGenerated;
            _typeInstance = typeInstance;
            IsStatic = isStatic;
            Writability = writability;
        }

        public Assembly Assembly => DeclaringType.Assembly;
        public Namespace Namespace => DeclaringType.Namespace;
        public Visibility Visibility { get; }
        public IType DeclaringType { get; }
        public string Name { get; }
        public string FullName { get; }
        public string AssemblyQualifiedName { get; }

        public bool IsCompilerGenerated { get; }
        public bool? IsStatic { get; }
        public Writability? Writability { get; }
        public bool IsGeneric => false;
        public List<GenericParameter> GenericParameters => new List<GenericParameter>();
        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances { get; } = new List<AttributeInstance>();
        public List<IMemberTypeDependency> MemberDependencies { get; } =
            new List<IMemberTypeDependency>();
        public List<IMemberTypeDependency> MemberBackwardsDependencies { get; } =
            new List<IMemberTypeDependency>();
        public List<ITypeDependency> Dependencies =>
            MemberDependencies.Cast<ITypeDependency>().ToList();

        public List<ITypeDependency> BackwardsDependencies =>
            MemberBackwardsDependencies.Cast<ITypeDependency>().ToList();

        public IType Type => _typeInstance.Type;
        public IEnumerable<GenericArgument> GenericArguments => _typeInstance.GenericArguments;
        public IEnumerable<int> ArrayDimensions => _typeInstance.ArrayDimensions;
        public bool IsArray => _typeInstance.IsArray;

        public override string ToString()
        {
            return $"{DeclaringType.FullName}{'.'}{Name}";
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

            return obj.GetType() == GetType() && Equals((FieldMember)obj);
        }

        private bool Equals(FieldMember other)
        {
            return FullName.Equals(other.FullName);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}
