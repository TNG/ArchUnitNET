using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceAssignment : IHasDescription
    {
        private readonly Func<IType, FrozenRuleSliceIdentifier> _assignIdentifierFunc;

        public SliceAssignment(
            Func<IType, FrozenRuleSliceIdentifier> assignIdentifierFunc,
            string description
        )
        {
            _assignIdentifierFunc = assignIdentifierFunc;
            Description = description;
        }

        public string Description { get; }

        public IEnumerable<Slice> Apply(IEnumerable<IType> types)
        {
            return types.GroupBy(
                _assignIdentifierFunc,
                (identifier, enumerable) => new Slice(identifier, enumerable),
                FrozenRuleSliceIdentifier.Comparer
            );
        }
    }
}
