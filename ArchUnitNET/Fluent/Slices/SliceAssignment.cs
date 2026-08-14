using System;
using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Slices
{
    /// <summary>
    ///     Defines how types are assigned to slices. The assignment function maps each type to a
    ///     <see cref="SliceIdentifier"/>, and types with the same identifier are grouped into the
    ///     same <see cref="Slice"/>.
    /// </summary>
    public class SliceAssignment : IHasDescription
    {
        private readonly Func<IType, SliceIdentifier> _assignIdentifierFunc;

        /// <summary>
        ///     Creates a new slice assignment with the given assignment function and description.
        /// </summary>
        /// <param name="assignIdentifierFunc">
        ///     A function that maps a type to a <see cref="SliceIdentifier"/>. Types that do not
        ///     match the pattern should return <see cref="SliceIdentifier.Ignore"/>.
        /// </param>
        /// <param name="description">
        ///     A human-readable description of the assignment (e.g. <c>matching "App.(*)"</c>).
        /// </param>
        public SliceAssignment(
            Func<IType, SliceIdentifier> assignIdentifierFunc,
            string description
        )
        {
            _assignIdentifierFunc = assignIdentifierFunc;
            Description = description;
        }

        /// <inheritdoc />
        public string Description { get; }

        /// <summary>
        ///     Applies the assignment function to all types, grouping them into slices.
        ///     Types that produce the same <see cref="SliceIdentifier"/> (as determined by
        ///     <see cref="SliceIdentifierComparer"/>) are grouped into the same slice.
        /// </summary>
        /// <param name="types">The types to assign to slices.</param>
        /// <returns>The resulting slices, including ignored slices (filtered out later).</returns>
        public IEnumerable<Slice> Apply(IEnumerable<IType> types)
        {
            return types.GroupBy(
                _assignIdentifierFunc,
                (identifier, enumerable) => new Slice(identifier, enumerable),
                SliceIdentifier.Comparer
            );
        }
    }
}
