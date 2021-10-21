//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 	Copyright 2020 Pavel Fischer <rubbiroid@gmail.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Domain.PlantUml
{
    public class PlantUmlDependency
    {
        public PlantUmlDependency(string origin, string target)
        {
            Origin = origin;
            Target = target;
        }

        public string Origin { get; }
        public string Target { get; }


        public override bool Equals(object obj)
        {
            return obj is PlantUmlDependency dependency &&
                   Equals(Origin, dependency.Origin) &&
                   Equals(Target, dependency.Target);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ (Origin != null ? Origin.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}