//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;

namespace ArchUnitNET.Domain
{
    public class GenericParameter
    {
        public GenericParameter(string name, GenericParameterVariance variance,
            IEnumerable<IType> typeConstraints, bool hasReferenceTypeConstraint, bool hasNotNullableValueTypeConstraint,
            bool hasDefaultConstructorConstraint)
        {
            Name = name;
            Attributes = new List<Attribute>();
            Variance = variance;
            TypeConstraints = typeConstraints;
            HasReferenceTypeConstraint = hasReferenceTypeConstraint;
            HasNotNullableValueTypeConstraint = hasNotNullableValueTypeConstraint;
            HasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
        }

        public string Name { get; }
        public List<Attribute> Attributes { get; }
        public GenericParameterVariance Variance { get; }
        public IEnumerable<IType> TypeConstraints { get; }
        public bool HasReferenceTypeConstraint { get; }
        public bool HasNotNullableValueTypeConstraint { get; }
        public bool HasDefaultConstructorConstraint { get; }

        public bool HasConstraints => HasReferenceTypeConstraint || HasNotNullableValueTypeConstraint ||
                                      HasDefaultConstructorConstraint || TypeConstraints.Any();

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

            return Equals(Name, other.Name) && Attributes.SequenceEqual(other.Attributes) &&
                   Equals(Variance, other.Variance) && TypeConstraints.SequenceEqual(other.TypeConstraints) &&
                   Equals(HasReferenceTypeConstraint, other.HasReferenceTypeConstraint) &&
                   Equals(HasNotNullableValueTypeConstraint, other.HasNotNullableValueTypeConstraint) &&
                   Equals(HasDefaultConstructorConstraint, other.HasDefaultConstructorConstraint);
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

            return Equals((GenericParameter) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = Attributes.Aggregate(hashCode,
                    (current, attribute) => (current * 397) ^ (attribute != null ? attribute.GetHashCode() : 0));
                hashCode = (hashCode * 397) ^ Variance.GetHashCode();
                hashCode = TypeConstraints.Aggregate(hashCode,
                    (current, type) => (current * 397) ^ (type != null ? type.GetHashCode() : 0));
                hashCode = (hashCode * 397) ^ HasReferenceTypeConstraint.GetHashCode();
                hashCode = (hashCode * 397) ^ HasNotNullableValueTypeConstraint.GetHashCode();
                hashCode = (hashCode * 397) ^ HasDefaultConstructorConstraint.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public enum GenericParameterVariance
    {
        NonVariant,
        Covariant,
        Contravariant
    }
}