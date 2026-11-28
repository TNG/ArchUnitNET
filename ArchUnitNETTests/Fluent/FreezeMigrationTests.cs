using System.Collections.Generic;
using System.Linq;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Freeze;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Freeze.FreezingArchRule;

namespace ArchUnitNETTests.Fluent
{
    /// <summary>
    /// Frozen rules identify types and members as "FullName, AssemblySimpleName". Entries written
    /// before that (full name only) are migrated on the first run of each rule.
    /// </summary>
    public class FreezeMigrationTests
    {
        private const string ThisAssembly = "ArchUnitNETTests";

        private static readonly string UniqueNameA = typeof(UniqueA).FullName;
        private static readonly string UniqueNameB = typeof(UniqueB).FullName;

        private static readonly Architecture ThisArchitecture = new ArchLoader()
            .LoadAssembly(typeof(FreezeMigrationTests).Assembly)
            .Build();

        private static readonly IArchRule UniqueRuleA = Types()
            .That()
            .Are(typeof(UniqueA))
            .Should()
            .NotBePublic()
            .Because("A");

        private static readonly IArchRule UniqueRuleB = Types()
            .That()
            .Are(typeof(UniqueB))
            .Should()
            .NotBePublic()
            .Because("B");

        private static readonly IArchRule SliceRule = SliceRuleDefinition
            .Slices()
            .Matching("SlicesTestAssembly.MultipleSubnamespaces.(**)")
            .Should()
            .NotDependOnEachOther();

        private static readonly string[] SliceNames =
        {
            "Slice Slice3",
            "Slice Slice3.Group1",
            "Slice Slice2",
            "Slice Slice2.Service",
            "Slice Slice1",
            "Slice Slice1.Service",
        };

        [Theory]
        [InlineData(StoreKind.Json, DuplicateKind.Type, 1)]
        [InlineData(StoreKind.Xml, DuplicateKind.Type, 1)]
        [InlineData(StoreKind.Json, DuplicateKind.Member, 1)]
        [InlineData(StoreKind.Xml, DuplicateKind.Member, 1)]
        [InlineData(StoreKind.Json, DuplicateKind.Type, 2)]
        [InlineData(StoreKind.Xml, DuplicateKind.Type, 2)]
        public void LegacyEntryFreezesAndMigratesAllDuplicates(
            StoreKind kind,
            DuplicateKind duplicate,
            int legacyEntries
        )
        {
            // A legacy entry can't tell the duplicates apart, so it freezes all of them, as the
            // old identifiers did.
            using (var dir = new TempStoreDirectory())
            {
                var rule = DuplicateFullNames.Rule(duplicate);
                var path = dir.CreateFile(
                    FreezeTestStores.FileName(kind),
                    FreezeTestStores.HandWritten(
                        kind,
                        (
                            rule.Description,
                            Enumerable
                                .Repeat(DuplicateFullNames.FullName(duplicate), legacyEntries)
                                .ToArray()
                        )
                    )
                );

                var results = Freeze(rule, FreezeTestStores.Create(kind, path))
                    .Evaluate(DuplicateFullNames.Architecture)
                    .ToList();

                Assert.Equal(2, results.Count);
                Assert.All(results, result => Assert.True(result.Passed));
                Assert.Equal(
                    BothDuplicateIdentifiers(duplicate),
                    FreezeTestStores.ReadViolations(kind, path, rule.Description).OrderBy(v => v)
                );
            }
        }

        [Theory]
        [InlineData(StoreKind.Json)]
        [InlineData(StoreKind.Xml)]
        public void LegacyStoreIsMigratedOnFirstRun(StoreKind kind)
        {
            using (var dir = new TempStoreDirectory())
            {
                var path = dir.CreateFile(
                    FreezeTestStores.FileName(kind),
                    FreezeTestStores.HandWritten(
                        kind,
                        (UniqueRuleA.Description, new[] { UniqueNameA })
                    )
                );

                Freeze(UniqueRuleA, FreezeTestStores.Create(kind, path)).Check(ThisArchitecture);

                Assert.Equal(
                    new[] { UniqueNameA + ", " + ThisAssembly },
                    FreezeTestStores.ReadViolations(kind, path, UniqueRuleA.Description)
                );
            }
        }

