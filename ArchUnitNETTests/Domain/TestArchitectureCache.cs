/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using ArchUnitNET.Domain;

namespace ArchUnitNETTests.Domain
{
    public class TestArchitectureCache : ArchitectureCache
    {
        public int Size()
        {
            return Cache.Count;
        }

        public void Clear()
        {
            Cache.Clear();
        }
    }
}