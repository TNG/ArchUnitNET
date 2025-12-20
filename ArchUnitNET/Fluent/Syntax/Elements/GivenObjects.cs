using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Exceptions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements
{
    public abstract class GivenObjects<TGivenRuleTypeThat, TRuleTypeShould, TRuleType>
        : SyntaxElement<TRuleType>,
            IObjectProvider<TRuleType>
        where TRuleType : ICanBeAnalyzed
    {
        protected GivenObjects(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<TRuleType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public abstract TGivenRuleTypeThat That();
        public abstract TRuleTypeShould Should();

        public override string Description => ObjectProvider.Description;

        public IEnumerable<TRuleType> GetObjects(Architecture architecture)
        {
            if (PartialArchRuleConjunction != null)
            {
                throw new CannotGetObjectsOfCombinedArchRuleException(
                    "GetObjects cannot be called on a combined arch rule, because the analyzed objects may be of different types."
                );
            }
            return ObjectProvider.GetObjects(architecture);
        }

        public string FormatDescription(
            string emptyDescription,
            string singleDescription,
            string multipleDescription
        ) => $"{multipleDescription} {Description}";
    }
}