        [Theory]
        [InlineData(StoreKind.Json)]
        [InlineData(StoreKind.Xml)]
        public void LegacyEntryThatNoLongerFailsIsDroppedOnMigration(StoreKind kind)
        {
            using (var dir = new TempStoreDirectory())
            {
                var path = dir.CreateFile(
                    FreezeTestStores.FileName(kind),
                    FreezeTestStores.HandWritten(
                        kind,
                        (UniqueRuleA.Description, new[] { UniqueNameA, "Some.Removed.Type" })
                    )
                );

                Freeze(UniqueRuleA, FreezeTestStores.Create(kind, path)).Check(ThisArchitecture);

                Assert.Equal(
                    new[] { UniqueNameA + ", " + ThisAssembly },
                    FreezeTestStores.ReadViolations(kind, path, UniqueRuleA.Description)
                );
            }
        }

        [Theory]
        [InlineData(StoreKind.Json)]
        [InlineData(StoreKind.Xml)]
        public void RuleThatIsNotEvaluatedStaysLegacyInTheStore(StoreKind kind)
        {
            using (var dir = new TempStoreDirectory())
            {
                var path = dir.CreateFile(
                    FreezeTestStores.FileName(kind),
                    FreezeTestStores.HandWritten(
                        kind,
                        (UniqueRuleA.Description, new[] { UniqueNameA }),
                        (UniqueRuleB.Description, new[] { UniqueNameB })
                    )
                );
                var untouchedBefore = FreezeTestStores.ReadRaw(kind, path, UniqueRuleB.Description);

                Freeze(UniqueRuleA, FreezeTestStores.Create(kind, path)).Check(ThisArchitecture);

                var untouchedAfter = FreezeTestStores.ReadRaw(kind, path, UniqueRuleB.Description);
                Assert.Equal(untouchedBefore.ToString(), untouchedAfter.ToString());
                Assert.Equal(
                    new[] { UniqueNameB },
                    FreezeTestStores.ReadViolations(kind, path, UniqueRuleB.Description)
                );
                Assert.Equal(
                    new[] { UniqueNameA + ", " + ThisAssembly },
                    FreezeTestStores.ReadViolations(kind, path, UniqueRuleA.Description)
                );
            }
        }

        [Theory]
        [InlineData(StoreKind.Json)]
        [InlineData(StoreKind.Xml)]
        public void LegacySliceRuleStillMatchesExactly(StoreKind kind)
        {
            using (var dir = new TempStoreDirectory())
            {
                var path = dir.CreateFile(
                    FreezeTestStores.FileName(kind),
                    FreezeTestStores.HandWritten(kind, (SliceRule.Description, SliceNames))
                );

                Freeze(SliceRule, FreezeTestStores.Create(kind, path))
                    .Check(StaticTestArchitectures.SlicesTestArchitecture);

                Assert.Equal(
                    SliceNames.OrderBy(n => n),
                    FreezeTestStores
                        .ReadViolations(kind, path, SliceRule.Description)
                        .OrderBy(n => n)
                );
            }
        }

        [Fact]
        public void CustomStoreMatchesNewFormEntriesWithPlainStringIdentifiers()
        {
            var store = new CustomStore(BothDuplicateIdentifiers(DuplicateKind.Type));

            var results = Freeze(DuplicateFullNames.Rule(DuplicateKind.Type), store)
                .Evaluate(DuplicateFullNames.Architecture)
                .ToList();

            Assert.Equal(2, results.Count);
            Assert.All(results, result => Assert.True(result.Passed));
        }

        [Fact]
        public void CustomStoreWithFullNamesIsMigrated()
        {
            var store = new CustomStore(UniqueNameA);

            var results = Freeze(UniqueRuleA, store).Evaluate(ThisArchitecture).ToList();

            Assert.True(Assert.Single(results).Passed);
            Assert.Equal(new[] { UniqueNameA + ", " + ThisAssembly }, store.Stored);
        }

