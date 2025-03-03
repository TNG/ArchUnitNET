using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class MethodMember : IMember
    {
        public MethodMember(
            string name,
            string fullName,
            IType declaringType,
            Visibility visibility,
            bool isVirtual,
            MethodForm methodForm,
            bool isGeneric,
            bool isStub,
            bool isCompilerGenerated,
            bool? isIterator,
            bool? isStatic
        )
        {
            Name = name;
            FullName = fullName;
            AssemblyQualifiedName = System.Reflection.Assembly.CreateQualifiedName(
                declaringType.Assembly.FullName,
                fullName
            );
            DeclaringType = declaringType;
            Visibility = visibility;
            IsVirtual = isVirtual;
            MethodForm = methodForm;
            IsGeneric = isGeneric;
            IsStub = isStub;
            IsCompilerGenerated = isCompilerGenerated;
            IsIterator = isIterator;
            IsStatic = isStatic;
        }

        public bool IsVirtual { get; }
        public MethodForm MethodForm { get; }

        public List<ITypeInstance<IType>> ParameterInstances { get; } =
            new List<ITypeInstance<IType>>();
        public IEnumerable<IType> Parameters =>
            ParameterInstances.Select(instance => instance.Type);
        public ITypeInstance<IType> ReturnTypeInstance { get; internal set; }
        public IType ReturnType => ReturnTypeInstance.Type;
        public bool IsStub { get; }
        public bool IsCompilerGenerated { get; }
        public bool? IsIterator { get; }
        public bool IsGeneric { get; }
        public bool? IsStatic { get; }
        public Writability? Writability => null;
        public List<GenericParameter> GenericParameters { get; } = new List<GenericParameter>();
        public Visibility Visibility { get; }
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

        public Assembly Assembly => DeclaringType.Assembly;
        public Namespace Namespace => DeclaringType.Namespace;
        public string Name { get; }
        public string FullName { get; }
        public string AssemblyQualifiedName { get; }
        public IType DeclaringType { get; }

        public override string ToString()
        {
            return $"{DeclaringType.FullName}::{Name}";
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

            return obj.GetType() == GetType() && Equals((MethodMember)obj);
        }

        private bool Equals(MethodMember other)
        {
            return FullName.Equals(other.FullName);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}
