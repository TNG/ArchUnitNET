using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Interfaces
{
    public sealed class GivenInterfacesThat
        : AddInterfacePredicate<Interface, GivenInterfacesConjunction>
    {
        public GivenInterfacesThat(IArchRuleCreator<Interface> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenInterfacesConjunction CreateNextElement(
            IPredicate<Interface> predicate
        )
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenInterfacesConjunction(_ruleCreator);
        }
    }
}
