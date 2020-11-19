//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Loader
{
    public class GenericTypeInstance : Type, IGenericTypeInstance
    {
        public GenericTypeInstance(string fullName, string name, IType elementType,
            IEnumerable<IType> genericArguments) :
            base(fullName, name, elementType.Assembly, elementType.Namespace, elementType.Visibility,
                elementType.IsNested, true, elementType.GenericParameters, true)
        {
            IsGenericInstance = true;
            ElementType = elementType;
            GenericArguments = genericArguments;
        }

        public IType ElementType { get; }
        public IEnumerable<IType> GenericArguments { get; }

        public override IType GetElementType()
        {
            return ElementType;
        }

        public override IEnumerable<IType> GetGenericArguments()
        {
            return GenericArguments;
        }
    }
}