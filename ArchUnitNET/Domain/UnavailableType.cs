using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain.Dependencies;

namespace ArchUnitNET.Domain
{
    public class UnavailableType : IType
    {
        public UnavailableType(IType type)
        {
            Type = type;
        }

        private IType Type { get; }
        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public string AssemblyQualifiedName => Type.AssemblyQualifiedName;

        public Visibility Visibility => Type.Visibility;
        public bool IsNested => Type.IsNested;
        public bool IsGeneric => Type.IsGeneric;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsStub => true;
        public bool IsCompilerGenerated => Type.IsCompilerGenerated;

        public Namespace Namespace => Type.Namespace;
        public Assembly Assembly => Type.Assembly;

        public IEnumerable<Attribute> Attributes =>
            AttributeInstances.Select(instance => instance.Type);
        public List<AttributeInstance> AttributeInstances => Type.AttributeInstances;

        public List<ITypeDependency> Dependencies => Type.Dependencies;
        public List<ITypeDependency> BackwardsDependencies => Type.BackwardsDependencies;
        public IEnumerable<IType> ImplementedInterfaces => Type.ImplementedInterfaces;

        public MemberList Members => Type.Members;
        public List<GenericParameter> GenericParameters => Type.GenericParameters;

        public override string ToString()
        {
            return FullName;
        }

        private bool Equals(Struct other)
        {
            return Equals(Type, other.Type);
        }

        public bool Equals(UnavailableType other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Type.Equals(other.Type);
        }
        
        public override bool Equals(object obj)
        {
            return obj is UnavailableType unavailableType
                   && Equals(unavailableType);
        } 

        public override int GetHashCode()
        {
            return Type != null ? Type.GetHashCode() : 0;
        }
    }
}
