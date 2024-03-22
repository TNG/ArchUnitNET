//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
//
// 	SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using ArchUnitNET.Loader;

namespace ArchUnitNET.Domain.Dependencies
{
    public class MethodCallDependency : MemberTypeInstanceDependency, IMemberMemberDependency
    {
        public MethodCallDependency(IMember originMember, MethodMemberInstance calledMethodInstance)
            : base(originMember, calledMethodInstance)
        {
            TargetMember = calledMethodInstance.Member;
            TargetMemberGenericArguments = calledMethodInstance.GenericArguments;
        }

        public IMember TargetMember { get; }
        public IEnumerable<GenericArgument> TargetMemberGenericArguments { get; }
    }
}
