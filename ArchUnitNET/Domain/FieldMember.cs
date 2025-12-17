using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class FieldMember : IMember
    {
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
            Type = typeInstance;
            IsStatic = isStatic;
            Writability = writability;
        }

        public ITypeInstance<IType> Type { get; }

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
        public List<ITypeDependency> Dependencies { get; } = new List<ITypeDependency>();
        public List<ITypeDependency> BackwardsDependencies { get; } = new List<ITypeDependency>();

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
