//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using Xunit;

namespace ArchUnitNETTests.Domain
{
    public class FixedSizeBufferTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(FixedSizeBufferTests).Assembly).Build();

        private Class _structWithUnsafeContent;

        public FixedSizeBufferTests()
        {
            _structWithUnsafeContent = Architecture.GetClassOfType(typeof(StructWithFixedSizeBuffer));
        }

        [Fact]
        public void NoCompilerGeneratedFieldTest()
        {
            var fieldMembers = _structWithUnsafeContent.GetFieldMembers().ToList();
            Assert.Single(fieldMembers);
            Assert.False(fieldMembers[0].Type.IsCompilerGenerated);
        }
    }

    public struct StructWithFixedSizeBuffer
    {
        public unsafe fixed char FixedCharArray[256];
    }
}