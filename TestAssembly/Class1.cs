﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

// ReSharper disable UnusedVariable

// ReSharper disable UnusedMember.Global

namespace TestAssembly
{
    public class Class1
    {
        public Class1(string testProperty)
        {
            TestProperty = testProperty;
        }

        public string TestProperty { get; }

        public string AccessClass2(int intparam)
        {
            var class2 = new Class2();
            return "";
        }
    }
}
