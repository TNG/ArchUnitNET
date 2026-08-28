using System;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Slices;
using Xunit;

namespace ArchUnitNETTests.Fluent.Slices
{
    /// <summary>
    ///     Covers the fluent-API guard in <see cref="SliceRuleCreator" /> and the plain
    ///     domain-type members of <see cref="Slice" />, <see cref="SliceIdentifier" /> and
    ///     <see cref="SliceIdentifierComparer" />, and <see cref="GivenSlices" /> that
    ///     no slice-diagram or rule-evaluation fixture happens to exercise.
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
            // SlicesTestArchitecture has no interfaces, so use a richer architecture: this test
            // is about Slice's own filtering, not slice-assignment semantics.
            var richArchitecture = StaticTestArchitectures.ArchUnitNETTestArchitecture;
            var classType = richArchitecture.Classes.First();
            var interfaceType = richArchitecture.Interfaces.First();
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

            Assert.Equal("SomeSlice", slice.ToString());
            Assert.Equal(slice.Description, slice.ToString());
        }

        /// <remarks>
        ///     Slice.Equals compares its Types collection by reference, not by content, so
        ///     <c>differentTypesInstance</c> below is unequal despite also being empty. That
        ///     relies on the two <c>new IType[0]</c> expressions yielding distinct references --
        ///     true today, but it would silently stop testing anything if empty arrays were ever
        ///     interned, so the two instances are kept deliberately separate and named for it.
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
            Assert.False(slice.Equals(differentTypesInstance));
            Assert.False(slice.Equals(differentIdentifier));
        }

        // --- SliceIdentifier / SliceIdentifierComparer equality -----------------------

        [Fact]
        public void SliceIdentifier_CompareTo_Null_ReturnsFalse()
        {
            Assert.False(SliceIdentifier.Of("A").CompareTo(null));
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
