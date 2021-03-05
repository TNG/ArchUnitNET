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
    public class GenericMethodTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssembly(typeof(GenericMethodTests).Assembly).Build();

        private readonly MethodMember _oneGenericArgumentMethod;
        private readonly MethodMember _twoGenericArgumentsMethod;

        public GenericMethodTests()
        {
            var intf = Architecture.GetInterfaceOfType(typeof(IInterfaceWithGenericMethodsWithSameName));
            _oneGenericArgumentMethod = intf.GetMethodMembers()
                .First(member => member.GenericParameters.Count == 1);
            _twoGenericArgumentsMethod = intf.GetMethodMembers()
                .First(member => member.GenericParameters.Count == 2);
        }


        [Fact]
        public void AssignDifferentGenericParametersToMethodsWithSameName()
        {
            Assert.NotNull(_oneGenericArgumentMethod);
            Assert.NotNull(_twoGenericArgumentsMethod);

            Assert.Empty(
                _oneGenericArgumentMethod.GenericParameters.Intersect(_twoGenericArgumentsMethod.GenericParameters));
        }
    }

    internal interface IInterfaceWithGenericMethodsWithSameName
    {
        void Method<T, K>();
        void Method<T>();
    }
}