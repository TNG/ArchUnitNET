//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

namespace ArchUnitNET.Domain.Dependencies
{
    public class CastTypeDependency : MemberTypeInstanceDependency
    {
        public CastTypeDependency(IMember originMember, ITypeInstance<IType> castTypeInstance)
            : base(originMember, castTypeInstance) { }
    }
}
