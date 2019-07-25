/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paula.ruiz@tngtech.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.ArchitectureExceptions;
using ArchUnitNET.Fluent;
using Xunit;

namespace ArchUnitNETTests.Fluent
{
    public class NullableExtensionTests
    {
        [Fact]
        public void RequiredNotNullReturnsThisWhenNotNull()
        {
            var nonNullObject = new NullableExtensionTests();
            Assert.Equal(nonNullObject, nonNullObject.RequiredNotNull());
        }

        [Fact]
        public void RequiredNotNullThrowsExceptionWhenNull()
        {
            NullableExtensionTests nullObject = null;
            Assert.Throws<InvalidStateException>(() => nullObject.RequiredNotNull());
        }
    }
}