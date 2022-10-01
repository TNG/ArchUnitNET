//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace ArchUnitNETTests.Loader
{
    public class NestedTypesTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNETTests")).Build();

        [Fact]
        public void FindAllNestedTypes()
        {
            Types().That().Are(typeof(Layer1Class.Layer2Class)).Should().Exist().Check(Architecture);
            Types().That().Are(typeof(Layer1Class.Layer2Class.Layer3Class1)).Should().Exist().Check(Architecture);
            Types().That().Are(typeof(Layer1Class.Layer2Class.Layer3Class1.Layer4Class1)).Should().Exist()
                .Check(Architecture);
            Types().That().Are(typeof(Layer1Class.Layer2Class.Layer3Class2)).Should().Exist().Check(Architecture);
            Types().That().Are(typeof(Layer1Class.Layer2Class.Layer3Class2.Layer4Class2)).Should().Exist()
                .Check(Architecture);
        }
    }

    public class Layer1Class
    {
        public class Layer2Class
        {
            public class Layer3Class1
            {
                public class Layer4Class1
                {
                }
            }

            public class Layer3Class2
            {
                public class Layer4Class2
                {
                }
            }
        }
    }
}