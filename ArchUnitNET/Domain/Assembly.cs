/*
 * Copyright 2019 Florian Gather <florian.gather@tngtech.com>
 * Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
 *
 * SPDX-License-Identifier: Apache-2.0
 */

using Equ;

namespace ArchUnitNET.Domain
{
    public class Assembly : MemberwiseEquatable<Assembly>, IHasName
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
    }
}