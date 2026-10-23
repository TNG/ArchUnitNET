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
            return types
                .Select(type => (Type: type, Identifier: _assignIdentifierFunc(type)))
                .GroupBy(
                    assigned => assigned.Identifier,
                    (identifier, assigned) => CreateSlice(identifier, assigned.ToList()),
                    SliceIdentifier.Comparer
                );
        }

        private Slice CreateSlice(
            SliceIdentifier identifier,
            List<(IType Type, SliceIdentifier Identifier)> assigned
        )
        {
            // The types of one slice can have matched different prefixes, e.g. with a leading
            // ".."; such a slice has no single namespace to be nested under.
            var prefixes = assigned
                .Select(typeAndIdentifier => typeAndIdentifier.Identifier.MatchedPrefix)
                .Distinct()
                .ToList();
            var types = assigned.Select(typeAndIdentifier => typeAndIdentifier.Type).ToList();
            return new Slice(identifier, types)
            {
                DiagramNamespace = prefixes.Count == 1 ? prefixes[0] : null,
                NestByDefault = _nestByDefault,
            };
        }
    }
}
