using System.Collections.Generic;
using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Exceptions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types
{
    public sealed class GivenTypes : GivenObjects<GivenTypesThat, TypesShould, IType>
    {
        internal GivenTypes(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<IType> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenTypesThat That() =>
            new GivenTypesThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override TypesShould Should() =>
            new TypesShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
