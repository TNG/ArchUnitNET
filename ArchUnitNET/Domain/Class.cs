using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Domain
{
    public class Class : IType
    {
        private IType Type { get; }

        public Class(
            IType type,
            bool? isAbstract = null,
            bool? isSealed = null,
            bool? isRecord = null
        )
        {
            Type = type;
            IsAbstract = isAbstract;
            IsSealed = isSealed;
            IsRecord = isRecord;
        }

        public Class(Class @class)
        {
            Type = @class.Type;
            IsAbstract = @class.IsAbstract;
            IsSealed = @class.IsSealed;
            IsRecord = @class.IsRecord;
        }

        public IEnumerable<ITypeDependency> DependenciesIncludingInherited =>
            BaseClass != null
                ? Type.Dependencies.Concat(BaseClass.DependenciesIncludingInherited)
                : Type.Dependencies;

        public MemberList MembersIncludingInherited =>
            BaseClass != null
                ? new MemberList(Type.Members.Concat(BaseClass.MembersIncludingInherited).ToList())
                : Type.Members;

        [CanBeNull]
        public Class BaseClass =>
            (Class)Dependencies.OfType<InheritsBaseClassDependency>().FirstOrDefault()?.Target;

        public IEnumerable<MethodMember> Constructors => Type.GetConstructors();
        public bool? IsAbstract { get; }
        public bool? IsSealed { get; }
        public bool? IsRecord { get; }

        public IEnumerable<Class> InheritedClasses =>
            BaseClass == null
                ? Enumerable.Empty<Class>()
                : BaseClass.InheritedClasses.Concat(new[] { BaseClass });

        public Visibility Visibility => Type.Visibility;
        public bool IsNested => Type.IsNested;
        public bool IsGeneric => Type.IsGeneric;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsStub => Type.IsStub;
        public bool IsCompilerGenerated => Type.IsCompilerGenerated;
        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public string AssemblyQualifiedName => Type.AssemblyQualifiedName;

        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;

        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;

        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances => Type.AttributeInstances;

        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;
        public MemberList Members => Type.Members;

        public List<GenericParameter> GenericParameters => Type.GenericParameters;

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Class other)
        {
            return Equals(Type, other.Type)
                && Equals(IsAbstract, other.IsAbstract)
                && Equals(IsSealed, other.IsSealed)
                && Equals(IsRecord, other.IsRecord);
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

            return obj.GetType() == GetType() && Equals((Class)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type != null ? Type.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ IsAbstract.GetHashCode();
                hashCode = (hashCode * 397) ^ IsSealed.GetHashCode();
                hashCode = (hashCode * 397) ^ IsRecord.GetHashCode();
                return hashCode;
            }
        }
    }
}
