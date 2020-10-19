//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Domain.Dependencies
{
    public class MemberTypeDependency : IMemberTypeDependency
    {
        public MemberTypeDependency(IMember originMember, IType target)
        {
            Origin = originMember.DeclaringType;
            Target = target;
            OriginMember = originMember;
        }

        public IType Origin { get; }
        public IType Target { get; }
        public IMember OriginMember { get; }
    }
}