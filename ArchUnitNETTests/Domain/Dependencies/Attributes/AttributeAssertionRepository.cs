using System;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using ArchUnitNET.Domain.Extensions;
using Xunit;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNETTests.Domain.Dependencies.Attributes
{
    public static class AttributeAssertionRepository
    {
        public static void TypeAttributeAsExpected<TTargetType>(
            TTargetType targetType,
            Class expectedAttributeType,
            Attribute actualAttribute
        )
            where TTargetType : IType
        {
            //Precondition Checks
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (expectedAttributeType == null)
            {
                throw new ArgumentNullException(nameof(expectedAttributeType));
            }

            if (actualAttribute == null)
            {
                throw new ArgumentNullException(nameof(actualAttribute));
            }

            //Assert
            Assert.Contains(actualAttribute, targetType.Attributes);

            Assert.Equal(expectedAttributeType, actualAttribute);
        }

        public static void MemberAttributeAsExpected(
            IMember targetMember,
            Class expectedAttributeType,
            Attribute actualAttribute
        )
        {
            //Precondition Checks
            if (targetMember == null)
            {
                throw new ArgumentNullException(nameof(targetMember));
            }

            if (expectedAttributeType == null)
            {
                throw new ArgumentNullException(nameof(expectedAttributeType));
            }

            if (actualAttribute == null)
            {
                throw new ArgumentNullException(nameof(actualAttribute));
            }

            //Assert
            Assert.Contains(actualAttribute, targetMember.Attributes);

            Assert.Equal(expectedAttributeType, actualAttribute);
        }

        public static void AttributeDependencyAsExpected(
            IMember targetMember,
            Class expectedAttributeClass
        )
        {
            //Precondition Checks
            if (targetMember == null)
            {
                throw new ArgumentNullException(nameof(targetMember));
            }

            if (expectedAttributeClass == null)
            {
                throw new ArgumentNullException(nameof(expectedAttributeClass));
            }

            //Arrange, Act
            var expectedAttribute = new Attribute(expectedAttributeClass);

            var expectedAttributeDependency = new AttributeMemberDependency(
                targetMember,
                new AttributeInstance(expectedAttribute)
            );

            //Assert
            Assert.Contains(expectedAttributeDependency, targetMember.Dependencies);
        }

        public static void AttributeDependencyAsExpected(
            IType targetType,
            Class expectedAttributeClass
        )
        {
            //Precondition Checks
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (expectedAttributeClass == null)
            {
                throw new ArgumentNullException(nameof(expectedAttributeClass));
            }

            //Arrange, Act
            var expectedAttribute = new Attribute(expectedAttributeClass);

            var expectedAttributeDependency = new AttributeTypeDependency(
                targetType,
                new AttributeInstance(expectedAttribute)
            );

            //Assert
            Assert.True(targetType.HasDependency(expectedAttributeDependency));
        }
    }
}
