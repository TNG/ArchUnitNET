using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    public class SliceAssignment : IHasDescription
    {
        private readonly Func<IType, SliceIdentifier> _assignIdentifierFunc;

        public SliceAssignment(
            Func<IType, SliceIdentifier> assignIdentifierFunc,
            string description
        )
            : this(assignIdentifierFunc, description, false) { }

        internal SliceAssignment(
            Func<IType, SliceIdentifier> assignIdentifierFunc,
            string description,
            bool nestByDefault
        )
        {
            _assignIdentifierFunc = assignIdentifierFunc;
            Description = description;
            _nestByDefault = nestByDefault;
        }

        /// <summary>
        /// Whether diagrams nest the slices in packages even if
        /// <see cref="Domain.PlantUml.Export.GenerationOptions.NestSlicesInPackages"/> is not set;
        /// only for the obsolete <c>MatchingWithPackages</c>.
        /// </summary>
        private readonly bool _nestByDefault;

        public string Description { get; }

        public IEnumerable<Slice> Apply(IEnumerable<IType> types)
        {
            return types.GroupBy(
                _assignIdentifierFunc,
                (identifier, enumerable) =>
                    new Slice(identifier, enumerable) { NestByDefault = _nestByDefault },
                SliceIdentifier.Comparer
            );
        }
    }
}
