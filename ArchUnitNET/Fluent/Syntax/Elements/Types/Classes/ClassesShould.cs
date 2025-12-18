using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Conditions;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShould : AddClassCondition<ClassesShouldConjunction>
    {
        public ClassesShould(IArchRuleCreator<Class> ruleCreator)
            : base(ruleCreator) { }

        protected override ClassesShouldConjunction CreateNextElement(
            IOrderedCondition<Class> condition
        )
        {
            _ruleCreator.AddCondition(condition);
            return new ClassesShouldConjunction(_ruleCreator);
        }
    }
}
