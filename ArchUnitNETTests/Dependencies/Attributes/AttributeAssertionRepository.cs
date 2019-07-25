/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies.Members;
using ArchUnitNET.Domain.Dependencies.Types;
using ArchUnitNET.Fluent;
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