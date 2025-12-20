using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class GivenClasses : GivenObjects<GivenClassesThat, ClassesShould, Class>
    {
        internal GivenClasses(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Class> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenClassesThat That() =>
            new GivenClassesThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override ClassesShould Should() =>
            new ClassesShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
