//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using ArchUnitNET.Domain.Exceptions;
using ArchUnitNET.Domain.Extensions;
using Xunit;

namespace ArchUnitNETTests.Domain.Extensions
{
    public class NullableExtensionTests
    {
        [Fact]
        public void RequiredNotNullReturnsThisWhenNotNull()
        {
            var nonNullObject = new object();
            Assert.Equal(nonNullObject, nonNullObject.RequiredNotNull());
        }

        [Fact]
        public void RequiredNotNullThrowsExceptionWhenNull()
        {
            Assert.Throws<InvalidStateException>(() => ((object) null).RequiredNotNull());
        }
    }
}