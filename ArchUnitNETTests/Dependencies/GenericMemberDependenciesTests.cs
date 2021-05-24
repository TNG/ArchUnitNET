﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNETTests.Dependencies
{
    public class GenericMemberDependenciesTests
    {
    }

    internal class GenericClass<T>
    {
        public M GenericMethod<M>(T t) where M : new()
        {
            return new M();
        }
    }
}