        [Fact]
        public void CustomStoreIsGivenIdentifiersInNewForm()
        {
            var store = new CustomStore();

            Freeze(DuplicateFullNames.Rule(DuplicateKind.Type), store)
                .Check(DuplicateFullNames.Architecture);

            Assert.Equal(
                BothDuplicateIdentifiers(DuplicateKind.Type),
                store.Stored.OrderBy(v => v)
            );
        }

        [Fact]
        public void LegacyEntryDoesNotFreezeCustomIdentifierOfAnalyzedObject()
        {
            // A custom rule may identify an analyzed object differently; its legacy form is then
            // not the full name, so only an exact match freezes it.
            var store = new CustomStore(UniqueNameA);
            var rule = new CustomRule(("custom:" + UniqueNameA, typeof(UniqueA)));

            var results = Freeze(rule, store).Evaluate(ThisArchitecture).ToList();

            Assert.False(Assert.Single(results).Passed);
        }

        [Fact]
        public void LegacyEntryFreezesCustomRuleUsingTheNewForm()
        {
            var store = new CustomStore(UniqueNameA);
            var rule = new CustomRule((UniqueNameA + ", " + ThisAssembly, typeof(UniqueA)));

            var results = Freeze(rule, store).Evaluate(ThisArchitecture).ToList();

            Assert.True(Assert.Single(results).Passed);
        }

        [Fact]
        public void FullNamesNeverContainTheAssemblySeparator()
        {
            // Legacy entries are recognized by matching a bare full name, which a new-form entry
            // can never equal as long as full names don't contain ", ". See FreezingArchRule.
            var architectures = new[]
            {
                StaticTestArchitectures.FullArchUnitNETArchitecture,
                StaticTestArchitectures.FunctionPointerArchitecture,
                StaticTestArchitectures.LoaderTestArchitecture,
            };
            var fullNames = architectures.SelectMany(architecture =>
                architecture
                    .Types.Concat(architecture.ReferencedTypes)
                    .SelectMany(type =>
                        new[] { type.FullName }.Concat(type.Members.Select(m => m.FullName))
                    )
            );

            Assert.All(fullNames, fullName => Assert.DoesNotContain(", ", fullName));
        }

        private static string[] BothDuplicateIdentifiers(DuplicateKind duplicate) =>
            new[]
            {
                DuplicateFullNames.Identifier(duplicate, DuplicateFullNames.AssemblyA),
                DuplicateFullNames.Identifier(duplicate, DuplicateFullNames.AssemblyB),
            };

        private sealed class CustomStore : IViolationStore
        {
            private readonly List<string> _frozen;

            public CustomStore(params string[] frozen)
            {
                _frozen = frozen.ToList();
            }

            public List<string> Stored { get; private set; } = new List<string>();

            public bool RuleAlreadyFrozen(IArchRule rule) => _frozen.Any();

            public IEnumerable<StringIdentifier> GetFrozenViolations(IArchRule rule) =>
                _frozen.Select(v => new StringIdentifier(v));

            public void StoreCurrentViolations(
                IArchRule rule,
                IEnumerable<StringIdentifier> violations
            ) => Stored = violations.Select(v => v.Identifier).ToList();
        }

        // A user-defined rule that fails once per given identifier, for the given type.
        private sealed class CustomRule : IArchRule
        {
            private readonly (string identifier, System.Type type)[] _violations;

            public CustomRule(params (string identifier, System.Type type)[] violations)
            {
                _violations = violations;
            }

            public string Description => "Custom rule";

            public bool HasNoViolations(Architecture architecture) =>
                Evaluate(architecture).All(result => result.Passed);

            public IEnumerable<EvaluationResult> Evaluate(Architecture architecture) =>
                _violations.Select(violation => new EvaluationResult(
                    architecture.GetITypeOfType(violation.type),
                    new StringIdentifier(violation.identifier),
                    false,
                    violation.identifier,
                    this,
                    architecture
                ));

            public CombinedArchRuleDefinition And() => throw new System.NotSupportedException();

            public CombinedArchRuleDefinition Or() => throw new System.NotSupportedException();

            public IArchRule And(IArchRule archRule) => throw new System.NotSupportedException();

            public IArchRule Or(IArchRule archRule) => throw new System.NotSupportedException();
        }

        public class UniqueA { }

        public class UniqueB { }
    }
}
