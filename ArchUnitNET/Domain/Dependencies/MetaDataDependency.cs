//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
//
// 	SPDX-License-Identifier: Apache-2.0
//

namespace ArchUnitNET.Domain.Dependencies
{
    public class MetaDataDependency : MemberTypeInstanceDependency
    {
        public MetaDataDependency(IMember originMember, ITypeInstance<IType> metaDataTypeInstance)
            : base(originMember, metaDataTypeInstance) { }
    }
}
