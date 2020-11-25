//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodSignatureDependency : MemberTypeInstanceDependency
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public MethodSignatureDependency(MethodMember method, ITypeInstance<IType> signatureTypeInstance) : base(method,
            signatureTypeInstance)
        {
        }
    }
}