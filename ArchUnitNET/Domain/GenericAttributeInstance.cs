//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Domain
{
    public class GenericAttributeInstance : GenericClassInstance
    {
        public GenericAttributeInstance(IGenericTypeInstance type, bool isAbstract, bool isSealed) :
            base(type, isAbstract, isSealed, false, false)
        {
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        public GenericAttributeInstance(GenericClassInstance cls) :
            base((IGenericTypeInstance) cls.Type, cls.IsAbstract, cls.IsSealed, cls.IsValueType, cls.IsEnum)
        {
        }
    }
}