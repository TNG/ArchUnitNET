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

namespace ArchUnitNETTests.Fluent.Syntax.Elements
{
    public class MatchGenericReturnTypesTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(System.Reflection.Assembly.Load("ArchUnitNETTests"))
            .Build();

        [Fact]
        public void OneGenericParameter()
        {
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(OneGenericReturnTypeExample))
                .And()
                .HaveName("CorrectGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .HaveReturnType(typeof(ExampleGenericClass<>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(OneGenericReturnTypeExample))
                .And()
                .HaveName("WrongGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .HaveReturnType(typeof(ExampleGenericClass<>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(OneGenericReturnTypeExample))
                .And()
                .HaveNameContaining("WrongReturnType")
                .Should()
                .Exist()
                .AndShould()
                .NotHaveReturnType(typeof(ExampleGenericClass<>))
                .Check(Architecture);
        }

        [Fact]
        public void TwoGenericParameters()
        {
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(TwoGenericReturnTypesExample))
                .And()
                .HaveName("CorrectGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .HaveReturnType(typeof(ExampleGenericClass<,>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(TwoGenericReturnTypesExample))
                .And()
                .HaveName("WrongGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .HaveReturnType(typeof(ExampleGenericClass<,>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(TwoGenericReturnTypesExample))
                .And()
                .HaveNameContaining("WrongReturnType")
                .Should()
                .Exist()
                .AndShould()
                .NotHaveReturnType(typeof(ExampleGenericClass<,>))
                .Check(Architecture);
        }

        [Fact]
        public void OneGenericArgument()
        {
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(OneGenericReturnTypeExample))
                .And()
                .HaveName("CorrectGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .HaveReturnType(typeof(ExampleGenericClass<ExampleArgument>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(OneGenericReturnTypeExample))
                .And()
                .HaveName("WrongGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .NotHaveReturnType(typeof(ExampleGenericClass<ExampleArgument>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(OneGenericReturnTypeExample))
                .And()
                .HaveNameContaining("WrongReturnType")
                .Should()
                .Exist()
                .AndShould()
                .NotHaveReturnType(typeof(ExampleGenericClass<ExampleArgument>))
                .Check(Architecture);
        }

        [Fact]
        public void TwoGenericArguments()
        {
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(TwoGenericReturnTypesExample))
                .And()
                .HaveName("CorrectGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .HaveReturnType(typeof(ExampleGenericClass<ExampleArgument, int>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(TwoGenericReturnTypesExample))
                .And()
                .HaveName("WrongGenericArgument()")
                .Should()
                .Exist()
                .AndShould()
                .NotHaveReturnType(typeof(ExampleGenericClass<ExampleArgument, int>))
                .Check(Architecture);
            MethodMembers()
                .That()
                .AreDeclaredIn(typeof(TwoGenericReturnTypesExample))
                .And()
                .HaveNameContaining("WrongReturnType")
                .Should()
                .Exist()
                .AndShould()
                .NotHaveReturnType(typeof(ExampleGenericClass<ExampleArgument, int>))
                .Check(Architecture);
        }
    }

    // ReSharper disable All
    class ExampleGenericClass<T> { }

    class ExampleGenericClass<T1, T2> { }

    class ExampleArgument { }

    class OneGenericReturnTypeExample
    {
        public ExampleGenericClass<ExampleArgument> CorrectGenericArgument()
        {
            return new ExampleGenericClass<ExampleArgument>();
        }

        public ExampleGenericClass<int> WrongGenericArgument()
        {
            return new ExampleGenericClass<int>();
        }

        public ExampleGenericClass<ExampleArgument, ExampleArgument> WrongReturnType1()
        {
            return new ExampleGenericClass<ExampleArgument, ExampleArgument>();
        }

        public void WrongReturnType2()
        {
            return;
        }

        public bool WrongReturnType3()
        {
            return true;
        }
    }

    class TwoGenericReturnTypesExample
    {
        public ExampleGenericClass<ExampleArgument, int> CorrectGenericArgument()
        {
            return new ExampleGenericClass<ExampleArgument, int>();
        }

        public ExampleGenericClass<int, ExampleArgument> WrongGenericArgument()
        {
            return new ExampleGenericClass<int, ExampleArgument>();
        }

        public ExampleGenericClass<ExampleArgument> WrongReturnType1()
        {
            return new ExampleGenericClass<ExampleArgument>();
        }

        public void WrongReturnType2()
        {
            return;
        }

        public bool WrongReturnType3()
        {
            return true;
        }
    }
}
