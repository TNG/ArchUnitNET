using System;

namespace ArchUnitNET.Domain.Dependencies
{
    public class MemberGenericParameterTypeConstraintDependency : MemberTypeInstanceDependency
    {
        public MemberGenericParameterTypeConstraintDependency(
            GenericParameter originGenericParameter,
            ITypeInstance<IType> typeConstraintInstance
        )
            : base(originGenericParameter.DeclaringMethod, typeConstraintInstance)
        {
            if (!originGenericParameter.DeclarerIsMethod)
            {
                throw new ArgumentException(
                    "Use TypeGenericParameterTypeConstraintDependency for Generic Parameters of Types."
                );
            }

            OriginGenericParameter = originGenericParameter;
        }

        public GenericParameter OriginGenericParameter { get; }

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

            return obj.GetType() == GetType()
                && Equals((MemberGenericParameterTypeConstraintDependency)obj);
        }

        private bool Equals(MemberGenericParameterTypeConstraintDependency other)
        {
            return Equals(OriginGenericParameter, other.OriginGenericParameter)
                && Equals(TargetInstance, other.TargetInstance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode =
                    OriginGenericParameter != null ? OriginGenericParameter.GetHashCode() : 0;
                hashCode =
                    (hashCode * 397) ^ (TargetInstance != null ? TargetInstance.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
