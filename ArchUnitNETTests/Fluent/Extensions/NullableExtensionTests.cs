//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Fluent.Exceptions;
using ArchUnitNET.Fluent.Extensions;
using Xunit;

namespace ArchUnitNETTests.Fluent.Extensions
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