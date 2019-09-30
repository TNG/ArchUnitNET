using ArchUnitNET.Domain;

namespace ArchUnitNET.Fluent.Syntax.Elements.Types.Classes
{
    public class ClassesShouldConjunctionWithReason : ObjectsShouldConjunctionWithReason<ClassesShould, Class>
    {
        public ClassesShouldConjunctionWithReason(IArchRuleCreator<Class> ruleCreator) : base(ruleCreator)
        {
        }
    }
}