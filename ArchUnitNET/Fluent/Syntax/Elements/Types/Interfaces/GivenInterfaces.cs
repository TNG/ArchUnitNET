using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using JetBrains.Annotations;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public class GivenInterfaces : GivenObjects<GivenInterfacesThat, InterfacesShould, Interface>
    {
        internal GivenInterfaces(
            [CanBeNull] PartialArchRuleConjunction partialArchRuleConjunction,
            IObjectProvider<Interface> objectProvider
        )
            : base(partialArchRuleConjunction, objectProvider) { }

        public override GivenInterfacesThat That() =>
            new GivenInterfacesThat(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("that")
            );

        public override InterfacesShould Should() =>
            new InterfacesShould(
                PartialArchRuleConjunction,
                ObjectProvider.WithDescriptionSuffix("should")
            );
    }
}
