using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Predicates;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public sealed class GivenClassesThat : AddClassPredicate<Class, GivenClassesConjunction>
    {
        public GivenClassesThat(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }

        protected override GivenClassesConjunction CreateNextElement(IPredicate<Class> predicate)
        {
            _ruleCreator.AddPredicate(predicate);
            return new GivenClassesConjunction(_ruleCreator);
        }
    }
}
