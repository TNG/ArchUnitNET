/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent.Extensions;
using Xunit;
using Attribute = ArchUnitNET.Domain.Attribute;

namespace ArchUnitNETTests.Dependencies.Attributes
{
    public static class AttributeAssertionRepository
    {
        public static void TypeAttributeAsExpected<TTargetType>(TTargetType targetType, Class expectedAttributeType,
            Attribute actualAttribute) where TTargetType : IType
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

            Assert.Equal(expectedAttributeType, actualAttribute.Type);
        }

        public static void MemberAttributeAsExpected(IMember targetMember, Class expectedAttributeType,
            Attribute actualAttribute)
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

            Assert.Equal(expectedAttributeType, actualAttribute.Type);
        }

        public static void AttributeDependencyAsExpected(IMember targetMember, Class expectedAttributeClass)
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

            var expectedAttributeDependency =
                new AttributeMemberDependency(targetMember, expectedAttribute);

            //Assert
            Assert.Equal(expectedAttributeDependency,
                targetMember.Dependencies.First());

            Assert.True(targetMember.HasDependency(expectedAttributeDependency));
        }

        public static void AttributeDependencyAsExpected(IType targetType, Class expectedAttributeClass)
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

            var expectedAttributeDependency =
                new AttributeTypeDependency(targetType, expectedAttribute);

            //Assert
            Assert.True(targetType.HasDependency(expectedAttributeDependency));
        }
    }
}