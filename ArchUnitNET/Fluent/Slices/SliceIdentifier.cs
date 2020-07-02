//  Copyright 2019 Florian Gather <florian.gather@tngtech.com>
// 	Copyright 2019 Paula Ruiz <paularuiz22@gmail.com>
// 	Copyright 2019 Fritz Brandhuber <fritz.brandhuber@tngtech.com>
// 
// 	SPDX-License-Identifier: Apache-2.0
// 

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceIdentifier : IHasDescription
    {
        public static readonly SliceIdentifierComparer Comparer = new SliceIdentifierComparer();
        public readonly int Identifier; //could use strings as identifiers to prevent collisions, but at cost of performance
        public readonly bool Ignored;


        private SliceIdentifier(int identifier, bool ignored, string description)
        {
            Identifier = identifier;
            Ignored = ignored;
            Description = description;
        }

        public string Description { get; }

        public static SliceIdentifier Of(string identifier)
        {
            return new SliceIdentifier(identifier.GetHashCode(), false, identifier);
        }

        public static SliceIdentifier Ignore()
        {
            return new SliceIdentifier(0, true, "Ignored");
        }

        /// <summary>
        ///     Is true when the two SliceIdentifiers belong to the same slice
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CompareTo(SliceIdentifier other)
        {
            if (other == null)
            {
                return false;
            }

            return Ignored && Ignored || !Ignored && !other.Ignored && Identifier == other.Identifier;
        }

        private bool Equals(SliceIdentifier other)
        {
            return Identifier == other.Identifier && Ignored == other.Ignored;
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

            return obj.GetType() == GetType() && Equals((SliceIdentifier) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 397 ^ Identifier.GetHashCode();
                hashCode = (hashCode * 397) ^ Ignored.GetHashCode();
                return hashCode;
            }
        }
    }
}