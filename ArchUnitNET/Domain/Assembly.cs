﻿//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0

namespace ArchUnitNET.Domain
{
    public class Assembly : IHasName
    {
        public Assembly(string name, string fullName, bool isOnlyReferenced)
        {
            Name = name;
            FullName = fullName;
            IsOnlyReferenced = isOnlyReferenced;
        }

        public bool IsOnlyReferenced { get; }

        public string Name { get; }
        public string FullName { get; }

        public bool Equals(Assembly other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Name, other.Name) && Equals(FullName, other.FullName) &&
                   Equals(IsOnlyReferenced, other.IsOnlyReferenced);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Assembly) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name != null ? Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsOnlyReferenced.GetHashCode();
                return hashCode;
            }
        }
    }
}