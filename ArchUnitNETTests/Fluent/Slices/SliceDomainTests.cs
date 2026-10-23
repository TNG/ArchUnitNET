using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Slices;
using SlicesTestAssembly.MixedTypes;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    /// Covers the fluent-API guard in <see cref="SliceRuleCreator" /> and the plain
    /// domain-type members of <see cref="Slice" />, <see cref="SliceIdentifier" /> and
    /// <see cref="SliceIdentifierComparer" />, and <see cref="GivenSlices" /> that
    /// no slice-diagram or rule-evaluation fixture happens to exercise.
    /// </summary>
    public class SliceDomainTests
    {
        private static Architecture Architecture => StaticTestArchitectures.SlicesTestArchitecture;

        private const string Root = "SlicesTestAssembly.MultipleSubnamespaces.";

        // --- SliceRuleCreator.GetSlices() without a slice assignment ------------------

        [Fact]
        public void GetSlices_WithoutSliceAssignment_Throws()
        {
            var creator = new SliceRuleCreator();
            var ex = Assert.Throws<InvalidOperationException>(() =>
                creator.GetSlices(Architecture)
            );
            Assert.Equal(
                "The Slice Assignment has to be set before GetSlices() can be called.",
                ex.Message
            );
        }

        // --- Domain.Slice members ------------------------------------------------------

        [Fact]
        public void Slice_ClassesAndInterfaces_FilterByType()
        {
            var classType = Architecture.GetClassOfType(typeof(MixedTypesClass));
            var interfaceType = Architecture.GetInterfaceOfType(typeof(IMixedTypesInterface));
            var slice = new Slice(
                SliceIdentifier.Of("Mixed"),
                new IType[] { classType, interfaceType }
            );

            Assert.Equal(new[] { classType }, slice.Classes);
            Assert.Equal(new[] { interfaceType }, slice.Interfaces);
        }

        [Fact]
        public void Slice_BackwardsDependencies_AggregatesFromTypes()
        {
            var type = Architecture.Classes.First(c => c.BackwardsDependencies.Any());
            var slice = new Slice(SliceIdentifier.Of("Backwards"), new IType[] { type });

            Assert.Equal(type.BackwardsDependencies, slice.BackwardsDependencies);
        }

        [Fact]
        public void Slice_ToString_ReturnsDescription()
        {
            var slice = new Slice(SliceIdentifier.Of("SomeSlice"), Array.Empty<IType>());

            Assert.Equal("Slice SomeSlice", slice.ToString());
            Assert.Equal(slice.Description, slice.ToString());
        }

        /// <remarks>
        /// Slice.Equals looks at the identifier alone, so <c>differentTypesInstance</c> below is
        /// equal despite holding a distinct Types instance. The two <c>new IType[0]</c>
        /// expressions are kept deliberately separate and named for it: they are what would make
        /// this fail again if Types were ever compared by reference.
        /// </remarks>
        [Fact]
        public void Slice_Equals_Branches()
        {
            var types = new IType[0];
            var equalButDistinctTypes = new IType[0];
            Assert.NotSame(types, equalButDistinctTypes);

            var slice = new Slice(SliceIdentifier.Of("A"), types);
            var sameIdentifierAndTypes = new Slice(SliceIdentifier.Of("A"), types);
            var differentTypesInstance = new Slice(SliceIdentifier.Of("A"), equalButDistinctTypes);
            var differentIdentifier = new Slice(SliceIdentifier.Of("B"), types);

            Assert.False(slice.Equals(null));
            Assert.True(slice.Equals(slice));
            Assert.False(slice.Equals("not a slice"));
            Assert.True(slice.Equals(sameIdentifierAndTypes));
            Assert.True(slice.Equals(differentTypesInstance));
            Assert.Equal(slice.GetHashCode(), differentTypesInstance.GetHashCode());
            Assert.False(slice.Equals(differentIdentifier));
        }

        // --- SliceIdentifier / SliceIdentifierComparer equality -----------------------

        [Fact]
        public void SliceIdentifier_CompareTo_Null_ReturnsFalse()
        {
            Assert.False(SliceIdentifier.Of("A").CompareTo(null));
        }

        [Fact]
        public void SliceIdentifier_CompareTo_OneIgnored_ReturnsFalse()
        {
            // Same parts, so only the Ignored flag tells the two apart.
            Assert.False(SliceIdentifier.Of("Ignored").CompareTo(SliceIdentifier.Ignore()));
            Assert.False(SliceIdentifier.Ignore().CompareTo(SliceIdentifier.Of("Ignored")));
        }

        [Fact]
        public void SliceIdentifier_Equals_Branches()
        {
            var identifier = SliceIdentifier.Of("A");

            Assert.False(identifier.Equals(null));
            Assert.True(identifier.Equals(identifier));
            Assert.False(identifier.Equals("not an identifier"));
            Assert.True(identifier.Equals(SliceIdentifier.Of("A")));
            Assert.False(identifier.Equals(SliceIdentifier.Of("B")));
        }

        [Fact]
        public void SliceIdentifier_Equals_ComparesParts()
        {
            var identifier = SliceIdentifier.Of(new[] { "A", "B" });

            Assert.Equal(SliceIdentifier.Of(new[] { "A", "B" }), identifier);
            Assert.Equal(
                SliceIdentifier.Of(new[] { "A", "B" }).GetHashCode(),
                identifier.GetHashCode()
            );
            Assert.NotEqual(SliceIdentifier.Of(new[] { "B", "A" }), identifier);
            Assert.NotEqual(SliceIdentifier.Of("A.B"), identifier);
            Assert.NotEqual(SliceIdentifier.Of(new[] { "A", "B", "C" }), identifier);
        }

        [Fact]
        public void SliceIdentifier_Equals_IgnoresDiagramData()
        {
            // Where a diagram draws the slice is no part of its identity.
            var nested = SliceIdentifier.OfMatch(new[] { "A", "B" }, null, "X.", "A.B");

            Assert.Equal(SliceIdentifier.Of(new[] { "A", "B" }), nested);
            Assert.Equal(
                SliceIdentifier.Of(new[] { "A", "B" }).GetHashCode(),
                nested.GetHashCode()
            );
            Assert.Equal("Slice A - B", nested.Description);
        }

        [Fact]
        public void SliceIdentifier_OfString_IsASingleSlicePart()
        {
            var ofString = SliceIdentifier.Of("Orders");
            var ofParts = SliceIdentifier.Of(new[] { "Orders" });

            Assert.Equal(new[] { "Orders" }, ofString.Parts);
            Assert.Equal(ofParts, ofString);
            Assert.Equal("Slice Orders", ofString.Description);
            Assert.Equal("Slice Orders", ofParts.Description);
        }

        [Fact]
        public void SliceIdentifier_Description_PrefixesTheJoinedParts()
        {
            Assert.Equal("Slice Orders.Web", SliceIdentifier.Of("Orders.Web").Description);
            Assert.Equal(
                "Slice Orders - Http",
                SliceIdentifier.Of(new[] { "Orders", "Http" }).Description
            );
        }

        [Fact]
        public void SliceIdentifier_Of_RejectsMissingParts()
        {
            Assert.Throws<ArgumentNullException>(() =>
                SliceIdentifier.Of((System.Collections.Generic.IEnumerable<string>)null)
            );
            Assert.Throws<ArgumentException>(() => SliceIdentifier.Of(new string[0]));
            Assert.Throws<ArgumentException>(() => SliceIdentifier.Of(new[] { "A", null }));
        }

        [Fact]
        public void SliceIdentifier_PartsContainingTheSeparator_AreADifferentSlice()
        {
            // Both are described "Slice A - B", but they are different slices.
            var onePart = SliceIdentifier.Of(new[] { "A - B" });
            var twoParts = SliceIdentifier.Of(new[] { "A", "B" });

            Assert.Equal(onePart.Description, twoParts.Description);
            Assert.NotEqual(onePart, twoParts);
            Assert.False(SliceIdentifier.Comparer.Equals(onePart, twoParts));
        }

        [Fact]
        public void SliceIdentifier_ToString_IsTheDescription()
        {
            Assert.Equal("Slice Orders", SliceIdentifier.Of("Orders").ToString());
        }

        // --- Violation keys, which violation stores keep ---------------------------------

        [Fact]
        public void ViolationKey_IsTheDescription()
        {
            var keys = SliceRuleDefinition
                .Slices()
                .Matching(Root + "(*)")
                .Should()
                .NotDependOnEachOther()
                .Evaluate(Architecture)
                .Select(result => result.EvaluatedObjectIdentifier.Identifier);

            Assert.Contains("Slice Slice1", keys);
        }

        [Fact]
        public void ViolationKey_OfSeveralParts_IsTheDescription()
        {
            // Patterns capture a single part, so a custom assignment builds several.
            var creator = new SliceRuleCreator();
            creator.SetSliceAssignment(
                new SliceAssignment(
                    type => SliceIdentifier.Of(new[] { "Custom", type.Name }),
                    "by name"
                )
            );

            var keys = new SlicesShould(creator)
                .NotDependOnEachOther()
                .Evaluate(Architecture)
                .Select(result => result.EvaluatedObjectIdentifier.Identifier);

            Assert.Contains("Slice Custom - Slice1Class", keys);
        }

        // --- SlicedBy ------------------------------------------------------------------

        [Fact]
        public void SlicedBy_NamesSlicesAfterTheTypeLikeEveryOtherSlice()
        {
            var type = Architecture.GetClassOfType(typeof(MixedTypesClass));

            var slice = Assert.Single(new IType[] { type }.SlicedBy(""));

            Assert.Equal(new[] { type.FullName }, slice.Identifier.Parts);
            Assert.Equal("Slice " + type.FullName, slice.Description);
        }

        [Fact]
        public void SliceIdentifierComparer_Equals_NullFirstArgument_ReturnsFalse()
        {
            Assert.False(SliceIdentifier.Comparer.Equals(null, SliceIdentifier.Of("A")));
            Assert.True(
                SliceIdentifier.Comparer.Equals(SliceIdentifier.Of("A"), SliceIdentifier.Of("A"))
            );
        }

        // --- GivenSlices description plumbing -----------------------------------------

        [Fact]
        public void GivenSlices_Description_ReflectsRuleCreatorDescription()
        {
            var slices = SliceRuleDefinition.Slices().Matching(Root + "(*)");

            Assert.Equal("Slices matching \"" + Root + "(*)\"", slices.Description);
        }

        [Fact]
        public void GivenSlices_FormatDescription_PrependsMultipleDescription()
        {
            var slices = SliceRuleDefinition.Slices().Matching(Root + "(*)");

            var formatted = slices.FormatDescription("empty", "single", "multiple");

            Assert.Equal("multiple " + slices.Description, formatted);
        }
    }
}
