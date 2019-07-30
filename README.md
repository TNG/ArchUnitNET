<img src="Logo/ArchUnit-Logo.png" height="64" alt="ArchUnit">

# ArchUnitNET [![Build Status](https://travis-ci.com/fgather/ArchUnitNET.svg?branch=master)](https://travis-ci.com/fgather/ArchUnitNET) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/fgather/ArchUnitNET/blob/master/LICENSE)

license: Apache-2.0

ArchUnitNET is a free, simple library for checking the architecture of C# code. ArchUnitNET can check dependencies between
classes, members, interfaces, and more. This is done by analyzing C# bytecode and importing all classes into our C# code
structure. The main focus of ArchUnitNET is to automatically test architecture and coding rules.

## An Example
To use ArchUnitNET, install the ArchUnitNET package from NuGet:
```
PS> Install-Package ArchUnitNET
```
#### Create a Test
Then you will want to create a class to start testing. We used XUnit for our unit tests here, but you can also use other
testing tools, for example NUnit.
```cs

using System.Collections.Generic;
using System.Linq;

using ArchUnitNET.Core;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;

using Xunit;

namespace ExampleTest
{
    public class ExampleArchUnitTest
    {
        // TIP: load your architecture once at the start to maximize performance of your tests
        static readonly Architecture _architecture = new ArchLoader().LoadAssembly(typeof(FrenchChef).Assembly).Build();
        // replace <FrenchChef> with a class from your architecture

        // declare variables you'll use throughout your tests up here
        private readonly IEnumerable<Class> _chefs;
        private readonly Interface _cookInterface;

        // initialize your test variables in the constructor
        // TIP: access types of values from your architecture, then filter them using provided extension methods
        public ExampleArchUnitTest()
        {
            _chefs = _architecture.Classes.Where(cls => cls.NameEndsWith("Chef"));
            _cookInterface = _architecture.GetInterfaceOfType(typeof(ICook));
        }

        [Fact]
        public void AllChefsCook()
        {
            Assert.All(_chefs, chef => chef.Implements(_cookInterface));
        }
    }


    public class FrenchChef : ICook
    {
        private string _name;
        private int _age;

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
        private string _name;
        private int _age;

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
        }    }

    public interface ICook
    {
        void Cook();
    }
}
```


#### Further Info and Help
Check out test examples for the current release at 
[ArchUnitNET Examples](https://github.com/TNG/ArchUnitNET/master/ExampleTest "ExampleTests").


## License
ArchUnitNET is published under the Apache License 2.0. For more information concerning the license, see
[Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).
