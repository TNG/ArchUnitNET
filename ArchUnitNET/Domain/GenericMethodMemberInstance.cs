//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using System.Collections.Generic;

namespace ArchUnitNET.Domain
{
    public class GenericMethodMemberInstance : MethodMember, IGenericMemberInstance
    {
        public GenericMethodMemberInstance(string name, string fullName, List<IType> parameters, IType returnType,
            IEnumerable<GenericParameter> genericParameters, MethodMember elementMethod,
            IEnumerable<IType> genericArguments) :
            base(name, fullName, elementMethod.DeclaringType, elementMethod.Visibility, parameters, returnType,
                elementMethod.IsVirtual, elementMethod.MethodForm, true, genericParameters)
        {
            IsGenericInstance = true;
            ElementMember = elementMethod;
            GenericArguments = genericArguments;
        }

        public IMember ElementMember { get; }
        public IEnumerable<IType> GenericArguments { get; }

        public override IMember GetElementMember()
        {
            return ElementMember;
        }

        public override IEnumerable<IType> GetGenericArguments()
        {
            return GenericArguments;
        }
    }
}