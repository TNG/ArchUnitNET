/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

namespace ArchUnitNET.Domain
{
    public class Attribute : Class
    {
        public Attribute(IType type, bool isAbstract, bool isSealed) : base(type, isAbstract,
            isSealed, false, false)
        {
        }

        public Attribute(Class cls) : base(cls.Type, cls.IsAbstract, cls.IsSealed, cls.IsValueType, cls.IsSealed)
        {
        }
    }
}