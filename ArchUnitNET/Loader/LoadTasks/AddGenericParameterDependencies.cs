//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Dependencies;
using JetBrains.Annotations;

namespace ArchUnitNET.Loader.LoadTasks
{
    public class AddGenericParameterDependencies : ILoadTask
    {
        [NotNull] private readonly IType _type;

        public AddGenericParameterDependencies([NotNull] IType type)
        {
            _type = type;
        }

        public void Execute()
        {
            AddTypeGenericParameterDependencies();
            AddMemberGenericParameterDependencies();
        }

        private void AddTypeGenericParameterDependencies()
        {
            foreach (var genericParameter in _type.GenericParameters)
            {
                genericParameter.AssignDeclarer(_type);
                foreach (var typeInstanceConstraint in genericParameter.TypeInstanceConstraints)
                {
                    var dependency =
                        new TypeGenericParameterTypeConstraintDependency(genericParameter, typeInstanceConstraint);
                    genericParameter.Dependencies.Add(dependency);
                }
            }
        }

        private void AddMemberGenericParameterDependencies()
        {
            foreach (var member in _type.Members)
            {
                foreach (var genericParameter in member.GenericParameters)
                {
                    genericParameter.AssignDeclarer(member);
                    foreach (var typeInstanceConstraint in genericParameter.TypeInstanceConstraints)
                    {
                        var dependency =
                            new MemberGenericParameterTypeConstraintDependency(genericParameter,
                                typeInstanceConstraint);
                        genericParameter.Dependencies.Add(dependency);
                    }
                }
            }
        }
    }
}