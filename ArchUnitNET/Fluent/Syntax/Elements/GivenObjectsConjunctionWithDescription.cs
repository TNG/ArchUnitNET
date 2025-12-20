using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Exceptions;
using ArchUnitNET.Fluent.Predicates;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjectsConjunctionWithDescription<
        TGivenRuleTypeThat,
        TRuleTypeShould,
        TRuleType
    > : SyntaxElement<TRuleType>, IObjectProvider<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjectsConjunctionWithDescription(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider,
            IPredicate<TRuleType> predicate
        )
            : base(partialArchRuleConjunction, objectProvider)
        {
            Predicate = predicate;
        }

        protected IPredicate<TRuleType> Predicate { get; }

        public abstract TGivenRuleTypeThat And();
        public abstract TGivenRuleTypeThat Or();
        public abstract TRuleTypeShould Should();

        public override string Description =>
            $"{ObjectProvider.Description} {Predicate.Description}";

        public IEnumerable<TRuleType> GetObjects(Architecture architecture)
        {
            if (PartialArchRuleConjunction != null)
            {
                throw new CannotGetObjectsOfCombinedArchRuleException(
                    "GetObjects cannot be called on a combined arch rule, because the analyzed objects may be of different types."
                );
            }
            return Predicate.GetMatchingObjects(
                ObjectProvider.GetObjects(architecture),
                architecture
            );
        }

        public string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        ) => $"{multipleDescription} {Description}";
    }
}
