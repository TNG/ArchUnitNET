//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Extensions;
using ArchUnitNET.Loader;
using Xunit;

// ReSharper disable UnusedMember.Global

namespace ExampleTest
{
    public class ExampleArchUnitTestCooking
    {
        // initialize your test variables in the constructor
        // TIP: access types of values from your architecture, then filter them using provided extension methods
        public ExampleArchUnitTestCooking()
        {
            _chefs = ChefArchitecture.Classes.Where(cls => cls.NameEndsWith("Chef"));
            _cookInterface = ChefArchitecture.GetInterfaceOfType(typeof(ICook));
        }

        // TIP: load your architecture once at the start to maximize performance of your tests
        private static readonly Architecture ChefArchitecture =
            new ArchLoader().LoadAssembly(typeof(FrenchChef).Assembly).Build();
        // replace <FrenchChef> with a class from your architecture

        // declare variables you'll use throughout your tests up here
        private readonly IEnumerable<Class> _chefs;
        private readonly Interface _cookInterface;

        [Fact]
        public void AllChefsCook()
        {
            Assert.All(_chefs, chef => chef.ImplementsInterface(_cookInterface));
        }
    }


    public class FrenchChef : ICook
    {
        private int _age;
        private string _name;

        public FrenchChef(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public void Cook()
        {
            CremeBrulee();
            Crepes();
            Ratatouille();
        }

        private static void CremeBrulee()
        {
        }

        private static void Crepes()
        {
        }

        private static void Ratatouille()
        {
        }
    }

    public class ItalianChef : ICook
    {
        private int _age;
        private string _name;

        public ItalianChef(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public void Cook()
        {
            PizzaMargherita();
            Tiramisu();
            Lasagna();
        }

        private static void PizzaMargherita()
        {
        }

        private static void Tiramisu()
        {
        }

        private static void Lasagna()
        {
        }
    }

    public interface ICook
    {
        void Cook();
    }
}